using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ResultsUIController : MonoBehaviour {
    [Header("Results Text Reference")]
    [SerializeField] private TMP_Text resultsText;

    private Canvas canvas;

    private void Awake() => canvas = GetComponent<Canvas>();

    public void SetResultsText(string text) {
        resultsText.text = text;

        canvas.enabled = true;
    }

    public void Continue() {
        canvas.enabled = false;

        GameManager.Instance.FinishRound();
    }
}
