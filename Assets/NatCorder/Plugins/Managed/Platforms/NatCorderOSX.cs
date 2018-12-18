/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

#define DEFERRED_READBACK // Saves some time at the cost of memory

namespace NatCorderU.Core.Platforms {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using NatCamU.Dispatch;

    public class NatCorderOSX : INatCorder {

        #region --Op vars--
        protected Configuration configuration;
        protected VideoCallback videoCallback;
        protected IAudioSource audioSource;
        protected Texture2D framebuffer;
        private MainDispatch dispatch;
        private static NatCorderOSX instance { get { return NatCorder.Implementation as NatCorderOSX; }}
        #endregion


        #region --Properties--
        public bool IsRecording { get { return NatCorderBridge.IsRecording(); }}
        #endregion


        #region --Operations--

        public NatCorderOSX () {
            var writePath = 
            #if UNITY_EDITOR
            System.IO.Directory.GetCurrentDirectory();
            #else
            Application.persistentDataPath;
            #endif
            NatCorderBridge.Initialize(null, OnVideo, writePath);
            Debug.Log("NatCorder: Initialized NatCorder 1.2 macOS backend");
        }

        public virtual void StartRecording (Configuration configuration, VideoCallback videoCallback, IAudioSource audioSource) {
            // Make sure that recording size is multiple of two
            configuration = new Configuration(2 * (configuration.width / 2), 2 * (configuration.height / 2), configuration.framerate, configuration.bitrate, configuration.keyframeInterval);
            // Save state
            this.dispatch = new MainDispatch();
            this.configuration = configuration;
            this.videoCallback = videoCallback;
            this.audioSource = audioSource;
            this.framebuffer = new Texture2D(configuration.width, configuration.height, TextureFormat.ARGB32, false);
            // Start recording
            NatCorderBridge.StartRecording(
                configuration.width,
                configuration.height,
                configuration.framerate,
                configuration.bitrate,
                configuration.keyframeInterval,
                audioSource != null,
                audioSource != null ? audioSource.sampleRate : 0,
                audioSource != null ? audioSource.sampleCount : 0,
                audioSource != null ? audioSource.channelCount : 0
            );
        }

        public virtual void StopRecording () {
            if (audioSource != null) audioSource.Dispose();
            audioSource = null;
            NatCorderBridge.StopRecording();
            Texture2D.Destroy(framebuffer);
            framebuffer = null;
        }

        public Frame AcquireFrame () {
            return new Frame(
                RenderTexture.GetTemporary(
                    configuration.width,
                    configuration.height,
                    24,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Default,
                    1
                ),
                Time.FrameTime
            );
        }

        public virtual void CommitFrame (Frame frame) {            
            // Submit
            #if DEFERRED_READBACK
            dispatch.Dispatch(() => dispatch.Dispatch(() => { // Defer readback for (at least) one frame
                if (!IsRecording) {
                    RenderTexture.ReleaseTemporary(frame);
                    return;
                }
            #endif
                var currentRT = RenderTexture.active;
                RenderTexture.active = frame;
                framebuffer.ReadPixels(new Rect(0, 0, configuration.width, configuration.height), 0, 0, false);
                RenderTexture.active = currentRT;
                RenderTexture.ReleaseTemporary(frame);            
                var pixelBuffer = framebuffer.GetRawTextureData();
                var bufferHandle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
                NatCorderBridge.EncodeFrame(bufferHandle.AddrOfPinnedObject(), frame.timestamp);
                bufferHandle.Free();
            #if DEFERRED_READBACK
            }));
            #endif
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            if (IsRecording) NatCorderBridge.EncodeSamples(sampleBuffer, timestamp);
        }
        #endregion


        #region --Callbacks--

        [MonoPInvokeCallback(typeof(VideoCallback))]
        private static void OnVideo (string path) {
            instance.dispatch.Dispatch(() => instance.videoCallback(path));
            instance.dispatch.Dispose();
            instance.dispatch = null;
        }
        #endregion
    }
}