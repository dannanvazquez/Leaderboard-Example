using TMPro;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class GameOverUIController : MonoBehaviour {
    [Header("Text References")]
    [SerializeField] private TMP_Text scoreResultText;

    [Header("Scene References")]
    [SerializeField, Scene] private string gameSceneName;
    [SerializeField, Scene] private string mainMenuSceneName;

    private ProjectSettingsScriptableObject projectSettings;

    private Canvas canvas;

    private void Awake() {
        canvas = GetComponent<Canvas>();
        projectSettings = Resources.Load<ProjectSettingsScriptableObject>("ProjectData");
    }

    public async void SetGameOverCanvas(int score) {
        try {
            var personalScoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(projectSettings.leaderboardId);
            if (score > personalScoreResponse.Score) {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(projectSettings.leaderboardId, score);
                scoreResultText.text = $"You beat your personal best!";
            } else {
                scoreResultText.text = $"You did not beat your personal best: {personalScoreResponse.Score}";
            }
        } catch (LeaderboardsException ex) {
            if (ex.Reason == LeaderboardsExceptionReason.EntryNotFound) {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(projectSettings.leaderboardId, score);
                scoreResultText.text = $"You submitted your first score!";
            } else {
                scoreResultText.text = $"Leaderboard error: {ex.Message}.";

                Debug.LogException(ex);
            }
        }

        canvas.enabled = true;
    }

    public void Restart() {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void MainMenu() {
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }
}
