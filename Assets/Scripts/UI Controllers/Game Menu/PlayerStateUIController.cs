using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PlayerStateUIController : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;

    public void SetScoreText(int score) {
        scoreText.text = $"Rounds passed: {score}";
    }

    public void SetLivesText(int lives) {
        if (lives > 0) {
            livesText.text = $"Lives left: {lives}";
        } else {
            livesText.text = $"Dead";
        }
    }
}
