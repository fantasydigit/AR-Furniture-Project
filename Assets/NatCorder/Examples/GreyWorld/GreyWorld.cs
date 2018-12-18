/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Core;

    public class GreyWorld : MonoBehaviour {

        /**
        * GreyWorld Example
        * ------------------
        * This example records a WebCamTexture using the low-level `NatCorder` API
        * The WebCamTexture is recorded with a filter applied (using a shader/material)
        * When the user touches the screen, the greyness value is set to 1, making the preview become grey, and recording is started
        * Then in `Update`, we blit the WebCamTexture to encoder surfaces (NatCorder.AcquireFrame) with the greyscale material/shader
        * When the user stops pressing the screen, we revert the greyness and stop recording
        */

        public RawImage rawImage;
        private CameraPreview cameraPreview;
        private float greyness;
        private const float GreySpeed = 3f;

        void Awake () {
            cameraPreview = rawImage.GetComponent<CameraPreview>();
        }

        void Update () {
            // Animate the greyness
            if (cameraPreview.cameraTexture && rawImage.texture == cameraPreview.cameraTexture) {
                var currentGreyness = rawImage.material.GetFloat("_Greyness");
                var targetGreyness = Mathf.Lerp(currentGreyness, greyness, GreySpeed * Time.deltaTime);
                rawImage.material.SetFloat("_Greyness", targetGreyness);
            }
            // Record frames
            if (NatCorder.IsRecording && cameraPreview.cameraTexture.didUpdateThisFrame) {
                // Acquire an encoder frame
                var frame = NatCorder.AcquireFrame();
                // Blit with the preview's greyscale material
                Graphics.Blit(cameraPreview.cameraTexture, frame, rawImage.material);
                // Commit the encoder frame for encoding
                NatCorder.CommitFrame(frame);
            }
        }

        public void StartRecording () {
            // Become grey
            greyness = 1f;
            // If the camera is in a potrait rotation, then we swap the width and height for recording
            bool isPortrait = cameraPreview.cameraTexture.videoRotationAngle == 90 || cameraPreview.cameraTexture.videoRotationAngle == 270;
            int recordingWidth = isPortrait ? cameraPreview.cameraTexture.height : cameraPreview.cameraTexture.width;
            int recordingHeight = isPortrait ? cameraPreview.cameraTexture.width : cameraPreview.cameraTexture.height;
            // Start recording
            NatCorder.StartRecording(new Configuration(recordingWidth, recordingHeight), OnVideo);
        }

        public void StopRecording () {
            // Revert to normal color
            greyness = 0f;
            // Stop recording
            NatCorder.StopRecording();
        }

        void OnVideo (string path) {
            Debug.Log("Saved recording to: "+path);
            #if UNITY_IOS || UNITY_ANDROID
            // Playback the video
            Handheld.PlayFullScreenMovie(path);
            #endif
        }
    }
}