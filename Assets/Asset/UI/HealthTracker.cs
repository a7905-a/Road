using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    [SerializeField] Image sliderFill;
    [SerializeField] Material greenEmission;
    [SerializeField] Material yellowEmission;
    [SerializeField] Material redEmission;
    [SerializeField] Slider HealthBarSlider;

    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);
        HealthBarSlider.value = healthPercentage;

        UpdateColor(healthPercentage);
    }

    void UpdateColor(float healthPercentage)
    {
        if (healthPercentage >= 0.6f)
        {
            sliderFill.material = greenEmission;
        }
        else if (healthPercentage >= 0.3f)
        {
            sliderFill.material = yellowEmission;
        }
        else
        {
            sliderFill.material = redEmission;
        }
    }

}
