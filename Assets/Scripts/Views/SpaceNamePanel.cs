using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceNamePanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_InputField SpaceNameFiled;

    public void setSpaceName()
    {
        Debug.Log(SpaceNameFiled.text);
        PlayerPrefs.SetString("name",SpaceNameFiled.text);

    }

}
