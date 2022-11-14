using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class NewFilterView : MonoBehaviour


{

    [SerializeField]
    GameObject filtercategoryContainer;

    [SerializeField]
    GameObject FilterAdapter;
    [SerializeField]
    StringValueList AllFilterAppliedResultList;

    [SerializeField]
    List<GameObject> categoryGameobjectList;

    public void LoadFilterCategoryData()

    {
        foreach (Transform obj in filtercategoryContainer.transform)
            Destroy(obj.gameObject);


        foreach (string category in AllFilterAppliedResultList)
        {
            GameObject categoryItem = Instantiate(FilterAdapter, filtercategoryContainer.transform);
            categoryGameobjectList.Add(categoryItem);
            categoryItem.GetComponent<FilterAdapterView>().UpdateFilterCategoryView(category);
            Debug.Log("CategoryName" + category);


        }
    }

    




}