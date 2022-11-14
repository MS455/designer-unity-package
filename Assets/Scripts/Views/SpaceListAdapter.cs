using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;

public class SpaceListAdapter : MonoBehaviour
{
    [SerializeField]
    TMP_Text Name;

    [SerializeField]
    StringVariable V_SelectedLayout;
    [SerializeField]
    IntVariable vi_CurrentLayout;
    [SerializeField]
    Toggle toggle;

    int index;

    public void SetData(int Index, string name,ToggleGroup tg)
    {
        gameObject.name = name;
        Name.text = name;
        this.index = Index;
        toggle.group = tg;
        
    }
    public void setCart()
    {
        Debug.Log("Ready to go to cart");
        V_SelectedLayout.Value = Name.text;
        vi_CurrentLayout.Value = this.index;
    }

    public void SetLayoutIndex()
    {
        vi_CurrentLayout.Value = index;
    }
}
