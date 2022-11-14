using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;

public class SpaceAdapterView : MonoBehaviour
{
    [SerializeField]
    TMP_Text SpaceName;

    [SerializeField]
    TMP_Text OrgName;

    [SerializeField]
    TMP_Text OrgAddress;

    [SerializeField]
    TMP_Text Status;

    [SerializeField]
    StringVariable V_CurrentSelectedSpace;

    [SerializeField]
    IntVariable vi_CurrentSpaces;

    [SerializeField]
    TMP_Text N_Layout;

    int index; 


    public void SetData(int index , string spaceName,string orgName,string address,int l_Count)
    {
        this.index = index;
        SpaceName.text = spaceName;
        OrgName.text = orgName;
        OrgAddress.text = address;
        Status.text = "Submitted";
        N_Layout.text = l_Count.ToString() + "\n"+"Layout";
    }
    public void SetSelectedSpace()
    {
        V_CurrentSelectedSpace.Value = SpaceName.text;
        vi_CurrentSpaces.Value = this.index;
    }
}
