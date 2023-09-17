using System;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class NameChangeUIControllers : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Canvas settingsCanvas;

    public void SetPlayerNameText(string playerName) {
        playerNameInputField.text = playerName;
    }

    public async void ChangeName() {
        errorText.text = string.Empty;
        try {
            if (playerNameInputField.text.Length >= 3) {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerNameInputField.text);

                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                errorText.text = $"Display name has been changed to {playerName}!";

                MainMenuUIController mainMenuUIController = FindObjectOfType(typeof(MainMenuUIController)) as MainMenuUIController;
                mainMenuUIController.SetNameText(playerName);
                mainMenuUIController.SetupLeaderboardInfo();
            } else {
                errorText.text = "Username requires a minimum of 3 and a maximum of 20 characters";
            }
        } catch (Exception ex) {
            errorText.text = $"Error: {ex.Message}.";

            Debug.LogException(ex);
        }
    }

    public void SwitchSettingsCanvas() {
        GetComponent<Canvas>().enabled = false;
        settingsCanvas.enabled = true;
    }
}
