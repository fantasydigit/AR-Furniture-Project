using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Polaris.Unity.Base.Utilities.UI;
public class ModelPlacingPage : ViewableObject {
    #region PUBLIC_MEMBERS
    
    #endregion // PUBLIC_MEMBERS
    
    #region PUBLIC_METHOD
    
    public void OnBackButtonClick() {
        //PageViewer.Instance.ShowOneObject("FirstPanel");
        ObjectViewer.Instance.ShowOneObject("FirstPanel");
    }
    #endregion //PUBLIC_METHOD

    // Use this for initialization
    public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}
}
