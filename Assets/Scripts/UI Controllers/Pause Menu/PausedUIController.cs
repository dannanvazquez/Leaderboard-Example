using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class PausedUIController : MonoBehaviour {
    [Header("Scene References")]
    [SerializeField, Scene] private string mainMenuSceneName;
    [SerializeField, Scene] private string pausedSceneName;
    [SerializeField, Scene] private string settingsSceneName;

    private void Update() {
        if (Input.GetButtonDown("Cancel") && !IsSceneLoaded(settingsSceneName)) {
            Resume();
        }
    }

    private bool IsSceneLoaded(string sceneName) {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            if (SceneManager.GetSceneAt(i) == SceneManager.GetSceneByPath(sceneName)) {
                return true;
            }
        }

        return false;
    }

    public void Resume() {
        if (GameManager.Instance && GameManager.Instance.isPaused) {
            GameManager.Instance.isPaused = false;
        }

        SceneManager.UnloadSceneAsync(pausedSceneName);
    }

    public void SettingsMenu() {
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
    }

    public void MainMenu() {
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }
}
