/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core {

    using System;

    /// <summary>
    /// An audio source.
    /// Implementations of this interface are expected to commit audio sample buffers to NatCorder for encoding.
    /// </summary>
    public interface IAudioSource : IDisposable {
        /// <summary>
        /// Sample rate of audio source
        /// </summary>
        int sampleRate { get; }
        /// <summary>
        /// Number of audio samples per channel in one audio frame
        /// </summary>
        int sampleCount { get; }
        /// <summary>
        /// Number of audio channels
        /// </summary>
        int channelCount { get; }
    }
}