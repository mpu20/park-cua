using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Slider slider;
    public CrabController crabController;
    public Image fillImage;
    public Gradient gradient;

    void Update()
    {
        if (crabController != null)
        {
            slider.value = crabController.jumpValue;
            fillImage.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}