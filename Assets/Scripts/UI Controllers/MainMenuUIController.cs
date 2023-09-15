using Mirror;
using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class MainMenuUIController : MonoBehaviour {
    [Header("General References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private Canvas loginCanvas;
    [SerializeField, Scene] private string gameScene;
    [SerializeField, Scene] private string settingsMenuScene;

    [Header("Leaderboard References")]
    [SerializeField] private TMP_Text requestingDataText;
    [SerializeField] private TMP_Text personalBestText;
    [SerializeField] private Transform scoresHolderTransform;
    [SerializeField] private GameObject scorePanelPrefab;

    [Header("Leaderboard Settings")]
    [Tooltip("The ID of the leaderboard being used.")]
    [SerializeField] private string leaderboardId;
    [Tooltip("The amount of top scores displayed in the leaderboard.")]
    [SerializeField] private int topScoreAmount;

    public async void EnableMainMenu() {
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        nameText.text = $"Welcome, {playerName.Split('#')[0]}!";
        versionText.text = $"v{Application.version}";

        SetupLeaderboardInfo();

        GetComponent<Canvas>().enabled = true;
    }

    private async void SetupLeaderboardInfo() {
        try {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

            try {
                if (scoresResponse.Results.Count > 0) {
                    requestingDataText.text = string.Empty;

                    // Initialize personal best published
                    var personalScoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
                    if (personalScoreResponse != null) {
                        personalBestText.text = $"Your personal best: {personalScoreResponse.Score}\nRanked #{personalScoreResponse.Rank + 1}";
                    } else {
                        personalBestText.text = "You do not have a score published.";
                    }

                    // Initialize top scores published
                    for (int i = 0; i < topScoreAmount; i++) {
                        if (scoresResponse.Results.Count > i) {
                            var currentScoreResult = scoresResponse.Results[i];
                            GameObject currentScorePanel = Instantiate(scorePanelPrefab, scoresHolderTransform);
                            currentScorePanel.GetComponent<LeaderboardScoreUIController>().SetupScoreDetails(currentScoreResult.PlayerName.Split('#')[0], currentScoreResult.Score.ToString());
                        } else {
                            break;
                        }
                    }
                } else {
                    requestingDataText.text = "There doesn't seem to be any scores published currently. Be the first!";
                }
            } catch (Exception ex) {
                Debug.LogException(ex);
            }
        } catch (Exception ex) {
            requestingDataText.text = $"Error: {ex.Message}.";

            Debug.LogException(ex);
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    public void EnableSettingsMenu() {
        SceneManager.LoadScene(settingsMenuScene, LoadSceneMode.Additive);
    }

    public void SignOut() {
        AuthenticationService.Instance.SignOut();

        GetComponent<Canvas>().enabled = false;
        loginCanvas.enabled = true;

        Debug.Log("SignOut is successful.");
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
