using JSAM;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine.EventSystems;
using DapperDino.DapperTools.StateMachines;

public class AreaMeasurementView : BaseView
{
    [Header("VIEW VARIABLES")]
    [SerializeField]
    TextMeshProUGUI areaResultText;
    
    [SerializeField]
    GameObject overlayPanel;
    [SerializeField]
    GameObject buttonsPanel;
    [SerializeField]
    GameObject loadingPanel;
    [SerializeField]
    GameObject areaResultPanel;
    [SerializeField]
    GameObject optionsContainer;

    [SerializeField]
    BoolEvent E_Activate_AREvent;

    [SerializeField]
    VoidEvent E_Reset_Area;
    [SerializeField]
    VoidEvent E_Calculate_Area;
    
    [SerializeField]
    VoidEvent E_AR_ResetPlacementObject;
    
    [SerializeField]
    StringVariable V_AreaValue; 

    [SerializeField]
    ObjectValueList allCategoriesResult;

    [SerializeField]
    BoolEvent E_ARMeasureMode;

    [SerializeField]
    StringVariable v_PreviousState;

    [SerializeField]
    VolumeData volumeData;
    

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        //ActivateOverlayPanel();
        E_Activate_AREvent.Raise(true);
        Debug.LogWarning("AR Measurement View");
        E_ARMeasureMode.Raise(true);
    //    E_ToggleHeaderInfoButtonEvent.Raise(false);
    //    E_ActivateStarterPanel.Raise(false);
    }

    public override void OnExit()
    {
        base.OnExit();

       /* Screen.orientation = ScreenOrientation.Portrait;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;*/

        gameObject.SetActive(false);
        E_Activate_AREvent.Raise(false);
        E_ARMeasureMode.Raise(false);

        v_PreviousState.Value = "AR_Measure";
     //   E_ActivateStarterPanel.Raise(true);
    }

    /*private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    areaMeasurementManager.ClickPlacePoint();
                
            }
        }
    }*/

    public void OnClickHomeButton()
    {
        Debug.Log("ON CLICK HOME BUTTON");
      //  E_FSM_To_Explore.Raise();
        E_AR_ResetPlacementObject.Raise();
        allCategoriesResult.Clear();
    }

    public void ShowAreaResult(string areaResult)
    {
        Debug.Log("Show result " + areaResult);
        volumeData = new VolumeData();
        volumeData = VolumeData.CreateFromJSON(areaResult);
        ActivateResultPanel();
        areaResultText.text = "";
        areaResultText.text = "Area : " + volumeData.volumeDataList[0].volume + " sq ft";
        V_AreaValue.Value = areaResult;
    }

    public void ActivateOverlayPanel()
    {
        overlayPanel.SetActive(true);
        buttonsPanel.SetActive(true);
        loadingPanel.SetActive(false);
        areaResultPanel.SetActive(false);
        optionsContainer.SetActive(true);
    }

    public void ActivateResultPanel()
    {
        overlayPanel.SetActive(false);
        buttonsPanel.SetActive(false);
        loadingPanel.SetActive(false);
        areaResultPanel.SetActive(true);
        optionsContainer.SetActive(true);
    }

    public void YesButton()
    {
        PlayClickSound();
        //StartCoroutine(ActivateLoadingAnimation());
       // ((AreaMeasureState)parentState).FetchAreaResultFromServer("area", V_AreaValue.Value.ToString());
    }

    
    public void NoButton()
    {
        PlayClickSound();
        ActivateOverlayPanel();
        V_AreaValue.Value = "";
    }

    IEnumerator ActivateLoadingAnimation()
    {
        Debug.Log("here");

        loadingPanel.SetActive(true);
        optionsContainer.SetActive(false);

        yield return new WaitForSeconds(0.01f);
    }

    public void PlayClickSound()
    {
        ActivateResultPanel();
      //  AudioManager.PlaySound(Sounds.ButtonClick);
    }
}