## NatCorder 1.2f1
+ We have significantly improved recording performance in iOS apps, especially in apps using the Metal API.
+ The Windows backend is no more experimental! It is now fully supported.
+ When recording with the `Replay` API, aspect fitting will be applied to prevent stretching in the video.
+ We have deprecated the Sharing API because we introduced a dedicated sharing API, [NatShare](https://github.com/olokobayusuf/NatShare-API).
+ Fixed audio stuttering in recorded videos on Windows.
+ Fixed tearing in recorded video when using OpenGL ES on iOS.
+ Fixed rare crash when recording in app that uses Metal API on iOS.
+ Fixed tearing and distortion in recorded video on macOS.
+ Fixed microphone audio recording from an older time in ReplayCam example.
+ Fixed crash when user tries to save video to camera roll from sharing dialog on iOS.
+ Deprecated `Microphone` API. Use `UnityEngine.Microphone` instead.
+ Deprecated `NatCorder.Verbose` flag.

## NatCorder 1.1f1
+ We have added a native macOS backend! The NatCorder recording API is now fully supported on macOS.
+ We have also added a native Windows backend! This backend is still experimental so it should not be used in production builds.
+ The Standalone backend (using FFmpeg) has been deprecated because we have added native Windows and macOS implementations.
+ We have significantly improved recording stability on Android especially for GPU-bound games.
+ Added support for different Unity audio DSP latency modes.
+ Added `sampleCount` property in `IAudioSource` interface.
+ Fixed crash when `StartRecording` is called on iOS running OpenGL ES2 or ES3.
+ Fixed rare crash on Android when recording with audio.
+ Fixed audio-video timing discrepancies on Android.
+ Fixed video tearing on Android when app does not use multithreaded rendering.
+ Fixed `FileUriExposedException` when `Sharing.Share` is called on Android 24 or newer.
+ Fixed `Sharing.GetThumbnail` not working on iOS.
+ Fixed `Sharing.SaveToCameraRoll` failing when permission is requested and approved on iOS.
+ Fixed `Sharing.SaveToCameraRoll` not working on Android.
+ Fixed rare crash on Android when a very short video (less than 1 second) is recorded.
+ Fixed build failing due to missing symbols for Sharing library on iOS.
+ Improved on microphone audio stuttering in ReplayCam example by adding minimal `Microphone` API in `NatCorderU.Extensions` namespace.
+ Refactored `Configuration.Default` to `Configuration.Screen`.
+ *Everything below*

## NatCorder 1.0f1
+ First release