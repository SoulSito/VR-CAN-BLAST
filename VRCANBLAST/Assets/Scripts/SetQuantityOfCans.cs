using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetQuantityOfCans : MonoBehaviour
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
        switch (value)
        {
            case 1:
                valueText.text = "Baja";
                break;
            case 2:
                valueText.text = "Normal";
                break;
            case 3:
                valueText.text = "Alta";
                break;
            default:
                valueText.text = "Normal";
                break;
        }
    }
}