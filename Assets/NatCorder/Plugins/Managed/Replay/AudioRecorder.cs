/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core {

    using UnityEngine;
    using System;

    public static partial class Replay {

        #region --Op vars--
        private static AudioRecorder audioRecorder;
        #endregion


        #region --Audio recording--

        [AddComponentMenu(""), DisallowMultipleComponent]
        private sealed class AudioRecorder : MonoBehaviour, IAudioSource {
            
            #region --IAudioSource--
            int IAudioSource.sampleRate { get { return AudioSettings.outputSampleRate; }}
            int IAudioSource.sampleCount {
                get {
                    int sampleCount, bufferCount;
                    AudioSettings.GetDSPBufferSize(out sampleCount, out bufferCount);
                    return sampleCount;
                }
            }
            int IAudioSource.channelCount { get { return (int)AudioSettings.speakerMode; }}
            #endregion


            #region --Op vars--
            public bool mute;
            private long timestamp, lastTime = -1; // Used to support pausing and resuming
            #endregion


            #region --Operations--

            void OnAudioFilterRead (float[] data, int channels) {
                // Calculate time
                var audioTime = Platforms.Time.AudioTime;
                if (!IsPaused) timestamp += lastTime > 0 ? audioTime - lastTime : 0;
                lastTime = audioTime;
                // Send to NatCorder for encoding
                if (!IsPaused) NatCorder.CommitSamples(data, timestamp);
                if (mute) Array.Clear(data, 0, data.Length);
            }

            void IDisposable.Dispose () {
                Destroy(this);
            }
            #endregion
        }
        #endregion
    }
}