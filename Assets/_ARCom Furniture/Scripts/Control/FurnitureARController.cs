//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------


namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
    using Polaris.Unity.Base.Utilities.UI;
    using System.Collections;
    using LitJson;
    using UnityEngine.Networking;
    using TriLib;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    // using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class FurnitureARController : MonoBehaviour
    {
        #region PUBLIC_MEMBERS 
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyAndroidPrefab;
                
        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;
        public GameObject CurAugmentedPlacementObj
        {
            get { return curAugmentedPlacementObj; }
            set { curAugmentedPlacementObj = value; }
        }
        public static FurnitureARController Instance;
        #endregion // PUBLIC_MEMBERS

        #region PRIVATE_MEMBERS
        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;
        private bool isBusy = false;
        private ObjectLoadedHandle objectLoaded = null;
        private GameObject curAugmentedPlacementObj;
        #endregion // PRIVATE_MEMBERS

        

        #region MONOBEHAVIOR_METHODS
        void Awake()
        {
            Instance = this;
            ShowRecogPointCloud(false);
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        void Update()
        {
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }
            SearchingForPlaneUI.SetActive(showSearchingUI);
            
        }
        #endregion //MONOBEHAVIOR_METHODS

        #region PUBLIC_METHODS
        public void FinishPlacingModel() {
            curAugmentedPlacementObj.GetComponent<ProductPlacement>().UpdateState(ProductPlacement.PlacementStates.PLACED);
        }

        public void RemoveTyingModel() {
            GameObject.DestroyImmediate(curAugmentedPlacementObj);
            curAugmentedPlacementObj = null;
        }

        // load model with code
        public void LoadModelWithCode(Text code)
        {
            if (isBusy) return;
            isBusy = true;
            StartCoroutine(LoadModelode(code.text));
        }

        

        public void LoadModelWithBundle(GameObject bundle)
        {
            curAugmentedPlacementObj = Instantiate(AndyAndroidPrefab, new Vector3(0, 100, 0), Quaternion.identity);
            
            ///GameObject model = GameObject.Instantiate(bundle);
            GameObject model = bundle;
            curAugmentedPlacementObj.GetComponent<ProductPlacement>().ScaleIndicator(model);
            isBusy = false;
        }

        
        public GameObject poingCloud;
        public GameObject planeGenerator;
        public void ShowRecogPointCloud(bool flag) {
            poingCloud.SetActive(flag);
            planeGenerator.SetActive(flag);
        }
        #endregion // PUBLIC_METHODS

        #region PRIVATE_METHODS
        private string GetBundleNameFromURL(string url)
        {
            string[] tokens = url.Split('/');
            return tokens[tokens.Length - 1];
        }
        // <summary>
        /// Check and update the application lifecycle.
        /// </summary>        
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }


        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        IEnumerator LoadModelode(string code)
        {
            Item _item = new Item();
            string strUrl = "http://agselling.com/arcore/json.php?id=" + code;
            Debug.Log(strUrl);
            WWW www = new WWW(strUrl);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                string correct = www.text.Remove(www.text.Length - 4, 1);
                JsonData json = JsonMapper.ToObject(correct);
                _item.url = json["url"].ToString();
                Debug.Log("url:" + _item.url);
            }
            else
            {
                Debug.Log("error");
                //_ShowAndroidToastMessage("controlla la connessione di rete");
                MessageBox.DisplayMessageBox("Error", "Poor Network Connection!", false, null);
                FurnitureARController.Instance.gameObject.GetComponent<ObjectViewer>().ShowOneObject("FirstPanel");
                yield break;
            }
            if (_item.url == "error")
            {
                //_ShowAndroidToastMessage("codice non VALIDO!!!");
                MessageBox.DisplayMessageBox("Error", "Invalid Code!", true, null);
                FurnitureARController.Instance.GetComponent<ObjectViewer>().ShowOneObject("FirstPanel");
                yield break;
            }
            else
            {
                //_item.URL = "https://agselling.com/arcore/output/AssetBundles/Android/sofa-collection";        
                //_item.URL = "https://agselling.com/arcore/output/AssetBundles/Android/andy";
                //_item.URL = "https://agselling.com/arcore/output/AssetBundles/Android/cucina";
                string bundleName = GetBundleNameFromURL(_item.url);
                //objectLoaded = null;
                //objectLoaded += LoadModelWithBundle;
                //GetComponent<MyLoadAssets>().DoLoad(bundleName, bundleName);
                GetComponent<MyAssetDownloader>().StartDownload(_item.url, "fbx");
                FurnitureARController.Instance.gameObject.GetComponent<ObjectViewer>().HideAllObjects();

            }

        }
        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        #endregion // PRIVATE_METHODS
    }
}
