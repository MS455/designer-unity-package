using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class cartListAdapterView : MonoBehaviour
{
    [SerializeField]
    TMP_Text productName;
    [SerializeField]
    TMP_Text areaText;
    [SerializeField]
    TMP_Text quantityText;

    [SerializeField]
    Image productImage;

    [SerializeField]
    IntEvent E_RemoveFromCart;

    int index;

    
    public void SetCartData(string name,int area,int quantity, string immageUrl,int ind)
    {
      
        productName.text = name;
        areaText.text = area.ToString();
        quantityText.text = quantity.ToString();
        index = ind;
        LoadThumb(immageUrl);
        //quantityText.text = quantity.ToString();

      
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
            .into(productImage)
            .start();
    }

    public void RemoveItem()
    {
        E_RemoveFromCart.Raise(index);
    }
}
