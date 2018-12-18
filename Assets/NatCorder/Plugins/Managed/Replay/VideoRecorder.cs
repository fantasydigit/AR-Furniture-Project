/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core {

    using UnityEngine;

    public static partial class Replay {

        #region --Op vars--
        private static VideoRecorder videoRecorder;
        #endregion


        #region --Video recording--

        [AddComponentMenu(""), RequireComponent(typeof(Camera)), DisallowMultipleComponent]
        private sealed class VideoRecorder : MonoBehaviour {

            #region --Op vars--
            private long timestamp, lastTime = -1; // Used to support pausing and resuming
            private int frameSkip;
            private Material aspectFitter;
            #endregion


            #region --Operations--

            public void Prepare (Configuration configuration) {
                // Create aspect fitter
                aspectFitter = new Material(Shader.Find("Hidden/NatCorder/AspectFitter"));
                // Calculate aspect fitting
                var configAspect = (float)configuration.width / configuration.height;
                var cameraAspect = GetComponent<Camera>().aspect;
                if (configAspect > cameraAspect)
                    aspectFitter.SetVector("aspectCorrection", new Vector2(configAspect / cameraAspect, 1f));
                else 
                    aspectFitter.SetVector("aspectCorrection", new Vector2(1f, cameraAspect / configAspect));                
                // Set frame skip
                frameSkip = (int)Mathf.Max(Application.targetFrameRate, 30f) / configuration.framerate;
            }

            private void OnRenderImage (RenderTexture src, RenderTexture dst) {
                // Calculate time
                var frameTime = Platforms.Time.FrameTime;
                if (!IsPaused) timestamp += lastTime > 0 ? frameTime - lastTime : 0;
                lastTime = frameTime;
                // Blit to recording frame
                if (IsRecording && !IsPaused && UnityEngine.Time.frameCount % frameSkip == 0) {
                    var encoderFrame = NatCorder.AcquireFrame();
                    encoderFrame.timestamp = timestamp;
                    Graphics.Blit(src, encoderFrame, aspectFitter);
                    NatCorder.CommitFrame(encoderFrame);
                }
                // Blit to render pipeline
                Graphics.Blit(src, dst);
            }

            private void OnDestroy () {
                Material.Destroy(aspectFitter);
            }
            #endregion
        }
        #endregion
    }
}