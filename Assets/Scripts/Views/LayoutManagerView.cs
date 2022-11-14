using UnityEngine;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using DapperDino.DapperTools.StateMachines;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LayoutManagerView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_Text SpaceName;

    [SerializeField]
    TMP_InputField SpaceNameFiled;

    [SerializeField]
    SpacesData hniSpaceData;

    [SerializeField]
    AllSpacesData allSpacesData;

    [SerializeField]
    GameObject ExtendedFooter;

    [SerializeField]
    GameObject Footer;

    [SerializeField]
    GameObject NoDataContainer;

    [SerializeField]
    GameObject ListSpacesContainer;

    [SerializeField]
    GameObject EditButton;
    [SerializeField]
    GameObject SelectDeselectBtn;

    [SerializeField]
    bool ShowLayout = false;

    [SerializeField]
    GameObject LayoutItemAdapter;
    [SerializeField]
    GameObject LayoutItemContainer;
    
    [SerializeField]
    StringVariable V_CurrentSelectedSpace;

    [SerializeField]
    IntVariable vi_CurrentSpaces;

    [SerializeField]
    IntVariable vi_currentLayout;

    [SerializeField]
    ToggleGroup LayoutGroup;

    [SerializeField]
    List<GameObject> LayoutItemList;

    bool CansaveSpace = false;

    public void OnEnable()
    {    
        
        gameObject.SetActive(true);
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("AllSpaceData")))
        {
            allSpacesData = new AllSpacesData();
        }
        else
        {
            allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
            if (vi_CurrentSpaces.Value < allSpacesData.spacesDatas.Count)
            {

                ShowLayout = (allSpacesData.GetSpacesData(vi_CurrentSpaces.Value).LayoutDataList.Count > 0) ? true : false;
                SpaceName.text = (string.IsNullOrEmpty(V_CurrentSelectedSpace.Value)) ? "Hexaware CXC" : V_CurrentSelectedSpace.Value;
            }
            else
                ShowLayout = false;
            
        }
        //allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        
        //Debug.Log(PlayerPrefs.GetString("AllSpaceData"));

        configureUI();
    }
    
    public void OnDisable()
    {
        
        gameObject.SetActive(false);
        ShowLayout = false;
        V_CurrentSelectedSpace.Value = null;
        if (CansaveSpace)
        {
            allSpacesData.spacesDatas.Add(hniSpaceData);
            vi_CurrentSpaces.Value = allSpacesData.spacesDatas.Count-1;
            allSpacesData.SaveToString();
            CansaveSpace = false;
            vi_CurrentSpaces.Value = allSpacesData.spacesDatas.Count - 1;
        }
        

        allSpacesData.SaveToString();

    }

    public void SetSpaceData()
    {
        CansaveSpace = true;
        SpaceName.text = SpaceNameFiled.text;
        hniSpaceData = new SpacesData();
        hniSpaceData.spaceName = SpaceName.text;
        V_CurrentSelectedSpace.Value = SpaceName.text;
        hniSpaceData.SaveToString();
    }

    public void configureUI()
    {
        if(ShowLayout)
        {            
            SelectDeselectBtn.gameObject.SetActive(true);
            EditButton.gameObject.SetActive(false);
            ListSpacesContainer.gameObject.SetActive(true);
            NoDataContainer.gameObject.SetActive(false);
            ExtendedFooter.gameObject.SetActive(true);
            Footer.gameObject.SetActive(false);
            
            PolulateLayout();
        }
        else
        {
            EditButton.gameObject.SetActive(true);
            SelectDeselectBtn.gameObject.SetActive(false);
            ListSpacesContainer.gameObject.SetActive(false);
            NoDataContainer.gameObject.SetActive(true);
            Footer.gameObject.SetActive(true);
            ExtendedFooter.gameObject.SetActive(false);
            
        }
    }

    public void PolulateLayout()
    {
        int spaceid = vi_CurrentSpaces.Value;
        foreach (Transform obj in LayoutItemContainer.transform)
            Destroy(obj.gameObject);
        LayoutItemList.Clear();
        
        SpacesData sd = allSpacesData.GetSpacesData(vi_CurrentSpaces.Value);

        for(int i=0;i<sd.LayoutDataList.Count;i++)
        {
            GameObject item = Instantiate(LayoutItemAdapter, LayoutItemContainer.transform);
            item.GetComponent<SpaceListAdapter>().SetData(i,
                sd.LayoutDataList[i].layoutName,
                LayoutItemContainer.gameObject.GetComponent<ToggleGroup>());
            LayoutItemList.Add(item);
        }
    }

    public void DeleteLayout()
    {
        allSpacesData.spacesDatas[vi_CurrentSpaces.Value].LayoutDataList.RemoveAt(vi_currentLayout.Value);
        allSpacesData.SaveToString();
        OnEnable();
    }

    public void BackBtn()
    {
        CansaveSpace = false;
    }
}
