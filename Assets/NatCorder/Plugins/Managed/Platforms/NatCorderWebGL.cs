/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using NatCamU.Dispatch;

    public sealed class NatCorderWebGL : NatCorderOSX { // EXPERIMENTAL
        
        #region --Op vars--
        private readonly Material transformMat;        
        #endregion


        #region --Operations--

        public NatCorderWebGL () : base() {
            transformMat = new Material(Shader.Find("Hidden/NatCorder/Transform"));
            transformMat.EnableKeyword(@"PLATFORM_WEBGL");
            Debug.Log("NatCorder: Initialized NatCorder 1.2 WebGL backend with macOS implementation");
        }

        public override void StopRecording () {
            if (audioSource != null) audioSource.Dispose();
            #if UNITY_WEBGL && !UNITY_EDITOR
            var pathPtr = NatCorderBridge.StopRecording(); // Signature is different
            #else
            var pathPtr = IntPtr.Zero;
            #endif
            Texture2D.Destroy(framebuffer);
            audioSource = null;
            framebuffer = null;
            var callback = new GameObject("NatCorderWebGL Delegate").AddComponent<VideoDelegate>();
            callback.StartCoroutine(OnVideo(pathPtr, callback));
        }

        public override void CommitFrame (Frame frame) {
            // Invert
            var correctedFrame = AcquireFrame();
            correctedFrame.timestamp = frame.timestamp;
            Graphics.Blit(frame, correctedFrame, transformMat);
            RenderTexture.ReleaseTemporary(frame);
            // Commit
            base.CommitFrame(correctedFrame);
        }
        #endregion


        #region --Callbacks--

        private IEnumerator OnVideo (IntPtr pathPtr, VideoDelegate callback) {
            yield return new WaitUntil(() => Marshal.ReadIntPtr(pathPtr) != IntPtr.Zero);
            MonoBehaviour.Destroy(callback); // We don't need this anymore
            pathPtr = Marshal.ReadIntPtr(pathPtr);
            var path = Marshal.PtrToStringAnsi(pathPtr);
            videoCallback(path);
        }

        private class VideoDelegate : MonoBehaviour {}
        #endregion
    }
}