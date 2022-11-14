using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CategoryAdapterView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    TextMeshProUGUI categoryNameText;

    [SerializeField]
    VoidEvent E_ApplyFilters;

    [SerializeField]
    StringValueList AllFilterAppliedResultList;

    [SerializeField]
    bool _FilterState;

    [SerializeField]
    Image TickBox;
    [SerializeField]
    Sprite CheckedSprite;
    [SerializeField]
    Sprite UncheckedSprite;
    [SerializeField]
    Sprite HoverSprite;
    public void UpdateCategoryView(Furniture_CategoryProperty model)
    {
        gameObject.name = model.Category_type;
        categoryNameText.text = model.Category_type;
    }

    public void ApplyFilters()
    {
        if (!_FilterState)
        {
            AllFilterAppliedResultList.Add(gameObject.name);
            _FilterState = true;
            TickBox.sprite = CheckedSprite;
        }
        else
        {
            RemoveFilter();
            TickBox.sprite = UncheckedSprite;
        }
        

    }

    public void RemoveFilter()
    {
        Debug.Log("Filter Removed "+_FilterState);
        _FilterState = false;
        AllFilterAppliedResultList.Remove(gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!(AllFilterAppliedResultList.Contains(gameObject.name)))  
            TickBox.sprite = HoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!AllFilterAppliedResultList.Contains(gameObject.name))
        {
            TickBox.sprite = UncheckedSprite;
        }
        
        
    }
    private void OnEnable()
    {
        if (!(AllFilterAppliedResultList.Contains(gameObject.name)))
            TickBox.sprite = UncheckedSprite;
        else
            TickBox.sprite = CheckedSprite;
    }




}
