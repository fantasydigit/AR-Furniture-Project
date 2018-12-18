/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Core;

    public class ReplayCam : MonoBehaviour {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using the high-level `Replay` API
        * We simply call `Replay.StartRecording` to start recording, and `Replay.StopRecording` to stop recording
        * When we want mic audio, we play the mic to an AudioSource and pass the audio source to `Replay.StartRecording`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

        public bool recordMicrophoneAudio;
        public AudioSource audioSource;

        public void StartRecording () {
            // Create a recording configuration
            const float DownscaleFactor = 2f / 3;
            var configuration = new Configuration((int)(Screen.width * DownscaleFactor), (int)(Screen.height * DownscaleFactor));
            // Start recording with microphone audio
            if (recordMicrophoneAudio) {
                StartMicrophone();
                Replay.StartRecording(Camera.main, configuration, OnReplay, audioSource, true);
            }
            // Start recording without microphone audio
            else Replay.StartRecording(Camera.main, configuration, OnReplay);
        }

        private void StartMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // If the clip has not been set, set it now
            if (audioSource.clip == null) {
                audioSource.clip = Microphone.Start(null, true, 60, 48000);
                while (Microphone.GetPosition(null) <= 0) ;
            }            
            // Play through audio source
            audioSource.timeSamples = Microphone.GetPosition(null);
            audioSource.loop = true;
            audioSource.Play();
            #endif
        }

        public void StopRecording () {
            if (recordMicrophoneAudio) audioSource.Stop();
            Replay.StopRecording();
        }

        void OnReplay (string path) {
            Debug.Log("Saved recording to: "+path);
            #if UNITY_IOS || UNITY_ANDROID
            // Playback the video
            Handheld.PlayFullScreenMovie(path);
            #endif
        }
    }
}