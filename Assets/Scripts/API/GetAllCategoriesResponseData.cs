using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class GetAllCategoriesResponseData
{
    [Serializable]
    public class CategoryData
    {
        public string type;
        //public string url;
        //public string hoverUrl;
    }

    public List<CategoryData> result = new List<CategoryData>();

    public static GetAllCategoriesResponseData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GetAllCategoriesResponseData>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void AddCategory(string categoryType, string url = "", string hoverUrl = "")
    {
        CategoryData categoryData = new CategoryData();

        categoryData.type = categoryType;
        //categoryData.url = url;
        //categoryData.hoverUrl = hoverUrl;

        result.Add(categoryData);
    }

    public bool IsCategoryPresent(string categoryType)
    {
        foreach (var data in result)
        {
            if (data.type == categoryType)
                return true;
        }
        return false;
    }
}