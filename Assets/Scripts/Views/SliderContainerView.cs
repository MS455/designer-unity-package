using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityAtoms.BaseAtoms;

public class SliderContainerView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Slider Slider;
    [SerializeField]
    TMP_Text Max_value;
    [SerializeField]
    TMP_Text SliderValueText;

    public int max_area;
    public int ValueText;

    [SerializeField]
    IntVariable V_SliderValue;
    [SerializeField]
    IntVariable V_SliderMaxValue;

    public void UpdateValue(int value)
    {
        
        ValueText = value;
        Slider.value = value;
        SliderValueText.text = value.ToString();
        V_SliderValue.Value = value;

    }
    public void SetMaxValue(int max_value)
    {
        
        max_area = max_value;
        Max_value.text = max_area.ToString();
        Slider.maxValue = max_value;
        V_SliderMaxValue.Value = max_value;
    }

    /*public void UpdateSlider()
    {
        Debug.Log("Updating Slider");
        Debug.Log("Value : "+ SliderValueText.text+"  Variable: "+ V_SliderValue.Value);
        //Slider.value = V_SliderValue.Value;
        SliderValueText.text = V_SliderValue.Value.ToString();
        ValueText = V_SliderValue.Value;

        Slider.maxValue = V_SliderMaxValue.Value;
        Max_value.text = V_SliderMaxValue.Value.ToString();
    }*/

    /*private void LateUpdate()
    {
        Slider.maxValue = max_area;
        
        Slider.value = ValueText;
        
       
        //Value.text = ValueText.ToString();
    }*/
}
