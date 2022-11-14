using System.Collections;
using System.Collections.Generic;
using DapperDino.DapperTools.StateMachines;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ARStateVIew : BaseView
{
   
    [SerializeField]
    BoolEvent E_Activate_ARCamera;
    [SerializeField]
    BoolEvent E_PlaneTracking;
   
    [SerializeField]
    StringVariable V_SelectedARItem;

    [SerializeField]
    GameObject ARItemsContainer;
    [SerializeField]
    GameObject ARItemAdapter;
    [SerializeField]
    GameObject Restorebtn;
    [SerializeField]
    GameObject SaveBtn;

    [SerializeField]
    List<GameObject> ARItemList;

    [SerializeField]
    SpacesData spacesData;

    AllSpacesData allSpacesData;

    [SerializeField]
    LayoutData layoutData;

    [SerializeField]
    IntVariable vi_CurrentLayout;
    [SerializeField]
    IntVariable vi_currentSpace;

    [SerializeField]
    MRSaveView mRSaveView;

    [SerializeField]
    GameObject SaveLoad;

    [SerializeField]
    VoidEvent E_ResetARSession;
    [SerializeField]
    StringEvent E_ToastMessage;

    [SerializeField]
    bool layoutSaved;


    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        SaveLoad.SetActive(true);
        layoutSaved = false;

        E_Activate_ARCamera.Raise(true);
        E_PlaneTracking.Raise(true);
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        spacesData = allSpacesData.spacesDatas[vi_currentSpace.Value];
        layoutData = spacesData.LayoutDataList[vi_CurrentLayout.Value];
        Debug.Log("Assets Loaded" + spacesData.LayoutDataList.Count);

        

        PopulateList();

    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
        SaveLoad.SetActive(false);
        E_Activate_ARCamera.Raise(false);
        E_PlaneTracking.Raise(false);

        if (layoutSaved)
            E_ResetARSession.Raise();

        layoutSaved = false;
        
    }
    private void PopulateList()
    {
        layoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));

        foreach (Transform obj in ARItemsContainer.transform)
            Destroy(obj.gameObject);

        ARItemList.Clear();

        for(int i =0;i<layoutData.productList.Count;i++)
        {
            GameObject item = Instantiate(ARItemAdapter, ARItemsContainer.transform);
            item.name = layoutData.productList[i].productName;
            item.GetComponent<ARAssetAdapterView>().Setdata(layoutData.productList[i].Thumbnail,layoutData.productList[i].ProductID);

            ARItemList.Add(item);
        }
    }

    public void SaveCurrentLayout()
    {
        mRSaveView.SaveMRObjects(vi_CurrentLayout.Value);
        layoutSaved = true;
    }

    public void RestoreItem()
    {
        if (layoutData.SavedDesign.data.Count <= 0)
        {
            Debug.Log("Nothing to Restore");
            return;
        }

        Debug.Log("Restoring Layout: " + vi_CurrentLayout.Value);
        mRSaveView.RestoreObjects(vi_CurrentLayout.Value);
        E_ToastMessage.Raise("Layout Restored, Tap on tracked Area to Load");
    }
    
}
