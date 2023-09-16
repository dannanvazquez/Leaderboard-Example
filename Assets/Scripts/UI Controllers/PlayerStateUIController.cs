using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PlayerStateUIController : MonoBehaviour {
    [Header("Text References")]
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
