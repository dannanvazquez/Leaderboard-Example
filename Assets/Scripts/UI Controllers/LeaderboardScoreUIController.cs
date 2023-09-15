using TMPro;
using UnityEngine;

public class LeaderboardScoreUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetupScoreDetails(string playerName, string score) {
        playerNameText.text = playerName;
        scoreText.text = score;
    }
}