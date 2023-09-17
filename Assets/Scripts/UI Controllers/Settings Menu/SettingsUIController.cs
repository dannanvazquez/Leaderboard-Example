using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class SettingsUIController : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Canvas nameChangeCanvas;
    [SerializeField] private Canvas deleteAccountCanvas;

    [Header("Scene References")]
    [SerializeField, Scene] private string mainMenuSceneName;
    [SerializeField, Scene] private string settingsScene;

    public async void SwitchNameChangeCanvas() {
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        nameChangeCanvas.GetComponent<NameChangeUIControllers>().SetPlayerNameText(playerName.Split('#')[0]);

        GetComponent<Canvas>().enabled = false;
        nameChangeCanvas.enabled = true;
    }

    public void SwitchDeleteAccountCanvas() {
        GetComponent<Canvas>().enabled = false;
        deleteAccountCanvas.enabled = true;
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }

    public void ExitSettings() {
        SceneManager.UnloadSceneAsync(settingsScene);
    }
}
