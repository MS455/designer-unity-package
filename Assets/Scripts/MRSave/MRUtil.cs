using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.hexaware.mr
{

    public static class MRUtil
    {
        static GameObject s_TrackablesObject;

        public static void AddObjectinMR(GameObject placedGO , Vector3 position, Quaternion rotation)
        {
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = position;
            anchorObject.transform.rotation = rotation;
            placedGO.transform.parent = anchorObject.transform;

            // Find trackables object in scene and use that as parent
            if (s_TrackablesObject == null)
                s_TrackablesObject = GameObject.Find("Trackables");
            if (s_TrackablesObject != null)
                anchorObject.transform.parent = s_TrackablesObject.transform;
        }

    }

}
