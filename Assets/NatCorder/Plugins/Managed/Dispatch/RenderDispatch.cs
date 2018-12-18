/* 
*   NatCam
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCamU.Dispatch {

	using UnityEngine;
	using System;
	using System.Runtime.InteropServices;

	public static class RenderDispatch {

		private static MainDispatch dispatch;

		public static void Initialize () {
			if (dispatch != null) return;
			dispatch = new MainDispatch();
			Action invocation = null;
			invocation = () => {
				GL.IssuePluginEvent(NatCamRenderDelegate(), 0x6723872);
				dispatch.Dispatch(invocation);
			};
			dispatch.Dispatch(invocation);
		}

		public static void Release () {
			if (dispatch != null) dispatch.Dispose();
			dispatch = null;
		}

		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("NatCamRenderDispatch")]
		#endif
		private static extern IntPtr NatCamRenderDelegate ();
	}
}