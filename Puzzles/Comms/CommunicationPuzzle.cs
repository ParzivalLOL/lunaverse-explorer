using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationPuzzle : MonoBehaviour
{
    public RectTransform targetWaveTransform;   // UI element for the target wave
    public RectTransform playerWaveTransform;   // UI element for the player's wave
    public Slider amplitudeSlider;              // Slider to control the amplitude
    public Slider wavelengthSlider;             // Slider to control the wavelength
    public TextMeshProUGUI matchPercentageText;            // UI text to show the match percentage

    private float targetAmplitude = 3f;         // Target amplitude (height of the wave)
    private float targetWavelength = 4f;        // Target wavelength (distance between peaks)
    
    private float playerAmplitude;              // Player-controlled amplitude
    private float playerWavelength;             // Player-controlled wavelength

    private int waveResolution = 100;           // Number of points to generate for the sine wave

    void Start()
    {
        // Initialize the target wave
        DrawSineWave(targetWaveTransform, targetAmplitude, targetWavelength);

        // Initialize the sliders with some starting values
        amplitudeSlider.value = 1f;             // Starting amplitude for player's wave
        wavelengthSlider.value = 1f;            // Starting wavelength for player's wave

        // Add listeners for the sliders to update the player's wave in real time
        amplitudeSlider.onValueChanged.AddListener(UpdatePlayerWave);
        wavelengthSlider.onValueChanged.AddListener(UpdatePlayerWave);

        // Draw the initial player's wave
        UpdatePlayerWave(0);
    }

    // Function to draw a sine wave based on amplitude and wavelength
    void DrawSineWave(RectTransform waveTransform, float amplitude, float wavelength)
    {
        LineRenderer lineRenderer = waveTransform.GetComponent<LineRenderer>();
        lineRenderer.positionCount = waveResolution;

        for (int i = 0; i < waveResolution; i++)
        {
            float x = i / (float)waveResolution * 2 * Mathf.PI * wavelength;
            float y = Mathf.Sin(x) * amplitude;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // Function to update the player's wave based on slider input
    public void UpdatePlayerWave(float value)
    {
        playerAmplitude = amplitudeSlider.value * targetAmplitude; // Scale the player's amplitude
        playerWavelength = wavelengthSlider.value * targetWavelength; // Scale the player's wavelength

        // Update the player's sine wave based on the new values
        DrawSineWave(playerWaveTransform, playerAmplitude, playerWavelength);

        // Check how close the player is to matching the target
        float matchPercentage = CalculateMatchPercentage();
        matchPercentageText.text = "Match: " + Mathf.RoundToInt(matchPercentage) + "%";
    }

    // Function to calculate how closely the player's wave matches the target
    float CalculateMatchPercentage()
    {
        // Calculate how close the amplitude and wavelength are to the target values
        float amplitudeDifference = Mathf.Abs(playerAmplitude - targetAmplitude);
        float wavelengthDifference = Mathf.Abs(playerWavelength - targetWavelength);

        // Normalize the differences (smaller is better)
        float amplitudeMatch = 1 - Mathf.Clamp01(amplitudeDifference / targetAmplitude);
        float wavelengthMatch = 1 - Mathf.Clamp01(wavelengthDifference / targetWavelength);

        // Average the two match scores
        return (amplitudeMatch + wavelengthMatch) / 2 * 100;
    }
}
