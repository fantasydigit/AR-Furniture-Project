/*==============================================================================
Copyright (c) 2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GroundPlaneUI : MonoBehaviour
{
    #region PUBLIC_MEMBERS
    [Header("UI Elements")]
    public Text m_Title;
    public Text m_TrackerStatus;
    public Text m_Instructions;
    public CanvasGroup m_ScreenReticle;

    [Header("UI Buttons")]
    public Button m_ResetButton;
    
    #endregion // PUBLIC_MEMBERS


    #region PRIVATE_MEMBERS
    const string TITLE_PLACEMENT = "Product Placement";
    const string TITLE_GROUNDPLANE = "Ground Plane";
    const string TITLE_MIDAIR = "Mid-Air";

    GraphicRaycaster m_GraphicRayCaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    Image m_TrackerStatusImage;
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
       
        m_GraphicRayCaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
       
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy() called.");

    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void Reset()
    {
        
    }

    

    public bool InitializeUI()
    {
        // Runs only once after first successful Automatic hit test
        return true;
    }

    public bool IsCanvasButtonPressed()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        m_GraphicRayCaster.Raycast(m_PointerEventData, results);

        bool resultIsButton = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<Toggle>() ||
                result.gameObject.GetComponent<Button>())
            {
                resultIsButton = true;
                break;
            }
        }
        return resultIsButton;
    }
    #endregion // PUBLIC_METHODS


   

}
