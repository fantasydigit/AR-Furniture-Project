/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core {

    using UnityEngine;
    using Platforms;
    using Docs;
    using NatCamU.Dispatch;

    [Doc(@"NatCorder")]
    public static class NatCorder {

        #region --Properties--
        /// <summary>
        /// The backing implementation NatCorder uses on this platform
        /// </summary>
        [Doc(@"Implementation")]
        public static readonly INatCorder Implementation;
        /// <summary>
        /// Is a video being recorded?
        /// </summary>
        [Doc(@"IsRecording")]
        public static bool IsRecording { get { return Implementation.IsRecording; }}
        #endregion


        #region --Operations--

        /// <summary>
        /// Start recording a video
        /// </summary>
        /// <param name="configuration">Configuration for recording</param>
        /// <param name="videoCallback">Callback to be invoked with the recorded video path</param>
        /// <param name="audioSource">Optional: Audio source for recording audio. If null, no audio is recorded.</param>
        [Doc(@"StartRecording"), Ref(@"Configuration", @"VideoCallback"), Code(@"RecordCamera")]
        public static void StartRecording (Configuration configuration, VideoCallback videoCallback, IAudioSource audioSource = null) {
            if (IsRecording) {
                Debug.LogError("NatCorder Error: Cannot start recording because NatCorder is already recording");
                return;
            } if (videoCallback == null) {
                Debug.LogError("NatCorder Error: Cannot record video without callback");
                return;
            }
            Implementation.StartRecording(configuration, videoCallback, audioSource);
        }

        /// <summary>
        /// Stop recording a video
        /// </summary>
        [Doc(@"StopRecording")]
        public static void StopRecording () {
            if (IsRecording) Implementation.StopRecording();
            else Debug.LogError("NatCorder Error: Cannot stop recording because NatCorder is not recording");            
        }

        /// <summary>
        /// Acquire a frame for encoding
        /// You will call Graphics::Blit on this frame
        /// </summary>
        [Doc(@"AcquireFrame", @"AcquireFrameDiscussion"), Ref(@"Frame"), Code(@"RecordCamera")]
        public static Frame AcquireFrame () {
            if (IsRecording) return Implementation.AcquireFrame();
            else Debug.LogError("NatCorder Error: Cannot acquire frame when NatCorder is not recording");
            return null;
        }

        /// <summary>
        /// Commit a frame for encoding
        /// </summary>
        [Doc(@"CommitFrame", @"CommitFrameDiscussion"), Ref(@"Frame"), Code(@"RecordCamera")]
        public static void CommitFrame (Frame frame) {
            if (IsRecording) Implementation.CommitFrame(frame);
            else {
                Debug.LogError("NatCorder Error: Cannot commit frame when NatCorder is not recording");
                RenderTexture.ReleaseTemporary(frame); // Release the frame
            }
        }

        /// <summary>
        /// Commit an audio sample buffer for encoding
        /// </summary>
        /// <param name="sampleBuffer">Raw PCM audio sample buffer, interleaved by channel</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds</param>
        [Doc(@"CommitSamples", @"CommitSamplesDiscussion"), Code(@"RecordPCM")]
        public static void CommitSamples (float[] sampleBuffer, long timestamp) {
            /**
             * We don't want to check if we're recording, because we can't (and won't) to guarantee that 
             * the calling thread is attached to the Java VM (the calling thread is probably the audio thread) 
             */
            Implementation.CommitSamples(sampleBuffer, timestamp);
        }
        #endregion


        #region --Initializer--

        static NatCorder () {
            // Create implementation for this platform
            Implementation = 
            #if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            new NatCorderOSX();
            #elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            new NatCorderWindows();
            #elif UNITY_IOS
            new NatCorderiOS();
            #elif UNITY_ANDROID
            new NatCorderAndroid();
            #elif UNITY_WEBGL
            new NatCorderWebGL();
            #else
            new NatCorderNull();
            #endif
            DispatchUtility.onQuit += () => { if (IsRecording) StopRecording(); }; // Stop recording if app is closed
        }
        #endregion
    }
}