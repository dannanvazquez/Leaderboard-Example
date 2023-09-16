using Mirror;
using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
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

    public void SetNameText(string playerName) => nameText.text = $"Welcome, {playerName}!";

    public async void EnableMainMenu() {
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        SetNameText(playerName);
        versionText.text = $"v{Application.version}";

        SetupLeaderboardInfo();

        GetComponent<Canvas>().enabled = true;
    }

    public async void SetupLeaderboardInfo() {
        // Initialize personal best published
        try {
            var personalScoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
            personalBestText.text = $"Your personal best: {personalScoreResponse.Score}\nRanked #{personalScoreResponse.Rank + 1}";
        } catch (LeaderboardsException ex) {
            if (ex.Reason == LeaderboardsExceptionReason.EntryNotFound) {
                personalBestText.text = "You do not have a score published.";
            } else {
                personalBestText.text = $"Leaderboard error: {ex.Message}.";

                Debug.LogException(ex);
            }
        } catch (Exception ex) {
            personalBestText.text = $"Error: {ex.Message}.";

            Debug.LogException(ex);
        }

        // Initialize top scores published
        try {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            if (scoresResponse.Results.Count > 0) {
                requestingDataText.text = string.Empty;
                if (scoresHolderTransform.childCount > 0) {
                    foreach (Transform child in scoresHolderTransform) {
                        Destroy(child.gameObject);
                    }
                }

                for (int i = 0; i < topScoreAmount; i++) {
                    if (scoresResponse.Results.Count > i) {
                        var currentScoreResult = scoresResponse.Results[i];
                        GameObject currentScorePanel = Instantiate(scorePanelPrefab, scoresHolderTransform);
                        currentScorePanel.GetComponent<LeaderboardScoreUIController>().SetupScoreDetails(currentScoreResult.PlayerName, currentScoreResult.Score.ToString());
                    } else {
                        break;
                    }
                }
            } else {
                requestingDataText.text = "There doesn't seem to be any scores published currently. Be the first!";
                personalBestText.text = "You do not have a score published.";
            }
        } catch (LeaderboardsException ex) {
            requestingDataText.text = $"Leaderboard error: {ex.Message}.";

            Debug.LogException(ex);
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
