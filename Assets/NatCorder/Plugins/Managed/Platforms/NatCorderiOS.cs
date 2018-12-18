/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using AOT;
    using UnityEngine;
    using System;
    using NatCamU.Dispatch;
    using FramePool = System.Collections.Generic.Dictionary<System.IntPtr, UnityEngine.RenderTexture>;

    public sealed class NatCorderiOS : INatCorder {

        #region --Op vars--
        private Configuration configuration;
        private VideoCallback videoCallback;
        private IAudioSource audioSource;
        private MainDispatch dispatch;
        private static FramePool framePool = new FramePool();
        private static NatCorderiOS instance { get { return NatCorder.Implementation as NatCorderiOS; }}
        #endregion
        

        #region --Properties--
        public bool IsRecording { get { return NatCorderBridge.IsRecording();}}
        #endregion


        #region --Operations--

        public NatCorderiOS () {
            NatCorderBridge.Initialize(OnEncode, OnVideo, Application.persistentDataPath);
            RenderDispatch.Initialize();
            Debug.Log("NatCorder: Initialized NatCorder 1.2 iOS backend");
        }

        public void StartRecording (Configuration configuration, VideoCallback videoCallback, IAudioSource audioSource) {
            // Make sure that recording size is multiple of two
            configuration = new Configuration(2 * (configuration.width / 2), 2 * (configuration.height / 2), configuration.framerate, configuration.bitrate, configuration.keyframeInterval);
            // Save state
            this.dispatch = new MainDispatch();
            this.configuration = configuration;
            this.videoCallback = videoCallback;
            this.audioSource = audioSource;
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

        public void StopRecording () {
            if (audioSource != null) audioSource.Dispose();
            audioSource = null;
            NatCorderBridge.StopRecording();
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

        public void CommitFrame (Frame frame) {
            var handle = ((RenderTexture)frame).GetNativeTexturePtr();
            framePool.Add(handle, frame);
            NatCorderBridge.EncodeFrame(handle, frame.timestamp);
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            if (IsRecording) NatCorderBridge.EncodeSamples(sampleBuffer, timestamp);
        }
        #endregion


        #region --Callbacks--

        [MonoPInvokeCallback(typeof(NatCorderBridge.EncodeCallback))]
        private static void OnEncode (IntPtr frame) {
            instance.dispatch.Dispatch(() => {
                // Release RenderTexture
                var surface = framePool[frame];
                RenderTexture.ReleaseTemporary(surface);
                framePool.Remove(frame);
            });
        }

        [MonoPInvokeCallback(typeof(VideoCallback))]
        private static void OnVideo (string path) {
            instance.dispatch.Dispatch(() => instance.videoCallback(path));
            instance.dispatch.Dispose();
            instance.dispatch = null;
        }
        #endregion
    }
}