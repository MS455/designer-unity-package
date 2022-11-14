using TMPro;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using System.Collections;

public class CartButton : MonoBehaviour
{
    public int itemsInCart;
    private int areainCart;

    [SerializeField]
    TMP_Text CartItems;

    [SerializeField]
    LayoutData LayoutData;

    AllSpacesData allSpacesData;

    [SerializeField]
    StringVariable v_SpacesData;

    [SerializeField]
    IntVariable vi_CurrentLayout;

    [SerializeField]
    IntVariable vi_CurrentSpaces;
    [SerializeField]
    VoidEvent E_UpdateSlider;
    [SerializeField]
    IntVariable v_sliderValue;
    [SerializeField]
    IntEvent E_Value;



    
    void OnEnable ()
    {
        SetcartItem();
        allSpacesData = AllSpacesData.CreateFromJSON(PlayerPrefs.GetString("AllSpaceData"));
        LayoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));
        Debug.Log(LayoutData.layoutName);

        //StartCoroutine(DelayedCall());  
    }

    public void SetcartItem()
    {
        LayoutData = LayoutData.CreateFromJSON(PlayerPrefs.GetString("LayoutData"));
        itemsInCart = LayoutData.productList.Count;
        areainCart = 0;
        foreach (var x in LayoutData.productList)
        {
            areainCart += (x.area*x.quantity);
        }
        E_Value.Raise(areainCart);
        //E_UpdateSlider.Raise();

        //v_sliderValue.Value = areainCart;

        CartItems.text = itemsInCart.ToString();
    }
    private void LateUpdate()
    {
        SetcartItem();
    }

    /*IEnumerator DelayedCall()
    {
        SetcartItem();
        Debug.Log("Runnning Call");
        yield return new WaitForEndOfFrame();
        
    }*/
}
