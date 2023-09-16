using Mirror;
using System;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class DeleteAccountUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField, Scene] private string mainMenuScene;
    [SerializeField] private Canvas settingsCanvas;

    public async void DeleteAccount() {
        try {
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
