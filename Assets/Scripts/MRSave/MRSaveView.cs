using com.hexaware.datastructure;
using com.hexaware.spawner;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MRSaveView : MonoBehaviour
{

    [SerializeField]
    Transform _t;

    [SerializeField]
    ListStoredPlacementInfo allPlacements = new ListStoredPlacementInfo();

    [SerializeField]
    GameObjectValueList existingARModelValueList;

    [SerializeField]
    QueuedSpawner spawner;

    [SerializeField]
    GameObject prefabSaveRoot;

    [SerializeField]
    BoolEvent m_ActivatePlaneTracking;

    [SerializeField]
    IntVariable vi_currentSpace;

    [SerializeField]
    AllSpacesData allSpacesData;

    //[SerializeField]
    //StringEvent E_ShowToastMessage;
    public StoredPlacementInfo currentRestore = null;
    bool bRestoreEnabled = false;

    [SerializeField]
    GameObjectEvent E_AR_ModelObjectPlaced_In_AR;

    void OnEnable()
    {
        if (spawner == null)
            spawner = gameObject.AddComponent<QueuedSpawner>();

        bRestoreEnabled = false;
        Restore();
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
    }

    void Start()
    {
        SpawnCenteralObject();
    }   

    private void SpawnCenteralObject()
    {
        if (_t == null)
        {
            //_t = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            _t = new GameObject("MR Center Object").transform;
            _t.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            SphereCollider sc = _t?.GetComponent<SphereCollider>();
            if( sc != null )
                sc.isTrigger = true;
        }
    }

    public void Restore()
    {
        allPlacements.Restore();
    }

    public int GetPlacementsCount()
    {
        return allPlacements.placements.Count;
    }

    internal StoredPlacementInfo GetPlacement(int id)
    {
        return allPlacements.placements[id];
    }

    public void RemovePlacement(int id )
    {
        allPlacements.Remove(id);
    }

    public void ResetCurrentStore()
    {
        currentRestore = null;
    }

    public void RestoreItem(int a_placement_id)
    {
        //This Methods is not in use;

        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));

        LayoutData layoutData = allSpacesData.GetSpacesData(vi_currentSpace.Value).GetLayoutData(a_placement_id);

        Debug.Log("RestoreObjects Info : a_placement_id:"+ a_placement_id);
        for (int i = 0; i < allPlacements.placements.Count; i++)
        {
            currentRestore = allPlacements.placements[i];
            if (currentRestore.id == a_placement_id)
            {
                Debug.Log("RestoreObjects Info : restore enabled for :" + a_placement_id);
                bRestoreEnabled = true;
                break;
            }
            else
                currentRestore = null;
        }

        // 3nabl3 mr scan and markr  
        if( bRestoreEnabled)
            m_ActivatePlaneTracking.Raise(true);
    }

    public void RestoreObjects(int a_placement_id)
    {
        ///Custom Restore Saved Design uinsg Layout id,  it will accect layout id;
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        LayoutData layoutData = allSpacesData.GetSpacesData(vi_currentSpace.Value).GetLayoutData(a_placement_id);

        StoredPlacementInfo stored = layoutData?.SavedDesign;
        bRestoreEnabled = true;
        currentRestore = stored;

    }

    private void RestoreObjects(StoredPlacementInfo storedinfo)
    {
        // restore session

        Debug.Log("RestoreObjects Info :" + storedinfo);

        List<QueuedSpawner.Request> requests = new List<QueuedSpawner.Request>();

        for (int i = 0; i < storedinfo.data.Count; i++)
        {
            StoredPlacementInfo.StorageInfo objInfo = storedinfo.data[i];
            Vector3 pos = _t.transform.TransformPoint(objInfo.pose.position);

            Quaternion rotation = Quaternion.identity;

            rotation = _t.transform.rotation * objInfo.pose.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);

            // put things into request. 
            QueuedSpawner.Request request = new QueuedSpawner.Request(pos, rotation, objInfo.id);
            requests.Add(request);
        }

        spawner.AddRequests(requests, existingARModelValueList);
        bRestoreEnabled = false;
        //m_ActivatePlaneTracking.Raise(false);
    }


    public void SaveMRObjects(int placementid)
    {
        if (existingARModelValueList.Count <= 0)
        {
            //E_ShowToastMessage.Raise("Add atleast one model to save.");
            Debug.Log("Add atleast one model to save");
            return;
        }

        SpawnCenteralObject();

        StoredPlacementInfo data = new StoredPlacementInfo();

        //if (allPlacements.placements.Count > 0)
        //    data.id = allPlacements.placements.Last().id + 1;
        //else
        //    data.id = 1;
        
        // Setting layout id as the data id for save and Load....
        data.id = placementid;

        Vector3 centr = CalculateCenter();
        _t.position = centr;

        for (int i = 0; i < existingARModelValueList.Count; i++)
        {
            GameObject ho = existingARModelValueList[i];

            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            if (ho != null)
            {
                pos = _t.transform.InverseTransformPoint(ho.transform.position);
                rot = Quaternion.Inverse(_t.transform.rotation) * ho.transform.rotation;
                StoredPlacementInfo.StorageInfo si = new StoredPlacementInfo.StorageInfo(ho.name, pos, rot);
                data.Add(si);
            }
        }

        allPlacements.Add(data);
        allPlacements.Store();

        // Saving Data in Layout Data Class;

        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));

        LayoutData layoutData = allSpacesData.GetSpacesData(vi_currentSpace.Value).GetLayoutData(placementid);
        layoutData.SavedDesign = data;

        allSpacesData.UpdateLayout(vi_currentSpace.Value, placementid,layoutData);

        //save Method Called
        allSpacesData.SaveToString();


        //E_ShowToastMessage.Raise("Saved Successful.");
        Debug.Log("Saved Successful");
    }

    public void SetTapInMR(Vector3 pos)
    {
        if (!bRestoreEnabled)
        {
            Debug.Log("MRSaveView:: R2stor3 not 3nabl3d");
            return;
        }

        _t.position = pos;
        if (currentRestore != null)
        {
            RestoreObjects(currentRestore);
        }
    }

    private Vector3 CalculateCenter()
    {
        Vector3 centr = Vector3.zero;
        int count = existingARModelValueList.Count;

        for (int i = 0; i < existingARModelValueList.Count; i++)
        {
            GameObject ho = existingARModelValueList[i];
            if( ho != null )
                centr += ho.transform.position;
            else
            {
                // if object is not there, then dont use that in calculation
                count--;
            }

        }
        centr /= count;
        return centr;
    }

    public void ObjectSpawned( string name )
    {
        Debug.Log("MRSaveView --> ObjectSpawned() -> " + name);
       
        //m_ActivatePlaneTracking.Raise(false);
        E_AR_ModelObjectPlaced_In_AR.Raise(null);
    }

    public bool IsDesignModified()
    {
        // same no of objects
        if (currentRestore.data.Count != existingARModelValueList.Count)
            return true;
        
        for (int i = 0; i < currentRestore.data.Count; i++)
        {
            // same objects in sequence
            if ( currentRestore.data[i].id != existingARModelValueList[i].name )
            {
                return true;
            }

            // approx same positions
            float diff = Vector3.Distance(currentRestore.data[i].pose.position, existingARModelValueList[i].transform.position);

            if (diff >= 0.1f)
            {
                return true;
            }
            diff = Quaternion.Angle(currentRestore.data[i].pose.rotation, existingARModelValueList[i].transform.rotation);

            // approx same rotations 
            if (diff >= 5)
            {
                return true;
            }

        }
     
        return false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SetTapInMR(Vector3.zero);
        }
    }
#endif

}
