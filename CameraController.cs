using System.Collections;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Storage;
using System.Threading.Tasks;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    private FirebaseStorage firebaseStorage;
    private StorageReference storageReference;
    public TextController textController;

    private bool isWebcamReady = false;

    private void Start()
    {
        // FirebaseStorage ve StorageReference nesnelerini oluşturun
        firebaseStorage = FirebaseStorage.DefaultInstance;
        storageReference = firebaseStorage.GetReferenceFromUrl("gs://unity-capture-project.appspot.com");
    }

    private void Update()
    {
        // Space tuşuna basıldığında görüntüyü dosyaya kaydet ve Firebase'e yükle
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isWebcamReady)
            {
                StartWebcam();
            }
            else
            {
                StartCoroutine(SaveAndUploadWebcamImage());
            }
        }
        if (textController.showTamamText)
        {
            webcamTexture.Stop();
            isWebcamReady = false;
            Application.Quit();
        }
    }

    private void StartWebcam()
    {
        // Tüm kameraları al
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("Kamera bulunamadı.");
            return;
        }

        // Birinci kamerayı aç
        webcamTexture = new WebCamTexture(devices[0].name);
        webcamTexture.Play();
        isWebcamReady = true;
    }

    private IEnumerator SaveAndUploadWebcamImage()
    {
        yield return new WaitForEndOfFrame();

        // Raw görüntüyü al
        Texture2D snapshot = new Texture2D(webcamTexture.width, webcamTexture.height);
        snapshot.SetPixels(webcamTexture.GetPixels());
        snapshot.Apply();

        // Görüntüyü dosyaya kaydet
        byte[] bytes = snapshot.EncodeToPNG();
        string fileName = "screenshot" + GetRandomFileName() + ".png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, bytes);

        // Firebase'e yükle
        UploadImageToFirebase(filePath, fileName);
    }

    private string GetRandomFileName()
    {
        // Rastgele 1-1000 arasında bir sayı oluşturun ve string olarak döndürün
        return Random.Range(1, 1001).ToString();
    }

    private void UploadImageToFirebase(string filePath, string fileName)
    {
        // Firebase Storage'a yükle
        storageReference.Child(fileName).PutFileAsync(filePath)
            .ContinueWith((Task<StorageMetadata> task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log("Yükleme başarısız oldu.");
                }
                else
                {
                    Debug.Log("Görüntü başarıyla Firebase'e yüklendi.");
                    // Yükleme tamamlandıktan sonra, gerekiyorsa işlemler yapabilirsiniz.
                }
            });
    }
}
