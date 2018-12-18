/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    public sealed class NatCorderNull : INatCorder {

        #region --Properties--
        public bool IsRecording { get { return false; }}
        #endregion


        #region --Operations--

        public NatCorderNull () {
            UnityEngine.Debug.Log("NatCorder: NatCorder 1.2 is not supported on this platform");
        }

        public void StartRecording (Configuration configuration, VideoCallback videoCallback, IAudioSource audioSource) {
            // We don't need the audio source
            if (audioSource != null) audioSource.Dispose();
        }

        public void StopRecording () {}

        public Frame AcquireFrame () {return null;}

        public void CommitFrame (Frame frame) {}

        public void CommitSamples (float[] sampleBuffer, long timestamp) {}
        #endregion
    }
}