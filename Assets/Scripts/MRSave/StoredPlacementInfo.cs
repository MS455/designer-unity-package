using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace com.hexaware.datastructure
{

    [Serializable]
    public class StoredPlacementInfo
    {
        public int id;  // unique design id
        // stores current design informations
        public List<StorageInfo> data = new List<StorageInfo>();

        [Serializable]
        public class StorageInfo
        {
            public string id;   //  objects addressable id
            public Pose pose;

            public StorageInfo(string id, Pose pose)
            {
                this.id = id;
                this.pose = pose;
            }

            public StorageInfo(string id, Vector3 pos, Quaternion rot)
            {
                this.id = id;
                this.pose = new Pose(pos, rot);
            }

            public override string ToString()
            {
                return string.Format("ID-{0} pose-{1}", id, pose);
            }
        }

        internal void Add(StorageInfo si)
        {
            data.Add(si);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.AppendLine($"item: {item}");
            }
            return sb.ToString();
        }
    }

    [Serializable]
    public class ListStoredPlacementInfo
    {
        public List<StoredPlacementInfo> placements = new List<StoredPlacementInfo>();

        public void Add(StoredPlacementInfo info )
        {
            placements.Add(info);
        }

        public void Store()
        {
            string json = JsonUtility.ToJson(this);
            Debug.Log("PLacements:saving :" + json);
            PlayerPrefs.SetString(KEY, json);

        }

        static string KEY = "allplacements";

        public void Restore()
        {
            // read save 
            string json = PlayerPrefs.GetString(KEY, string.Empty);

            if (string.IsNullOrEmpty(json))
                return;

            ListStoredPlacementInfo save = JsonUtility.FromJson<ListStoredPlacementInfo>(json);

            this.placements = save.placements;

        }

        public void Remove( int id)
        {
            StoredPlacementInfo toRemove = null;
            foreach (var item in placements)
            {
                if( item.id == id)
                {
                    toRemove = item;
                    break;
                }
            }

            if( toRemove != null)
            {
                placements.Remove(toRemove);
            }
            Store();
        }

    }



}