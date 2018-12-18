/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using System;
    using System.Runtime.InteropServices;

    public static class NatCorderBridge {

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        "__Internal";
        #else
        "NatCorder";
        #endif

        public delegate void EncodeCallback (IntPtr frame);

        #if UNITY_IOS || UNITY_WEBGL || UNITY_STANDALONE || UNITY_EDITOR
        [DllImport(Assembly, EntryPoint = "NCInitialize")]
        public static extern void Initialize (
            EncodeCallback encodeCallback,
            VideoCallback videoCallback,
            #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            [MarshalAs(UnmanagedType.LPWStr)]
            #endif
            string writePath
        );
        [DllImport(Assembly, EntryPoint = "NCStartRecording")]
        public static extern void StartRecording (int width, int height, int framerate, int bitrate, int keyframes, bool audio, int sampleRate, int sampleCount, int channelCount);
        [DllImport(Assembly, EntryPoint = "NCStopRecording")]
        #if UNITY_WEBGL && !UNITY_EDITOR
        public static extern IntPtr StopRecording ();
        #else
        public static extern void StopRecording ();
        #endif
        [DllImport(Assembly, EntryPoint = "NCIsRecording")]
        public static extern bool IsRecording ();
        [DllImport(Assembly, EntryPoint = "NCEncodeFrame")]
        public static extern void EncodeFrame (IntPtr frame, long timestamp);
        [DllImport(Assembly, EntryPoint = "NCEncodeSamples")]
        public static extern void EncodeSamples (float[] sampleBuffer, long timestamp);

        #else
        public static void Initialize (EncodeCallback encodeCallback, VideoCallback videoCallback, string writePath) {}
        public static void StartRecording (int width, int height, int framerate, int bitrate, int keyframes, bool audio, int sampleRate, int sampleCount, int channelCount) {}
        #if UNITY_WEBGL
        public static IntPtr StopRecording () {}
        #else
        public static void StopRecording () {}
        #endif
        public static bool IsRecording () {return false;}
        public static void EncodeFrame (IntPtr frame, long timestamp) {}
        public static void EncodeSamples (float[] sampleBuffer, long timestamp) {}
        #endif
    }
}