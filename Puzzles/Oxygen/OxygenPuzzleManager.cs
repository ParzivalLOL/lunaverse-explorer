using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OxygenPuzzleManager : MonoBehaviour
{
    // Reference to the sliders
    public Slider oxygenSlider;
    public Slider co2Slider;
    public Slider nitrogenSlider;

    // Reference to the text fields for displaying percentages
    public TextMeshProUGUI oxygenPercentageText;
    public TextMeshProUGUI co2PercentageText;
    public TextMeshProUGUI nitrogenPercentageText;

    // Realistic values for the puzzle
    private float oxygenLevel = 21f; // Initial oxygen level
    private float co2Level = 0.04f;   // Initial CO2 level
    private float nitrogenLevel = 78f; // Initial nitrogen level

    // Acceptable ranges
    private const float oxygenMin = 0f;
    private const float oxygenMax = 25f;
    private const float co2Min = 0f;
    private const float co2Max = 1f;
    private const float nitrogenMin = 70f;
    private const float nitrogenMax = 80f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize slider values and set their ranges
        oxygenSlider.value = oxygenLevel;
        co2Slider.value = co2Level;
        nitrogenSlider.value = nitrogenLevel;

        oxygenSlider.minValue = oxygenMin;
        oxygenSlider.maxValue = oxygenMax;

        co2Slider.minValue = co2Min;
        co2Slider.maxValue = co2Max;

        nitrogenSlider.minValue = nitrogenMin;
        nitrogenSlider.maxValue = nitrogenMax;

        // Update the percentage displays
        UpdatePercentages();

        // Add listeners to handle slider changes
        oxygenSlider.onValueChanged.AddListener(OnOxygenSliderChanged);
        co2Slider.onValueChanged.AddListener(OnCo2SliderChanged);
        nitrogenSlider.onValueChanged.AddListener(OnNitrogenSliderChanged);
    }

    // Slider event handlers
    public void OnOxygenSliderChanged(float value)
    {
        oxygenLevel = Mathf.Clamp(value, oxygenMin, oxygenMax); // Ensure oxygen stays within bounds
        AdjustOtherGases("oxygen");
        UpdatePercentages(); // Update the percentage text
    }

    public void OnCo2SliderChanged(float value)
    {
        co2Level = Mathf.Clamp(value, co2Min, co2Max); // Ensure CO2 stays within bounds
        AdjustOtherGases("co2");
        UpdatePercentages(); // Update the percentage text
    }

    public void OnNitrogenSliderChanged(float value)
    {
        nitrogenLevel = Mathf.Clamp(value, nitrogenMin, nitrogenMax); // Ensure nitrogen stays within bounds
        AdjustOtherGases("nitrogen");
        UpdatePercentages(); // Update the percentage text
    }

    // Adjust the other gas levels to maintain total levels within acceptable ranges
    private void AdjustOtherGases(string changedGas)
    {
        float totalGas = oxygenLevel + co2Level + nitrogenLevel;
        float otherGasTotal = 100f - totalGas;

        // Set nitrogen to stay within its range and adjust the others
        if (changedGas == "oxygen")
        {
            nitrogenLevel = Mathf.Clamp(nitrogenLevel, nitrogenMin, nitrogenMax);
            // Ensure the sum remains 100%
            co2Level = Mathf.Clamp(100f - oxygenLevel - nitrogenLevel, co2Min, co2Max);
        }
        else if (changedGas == "co2")
        {
            nitrogenLevel = Mathf.Clamp(nitrogenLevel, nitrogenMin, nitrogenMax);
            // Ensure the sum remains 100%
            oxygenLevel = Mathf.Clamp(100f - co2Level - nitrogenLevel, oxygenMin, oxygenMax);
        }
        else if (changedGas == "nitrogen")
        {
            // Ensure the sum remains 100%
            oxygenLevel = Mathf.Clamp(100f - co2Level - nitrogenLevel, oxygenMin, oxygenMax);
        }

        // Update sliders after adjustment
        oxygenSlider.value = oxygenLevel;
        co2Slider.value = co2Level;
        nitrogenSlider.value = nitrogenLevel;
    }

    // Update the percentage text next to each slider
    private void UpdatePercentages()
    {
        oxygenPercentageText.text = Mathf.Round(oxygenLevel) + "%";
        co2PercentageText.text = Mathf.Round(co2Level * 100f) / 100f + "%"; // CO2 levels are very small
        nitrogenPercentageText.text = Mathf.Round(nitrogenLevel) + "%";
    }
}
