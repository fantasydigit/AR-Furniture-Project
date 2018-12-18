/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core {

    using UnityEngine;
    using Docs;

    [Doc(@"Replay")]
    public static partial class Replay {

        #region --Properties--
        /// <summary>
        /// Is a replay being recorded?
        /// </summary>
        [Doc(@"IsRecording")]
        public static bool IsRecording { get { return NatCorder.IsRecording; }}
        /// <summary>
        /// Is recording paused?
        /// </summary>
        [Doc(@"IsPaused")]
        public static bool IsPaused { get; private set; }
        #endregion


        #region --Operations--

        /// <summary>
        /// Start recording a replay with optional audio
        /// </summary>
        /// <param name="recordingCamera">Source camera for recording replay</param>
        /// <param name="configuration">Configuration for recording</param>
        /// <param name="replayCallback">Callback to be invoked with the path to the recorded replay</param>
        /// <param name="audioSource">Optional. Audio source for recording audio</param>
        [Doc(@"StartRecordingCamera"), Ref(@"Configuration", @"VideoCallback"), Code(@"RecordReplay")]
        public static void StartRecording (Camera recordingCamera, Configuration configuration, VideoCallback replayCallback, IAudioSource audioSource = null) {
            if (!recordingCamera) {
                Debug.LogError("NatCorder Error: Cannot record replay without source camera");
                return;
            } if (replayCallback == null) {
                Debug.LogError("NatCorder Error: Cannot record replay without callback");
                return;
            }
            NatCorder.StartRecording(configuration, replayCallback, audioSource);
            videoRecorder = recordingCamera.gameObject.AddComponent<VideoRecorder>();
            videoRecorder.Prepare(configuration);
        }

        /// <summary>
        /// Start recording a replay with audio from an AudioListener
        /// </summary>
        /// <param name="recordingCamera">Source camera for recording replay</param>
        /// <param name="configuration">Configuration for recording</param>
        /// <param name="replayCallback">Callback to be invoked with the path to the recorded replay</param>
        /// <param name="audioListener">Audio listener for recording audio</param>
        [Doc(@"StartRecordingAudioListener"), Ref(@"Configuration", @"VideoCallback")]
        public static void StartRecording (Camera recordingCamera, Configuration configuration, VideoCallback replayCallback, AudioListener audioListener) {
            if (audioListener == null) {
                Debug.LogError("NatCorder Error: Cannot record replay with null audio source");
                return;
            }
            audioRecorder = audioListener.gameObject.AddComponent<AudioRecorder>();
            StartRecording(recordingCamera, configuration, replayCallback, audioRecorder);
        }

        /// <summary>
        /// Start recording a replay with audio from an AudioSource
        /// </summary>
        /// <param name="recordingCamera">Source camera for recording replay</param>
        /// <param name="configuration">Configuration for recording</param>
        /// <param name="replayCallback">Callback to be invoked with the path to the recorded replay</param>
        /// <param name="audioSource">Audio source for recording audio</param>
        /// <param name="audioIsRecordOnly">When true, the audio source will not generate sound in Unity. Useful for recording microphone audio</param>
        [Doc(@"StartRecordingAudioSource", @"StartRecordingAudioSourceDiscussion"), Ref(@"Configuration", @"VideoCallback")]
        public static void StartRecording (Camera recordingCamera, Configuration configuration, VideoCallback replayCallback, AudioSource audioSource, bool audioIsRecordOnly = false) {
            if (audioSource == null) {
                Debug.LogError("NatCorder Error: Cannot record replay with null audio source");
                return;
            }
            audioRecorder = audioSource.gameObject.AddComponent<AudioRecorder>();
            StartRecording(recordingCamera, configuration, replayCallback, audioRecorder);
            audioRecorder.mute = audioIsRecordOnly;
        }

        /// <summary>
        /// Stop recording a replay
        /// </summary>
        [Doc(@"StopRecording")]
        public static void StopRecording () {
            VideoRecorder.Destroy(videoRecorder);
            NatCorder.StopRecording();
        }

        /// <summary>
        /// Pause recording
        /// </summary>
        [Doc(@"PauseRecording")]
        public static void PauseRecording () {
            IsPaused = true; // Easy peasy B)
        }

        /// <summary>
        /// Resume recording
        /// </summary>
        [Doc(@"ResumeRecording")]
        public static void ResumeRecording () {
            IsPaused = false;
        }
        #endregion
    }
}