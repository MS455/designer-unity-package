using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms;
using UnityAtoms.BaseAtoms;

public class ProductAdapterView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TextMeshProUGUI ProductNameText;
    [SerializeField]
    TMP_Text AreaText;
    [SerializeField]
    Image ThumbnailImage;
    [SerializeField]
    ObjectVariable V_CurrentSelectedItem;
    [SerializeField]
    VoidEvent E_openProductPanel;
    [SerializeField]
    Button GridItemButton;


    public void UpdateProductView(ProductsProperty model)
    {
        gameObject.name = model.Product_Name;
        ProductNameText.text = model.Product_Name;
        AreaText.text = (model.Product_Area.ToString() + " sq Ft"); ;
        LoadThumbnailImage(model.Product_thumbnailUrl);
        GridItemButton.onClick.AddListener(() =>
        {
            openDescriptionPanel(model);
        });
        /*gameObject.GetComponent<Button>().onClick.AddListener(() => {
            openDescriptionPanel();
            //V_CurrentSelectedItem.Value = model;
        });*/

       
    }
    void LoadThumbnailImage(string thumbnailImageURL)
    {
        Davinci.get()
            .load(thumbnailImageURL)
            .withStartAction(() =>
            {
                //Debug.Log("Loading");
            })
            .withDownloadedAction(() =>
            {
                //Debug.Log("Downloaded ");
            })
            .withErrorAction((Error)=>
            {
                //Debug.Log(Error);
            })
            .into(ThumbnailImage)
            .start();
    }
    public void openDescriptionPanel(ProductsProperty model)
    {
        //Debug.Log(model.Product_Name+" "+model.Product_Area);
        V_CurrentSelectedItem.Value = model;
        E_openProductPanel.Raise();
    }
}
