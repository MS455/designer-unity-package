using DapperDino.DapperTools.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityAtoms.BaseAtoms;

public class CreateLayoutView : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI Text References")]
    [SerializeField]
    TMP_InputField LayoutName;

    [SerializeField]
    TMP_InputField layoutWidth;

    [SerializeField]
    TMP_InputField layoutLength;

    [SerializeField]
    TMP_Text SpaceName;

    [Space]
    [Header("Atom Variable References")]
    [SerializeField]
    StringVariable v_layoutData;

    [SerializeField]
    StringVariable v_SelectedLayout;
    [SerializeField]
    IntVariable VI_CurrentLayout;
    [SerializeField]
    IntVariable vi_CurrentSpaces;

    [SerializeField]
    LayoutData layoutData;

    [SerializeField]
    StringVariable V_PreviousState;


    [SerializeField]
    StringVariable v_AreaValue;

    [SerializeField]
    VolumeData volumeData;

    [SerializeField]
    StringEvent E_ToastMessage;

    SpacesData hniSpacesData;

    [SerializeField]
    AllSpacesData allSpacesData;

    [SerializeField]
    VoidEvent NextState;

    float len;
    float wid;
    bool saveData = false;
   

    private void OnEnable()
    {
        VI_CurrentLayout.Value = VI_CurrentLayout.InitialValue;
        gameObject.SetActive(true);

        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));

        SpaceName.text = allSpacesData.GetSpacesData(vi_CurrentSpaces.Value).spaceName;

        if (V_PreviousState.Value == "AR_Measure")
        {
            volumeData = new VolumeData();
            volumeData = VolumeData.CreateFromJSON(v_AreaValue.Value);
            if (volumeData.volumeDataList.Count >= 0)
            {
                len = volumeData.volumeDataList[0].length;
                wid = volumeData.volumeDataList[0].width;
                //len = wid = Mathf.Round(Mathf.Sqrt(v_AreaValue.Value));
                layoutLength.text = len.ToString();
                layoutWidth.text = wid.ToString();
                
            }
            else
            {
                E_ToastMessage.Raise("AR Measure Data Not Confirmed");
            }
        }
        else
        {
            LayoutName.text = string.Empty;
            layoutLength.text = string.Empty;
            layoutWidth.text = string.Empty;
            
        }
        
    }
    public void OnDisable()
    {
        gameObject.SetActive(false);
        V_PreviousState.Value = "Create_Layout";


    }

    public void saveLayoutData()
    {
        v_SelectedLayout.Value = LayoutName.text;
        //SetLayoutData();
        V_PreviousState.Value = "SpaceManager";
        if (ValidateData())
        {
            CreateLayout();
            NextState.Raise();
        }
    }

    public void CreateLayout()
    {

        if (vi_CurrentSpaces.Value > allSpacesData.spacesDatas.Count)
        {
            allSpacesData.spacesDatas.Add(hniSpacesData);
            Debug.Log("Layout Saved");
        }
        else if (VI_CurrentLayout.Value > allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList.Count)
        {
            allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList.Add(layoutData);
            Debug.Log("Layout Saved");
        }
        else
        {
            allSpacesData.UpdateLayout(vi_CurrentSpaces.Value, VI_CurrentLayout.Value, layoutData);
            Debug.Log("Layout Updated");
            //allSpacesData.CheckandSave();
        }
        allSpacesData.SaveToString();
        saveData = false;
    }

    public bool ValidateData()
    {
        if (string.IsNullOrEmpty(LayoutName.text))
        {
            E_ToastMessage.Raise("Please Enter the Layout Name Correctly");
            return false;
        }
        else if(string.IsNullOrEmpty(layoutLength.text))
        {
            E_ToastMessage.Raise("Please Enter the Layout Length Correctly");
            return false;
        }
        else if(int.Parse(layoutLength.text)<=0)
        {
            E_ToastMessage.Raise("Length Should be Greater than 0");
            return false;
        }

        else if (string.IsNullOrEmpty(layoutWidth.text))
        {
            E_ToastMessage.Raise("Please Enter the Layout Width Correctly");
            return false;
        }
        else if (int.Parse(layoutWidth.text) <= 0)
        {
            E_ToastMessage.Raise("Length Should be Greater than 0");
            return false;
        }
        else
        {
            saveData = true;
            layoutData = new LayoutData();
            layoutData.layoutName = LayoutName.text;

            layoutData.layoutLength = string.IsNullOrEmpty(layoutLength.text) ? 0 : int.Parse(layoutLength.text);
            layoutData.layoutWidth = string.IsNullOrEmpty(layoutWidth.text) ? 0 : int.Parse(layoutWidth.text);

            v_layoutData.Value = layoutData.SaveToString();

            layoutData.SaveToString();
            hniSpacesData = allSpacesData.GetSpacesData(vi_CurrentSpaces.Value);

            hniSpacesData.LayoutDataList.Add(layoutData);

            VI_CurrentLayout.Value = hniSpacesData.LayoutDataList.Count - 1;
            return true;
        }
    }


}
