using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerAllocationPanel : MonoBehaviour
{
    // Text elements to display allocated power percentages
    public TextMeshProUGUI lifeSupportText;
    public TextMeshProUGUI navigationText;
    public TextMeshProUGUI communicationText;
    public TextMeshProUGUI lightingText;
    
    // Error message text
    public TextMeshProUGUI errorMessageText;

    // Initial power allocation percentages
    private int lifeSupportPower = 50;
    private int navigationPower = 30;
    private int communicationPower = 10;
    private int lightingPower = 10;

    // Power allocation limits
    private const int maxLifeSupport = 75;
    private const int minLifeSupport = 25;
    private const int maxNavigation = 50;
    private const int minNavigation = 10;
    private const int maxCommunication = 20;
    private const int minCommunication = 5;
    private const int maxLighting = 15;
    private const int minLighting = 5;

    // Total power available (example value: can change based on game events)
    private const int totalPower = 100;

    // Function to update UI
    void UpdateUI()
    {
        lifeSupportText.text = lifeSupportPower + "%";
        navigationText.text = navigationPower + "%";
        communicationText.text = communicationPower + "%";
        lightingText.text = lightingPower + "%";
    }

    // Function to display error message
    void DisplayErrorMessage(string message)
    {
        errorMessageText.text = message;
    }

    // Function to clear error message
    void ClearErrorMessage()
    {
        errorMessageText.text = "";
    }

    // Button handler for increasing power allocation
    public void IncreasePower(string system)
    {
        int totalAllocatedPower = lifeSupportPower + navigationPower + communicationPower + lightingPower;

        switch (system)
        {
            case "LifeSupport":
                if (lifeSupportPower < maxLifeSupport && totalAllocatedPower + 5 <= totalPower)
                {
                    lifeSupportPower += 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate more power to Life Support.");
                }
                break;

            case "Navigation":
                if (navigationPower < maxNavigation && totalAllocatedPower + 5 <= totalPower)
                {
                    navigationPower += 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate more power to Navigation.");
                }
                break;

            case "Communication":
                if (communicationPower < maxCommunication && totalAllocatedPower + 5 <= totalPower)
                {
                    communicationPower += 5;
                    ClearErrorMessage();
                }
                break;

            case "Lighting":
                if (lightingPower < maxLighting && totalAllocatedPower + 5 <= totalPower)
                {
                    lightingPower += 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate more power to Lighting.");
                }
                break;
        }

        UpdateUI();
    }

    // Button handler for decreasing power allocation
    public void DecreasePower(string system)
    {
        switch (system)
        {
            case "LifeSupport":
                if (lifeSupportPower > minLifeSupport)
                {
                    lifeSupportPower -= 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate less power to Life Support.");
                }
                break;

            case "Navigation":
                if (navigationPower > minNavigation)
                {
                    navigationPower -= 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate less power to Navigation.");
                }
                break;

            case "Communication":
                if (communicationPower > minCommunication)
                {
                    communicationPower -= 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate less power to Communication.");
                }
                break;

            case "Lighting":
                if (lightingPower > minLighting)
                {
                    lightingPower -= 5;
                    ClearErrorMessage();
                }
                else
                {
                    DisplayErrorMessage("Cannot allocate less power to Lighting.");
                }
                break;
        }

        UpdateUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        ClearErrorMessage();
    }
}
