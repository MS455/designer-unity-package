using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color Default_color;
    public Color HighlightedColor;

    private TMP_Text SignInText;

    private void Awake()
    {
        SignInText = gameObject.GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SignInText.color = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SignInText.color = Default_color;
    }

    // Start is called before the first frame update
    
}
