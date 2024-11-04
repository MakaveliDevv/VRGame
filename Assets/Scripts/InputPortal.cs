using UnityEngine;
using UnityEngine.InputSystem;

public class InputPortal : MonoBehaviour
{
    [SerializeField] private InputActionProperty rightHandPrimaryButton;
    [SerializeField] private new ParticleSystem particleSystem;

    private ParticleSystem.EmissionModule emissionModule;

    private void Awake() 
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = 0f; // Set the rate of emission (adjust as needed)
        emissionModule.rateOverDistance = 400f;
        emissionModule.enabled = false; // Start with emission off
    }

    private void Update()
    {
        if (rightHandPrimaryButton.action.IsPressed())
        {
            // Enable emission and modify settings
            emissionModule.enabled = true;
            Debug.Log("Right hand primary button is pressed, emission activated!");
        }
        else
        {
            // Optionally, disable emission when button is released
            emissionModule.enabled = false;
        }
    }
}
