using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClearPlayerprefs : MonoBehaviour
{
    // Start is called before the first frame update
    [MenuItem("Player Prefs/ClearAllSpaceData")]
    static void ClearAllSpaceData()
    {
        PlayerPrefs.DeleteKey("AllSpaceData");
    }
    [MenuItem("Player Prefs/Clear Everything")]
    static void clearAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
