using Mirror;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class DeleteAccountUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField, Scene] private string mainMenuScene;
    [SerializeField] private Canvas settingsCanvas;

    private class TResult {}

    public async void DeleteAccount() {
        try {
            var parameters = new Dictionary<string, object>() {
                {"playerIDToRemove", AuthenticationService.Instance.PlayerId}
            };
            await CloudCodeService.Instance.CallEndpointAsync<TResult>("DeleteLeaderboardEntries", parameters);

            await AuthenticationService.Instance.DeleteAccountAsync();

            SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void SwitchSettingsCanvas() {
        GetComponent<Canvas>().enabled = false;
        settingsCanvas.enabled = true;
    }
}
