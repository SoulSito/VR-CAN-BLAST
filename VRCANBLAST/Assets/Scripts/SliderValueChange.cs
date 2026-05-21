using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueChange : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TMP_Text valueText;

    private void Start()
    {
        slider = GetComponent<Slider>();
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        valueText.text = ((int)value).ToString();
    }
}