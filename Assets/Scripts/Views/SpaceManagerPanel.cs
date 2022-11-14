using UnityEngine;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using System.Collections.Generic;

public class SpaceManagerPanel : MonoBehaviour
{
    [SerializeField]
    GameObject areaMeasurementPanel;
    [SerializeField]
    BoolEvent E_Activate_ARCamera;

    [SerializeField]
    GameObject LayoutManagerPanel;
    [SerializeField]
    GameObject SpacesAdpter;
    [SerializeField]
    GameObject SpaceAdapterContainer;
    [SerializeField]
    GameObject NoSpaceMessage;

    [SerializeField]
    AllSpacesData allSpacesData;

    [SerializeField]
    List<GameObject> spaceGameobjectList;

    [SerializeField]
    IntVariable v_CurrentSpaces;
    [SerializeField]
    IntVariable vi_CurrentLayout;

    [SerializeField]
    StringVariable V_PreviousState;

    [SerializeField]
    StringVariable v_previousPanel;
    [SerializeField]
    VoidEvent E_CreateLayout;

    int SpaceCount;

    
    public void OnEnable()
    {
        
        gameObject.SetActive(true);
        LayoutManagerPanel.SetActive(false);

        v_previousPanel.Value = "SpaceManager";

        if (V_PreviousState.Value == "AR_Measure")
        {
            E_CreateLayout.Raise();
        }
        else if(V_PreviousState.Value == "Quotation")
        {
            //ToDo
        }
        
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("AllSpaceData")))
        {
            NoSpaceMessage.gameObject.SetActive(true);
        }
        else
        {
            NoSpaceMessage.gameObject.SetActive(false);
            foreach (Transform obj in SpaceAdapterContainer.transform)
                Destroy(obj.gameObject);
            spaceGameobjectList.Clear();

            allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
            SpaceCount = allSpacesData.spacesDatas.Count;
            for(var x=0;x<SpaceCount;x++)
            {
                GameObject spaceitem = Instantiate(SpacesAdpter, SpaceAdapterContainer.transform);
                spaceitem.GetComponent<SpaceAdapterView>()
                       .SetData(x,
                            allSpacesData.spacesDatas[x].spaceName,
                            allSpacesData.spacesDatas[x].customerData.OrgName,
                            allSpacesData.spacesDatas[x].customerData.Address,
                            allSpacesData.spacesDatas[x].LayoutDataList.Count);
                spaceGameobjectList.Add(spaceitem);
            }
            

        }
    }

   
    public  void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void ToggleARMeasurement(bool status)
    {
        Debug.Log("ToggleARMeasurement " + status);
        areaMeasurementPanel.SetActive(status);
        E_Activate_ARCamera.Raise(status);
        gameObject.SetActive(!status);
    }
    public void ResetCurrentSpace()
    {
        v_CurrentSpaces.Value = v_CurrentSpaces.InitialValue;
    }
    
}


