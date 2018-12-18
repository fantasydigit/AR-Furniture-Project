# NatCorder API
NatCorder is a lightweight, easy-to-use, native video recording API for iOS and Android. NatCorder comes with a rich featureset including:
+ Record anything that can be rendered into a texture.
+ Control recording quality and file size with bitrate and keyframe interval.
+ Record at any resolution. You get to specify what resolution recording you want.
+ Get path to recorded video in device storage.
+ Record game audio with video.
+ Support for recording on macOS--in the Editor or in Standalone builds.
+ Support for recording on Windows--in the Editor or in Standalone builds.
+ Experimental support for recording on WebGL.

There are two main levels of control that developers would usually want with a recording API like NatCorder: the high-level screen recording API; and the lower-level texture recording API.

## Recording Replays
NatCorder provides a very simple screen recording API with the `Replay` class. To start recording from a game camera, simply call:
```csharp
Replay.StartRecording(Camera.main, Configuration.Default, OnReplay);
```

And once you are done with recording, call:
```csharp
Replay.StopRecording();
```

NatCorder will finish recording and invoke your replay callback with the path to the recorded video:
```csharp
void OnReplay (string path) {
    // Log
    Debug.Log("Saved recording to: "+path);
    // First, show the replay to the user
    Handheld.PlayFullScreenMovie(path);
}
```

You can pause the recording and resume it as you need to:
```csharp
Replay.PauseRecording();
// ...
Replay.ResumeRecording();
```

You can also record with game audio. You can either record audio from the entire scene (using the scene's `AudioListener`) or from a specific audio source (using `AudioSource`). The former is useful for recording typical replays, whereas the latter is useful for recording specific audio (like microphone audio):
```csharp
public AudioListener audioListener; // Set this to the scene's audio listener
// ...
Replay.StartRecording(Camera.main, Configuration.Default, OnReplay, audioListener);
```

Here is an example using a specific audio source instead:
```csharp
public AudioSource audioSource; // Set this to an audio source
// ...
Replay.StartRecording(Camera.main, Configuration.Default, OnReplay, audioSource);
```

**NOTE**: When recording with audio, make sure that your `AudioSource`s do **not** have `Bypass Effects` or `Bypass Listener Effects` switched on.

## Recording Textures
NatCorder provides a lower-level recording API exposed with the `NatCorder` class. At this low-level, NatCorder works by encoding video frames and audio samples on demand. Like the `Replay` API, you must call `StartRecording` to start recording:
```csharp
var configuration = new Configuration(texture.width, texture.height);
NatCorder.StartRecording(configuration, OnReplay);
```

Once this is done, you then manually record individual frames. To do so, you first acquire an encoder frame with `NatCorder.AcquireFrame`; then you blit what you want to record into the frame with `Graphics.Blit` (and optionally set the `frame.timestamp`); then you commit the frame for encoding with `NatCorder.CommitFrame`:
```csharp
WebCamTexture webcamPreview; // Start this somewhere

void Update () {
    // Check that we are recording, and that the webcamtexture was updated this frame
    if (!NatCorder.IsRecording || !webcamPreview.didUpdateThisFrame) return;
    // Acquire an encoder frame
    var frame = NatCorder.AcquireFrame();
    // Blit the webcam preview to the frame
    Graphics.Blit(webcamPreview, frame);
    // Now commit the frame to the encoder
    NatCorder.CommitFrame(frame);
}
```

**NOTE**: All frames that are acquired from NatCorder **must be committed**, or else there will be a resource leak.

## Limitations of the WebGL Backend
The WebGL backend is currently experimental. As a result, it has a few limitations in its operations. Firstly, it is an 'immediate-encode' backend. This means that video frames are encoded immediately they are committed to NatCorder. As a result, there is no support for custom frame timing (the `timestamp` value of a `Frame` is always ignored).

Secondly, the WebGL backend does not implement the `Sharing` API, so the sharing functions will not work on the platform. This is due to the platform's differences from mobile platforms.

Thirdly, because Unity does not support the `OnAudioFilterRead` callback on WebGL, we cannot record game audio on WebGL (using an `AudioSource` or `AudioListener`). This is a limitation of Unity's WebGL implementation. However, you can still record audio using the low-level `NatCorder.CommitSamples` API.

Videos recorded on WebGL may be recorded with the VP8/9 codec or H.264 codec, depending on the browser and device. Videos are always recorded in the `webm` container format.

## Using NatCorder with NatCam
If you use NatCorder with our NatCam camera API, then you will have to remove duplicate copies of libraries that are shared by both API's:
- `Dispatch.dll` in 'NatCorder > Plugins > Managed > Dispatch'
- `RenderDispatch.cs` in 'NatCorder > Plugins > Managed > Dispatch'
- `NatCamRenderPipeline.aar` in 'NatCorder > Plugins > Android'
- `libNatCamRenderPipeline.a` in 'NatCorder > Plugins > iOS'
- `NatCamRenderDispatch.h` in in 'NatCorder > Plugins > iOS'
- `NatCamRenderDispatch.m` in in 'NatCorder > Plugins > iOS'

## Tutorials
- [Unity Recording Made Easy](https://medium.com/@olokobayusuf/natcorder-unity-recording-made-easy-f0fdee0b5055)
- [Audio Workflows](https://medium.com/@olokobayusuf/natcorder-tutorial-audio-workflows-1cfce15fb86a)

## Requirements
- On Android, NatCorder requires API Level 18 and up
- On iOS, NatCorder requires iOS 7 and up
- On macOS, NatCorder requires macOS 10.13 and up
- On Windows, NatCorder requires Windows 8 and up

## Notes
- NatCorder doesn't have full support for recording UI canvases that are in Screen Space - Overlay mode. See [here](https://forum.unity3d.com/threads/render-a-canvas-to-rendertexture.272754/#post-1804847).
- On Android, it is strongly recommended to enable 'Multithreaded Rendering' in Player Settings. It greatly improves recording performance.
- When building for WebGL, make sure that 'Use Prebuild Engine' is disabled in Build Settings.
- When using the `Microphone` API to record on iOS, game audio might be routed to the earpiece speaker. [This plugin](https://github.com/cbaltzer/UnitySpeakerFix) can rectify that.

## Quick Tips
- Please peruse the included scripting reference [here](https://olokobayusuf.github.io/NatCorder-Docs/)
- To discuss or report an issue, visit Unity forums [here](https://forum.unity.com/threads/natcorder-video-recording-api.505146/)
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com)

Thank you very much!