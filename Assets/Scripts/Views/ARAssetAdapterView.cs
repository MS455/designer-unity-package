using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class ARAssetAdapterView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image thumbImage;
    [SerializeField]
    Image DownloadImage;
    [SerializeField]
    GameObject SelectedAR;
    [SerializeField]
    StringVariable V_SelectedARItem;

    [SerializeField]
    StringEvent E_AddressableLoadAssetStart;

    [SerializeField]
    GameObjectEvent E_SetPlacementInteractableInAr;



    private string ProductID;

    bool canSelect = false;

    private GameObject ARModel;


    public void Setdata(string thumburl, string id)
    {
        LoadThumb(thumburl);
        ProductID = id;
    }

    void LoadThumb(string thumbnailImageURL)
    {
        Davinci.get()
            .load(thumbnailImageURL)
            .withStartAction(() =>
            {
                Debug.Log("Loading");
            })
            .withDownloadedAction(() =>
            {
                Debug.Log("Downloaded ");
            })
            .withErrorAction((Error) =>
            {
                Debug.Log(Error);
            })
            .into(thumbImage)
            .start();
    }

    public void SelectForAR()
    {
        V_SelectedARItem.Value = gameObject.name;

        if (!canSelect)
        {
            E_AddressableLoadAssetStart.Raise(ProductID);
        }
        else
        {
            E_SetPlacementInteractableInAr.Raise(ARModel);
            SelectedAR.SetActive(true);
        }

    }

    public void Downloadstatus(StringPair downloadData)
    {
        if (downloadData.Item2.Equals(ProductID))
        {
            DownloadImage.fillAmount = float.Parse(downloadData.Item1) / 100;

            if (downloadData.Item1.Equals("100"))
            {
                var Tcolor = DownloadImage.color;
                Tcolor.a = 255f;
                DownloadImage.color = Tcolor;
            }
        }
    }

    public void DownloadCompleted(GameObject obj1)
    {
        
        if (obj1.name.Equals(ProductID))
        {
            ARModel = obj1;
            canSelect = true;
            DownloadImage.fillAmount = 1;
            var Tcolor = DownloadImage.color;
            Tcolor.a = 255f;
            DownloadImage.color = Tcolor;
        }

        
    }

    public void ResetSelection()
    {
        SelectedAR.SetActive(false);
    }


}
