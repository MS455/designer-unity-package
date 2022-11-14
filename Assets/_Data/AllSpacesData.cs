using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class AllSpacesData
{
    public List<SpacesData> spacesDatas = new List<SpacesData>();

    public static AllSpacesData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AllSpacesData>(jsonString);
    }

    public string SaveToString()
    {
        string layoutDataString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("AllSpaceData", layoutDataString);
        return layoutDataString;
    }

    public SpacesData GetSpacesData( int index)
    {
        if (index >= 0 && index < spacesDatas.Count)
            return spacesDatas[index];

        return null;

    }
   
    public void UpdateLayout( int spaceIndex, int layoutIndex , LayoutData ld)
    {
        var spacedata = GetSpacesData(spaceIndex);

        var layout = spacedata.GetLayoutData(layoutIndex);

        layout?.Copy(ld);

        // TODO SAVE

    }


}
