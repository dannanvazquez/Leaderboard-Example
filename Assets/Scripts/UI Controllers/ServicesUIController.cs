using System;
using System.Collections;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ServicesUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text serviceInfoText;
    [SerializeField] private Canvas loginCanvas;
    [SerializeField] private MainMenuUIController mainMenuUIController;

    [Header("Settings")]
    [Tooltip("The amount of seconds before attempting to connect to Unity Services again after failing.")]
    [SerializeField] private int reconnectBufferSeconds;

    private void Awake() => InitializeUnityServices();

    private async void InitializeUnityServices() {
        if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn) {
            GetComponent<Canvas>().enabled = false;
            mainMenuUIController.EnableMainMenu();
            return;
        }

        try {
            serviceInfoText.text = "Attempting to connect to services...";
            await UnityServices.InitializeAsync();

            serviceInfoText.text = "Connected to services!";
            GetComponent<Canvas>().enabled = false;
            loginCanvas.enabled = true;

            Debug.Log("Connected to Unity Services.");
        } catch (Exception e) {
            StartCoroutine(BufferConnectReattempt(reconnectBufferSeconds));

            Debug.LogException(e);
        }
    }

    private IEnumerator BufferConnectReattempt(int seconds) {
        while (seconds > 0) {
            serviceInfoText.text = $"Failed to connect to services. Trying again in {seconds}...";

            yield return new WaitForSeconds(1f);
            seconds--;
        }

        InitializeUnityServices();
    }
}
