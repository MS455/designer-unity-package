using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class CatalogView : MonoBehaviour
{
    [SerializeField]
    GameObject categoryAdapter;
    [SerializeField]
    GameObject categoryContainer;
    [SerializeField]
    ObjectValueList allCategoriesResultList;
    [SerializeField]
    List<GameObject> categoryGameobjectList;
    

    public void LoadCategoryData()
    {        
        foreach (Transform obj in categoryContainer.transform)
            Destroy(obj.gameObject);
        categoryGameobjectList.Clear();

       

        foreach (Furniture_CategoryProperty category in allCategoriesResultList)
        {
            GameObject categoryItem = Instantiate(categoryAdapter, categoryContainer.transform);
            categoryGameobjectList.Add(categoryItem);
            categoryItem.GetComponent<CategoryAdapterView>().UpdateCategoryView(category);
           
        }
    }
    



}
