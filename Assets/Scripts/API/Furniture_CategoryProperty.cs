using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureCategoryModel", menuName = "Furniture/CreateCategoryModel", order = 0)]
public class Furniture_CategoryProperty : ScriptableObject
{
    [SerializeField]
    string category_type;

    [SerializeField]
    string category_thumbnailUrl;

    [SerializeField]
    string category_thumbnailHoverUrl;

    public string Category_type { get => category_type; set => category_type = value; }
    public string Category_thumbnailUrl { get => category_thumbnailUrl; set => category_thumbnailUrl = value; }
    public string Category_thumbnailHoverUrl { get => category_thumbnailHoverUrl; set => category_thumbnailHoverUrl = value; }
}
