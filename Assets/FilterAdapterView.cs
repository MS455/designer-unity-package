using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class FilterAdapterView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI categoryNameText;

    [SerializeField]
    StringValueList AllFilterAppliedResultList;

    int index;

    public void UpdateFilterCategoryView(string categoryName)
    {
        gameObject.name = categoryName;
        categoryNameText.text = categoryName;
        
    }

    public void RemoveFilter()
    {
        Debug.Log("Filter Removed");
        AllFilterAppliedResultList.Remove(gameObject.name);
    }
}
