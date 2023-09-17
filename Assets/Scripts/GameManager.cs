using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum FormType { NONE, ROCK, PAPER, SCISSORS }
public enum RoundResult { NONE, LOST, DRAW, WON }

public class GameManager : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private PlayerStateUIController playerStateUIController;
    [SerializeField] private ChosenFormsUIController chosenFormsUIController;
    [SerializeField] private FormChoicesUIController formChoicesUIController;
    [SerializeField] private ResultsUIController resultsUIController;
    [SerializeField] private GameOverUIController gameOverUIController;

    [Header("Player State Settings")]
    [SerializeField] private int score;
    [SerializeField] private int livesLeft;

    private FormType playerForm = FormType.NONE;
    private FormType computerForm = FormType.NONE;

    private RoundResult roundResult = RoundResult.NONE;

    public int GetScore() { return score; }

    public static GameManager Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        playerStateUIController.SetScoreText(score);
        playerStateUIController.SetLivesText(livesLeft);
    }

    public void StartRound(FormType form) {
        SetPlayerForm(form);
        SetComputerForm((FormType)Random.Range(1, 4));

        DetermineResults();
    }

    public void SetPlayerForm(FormType form) {
        playerForm = form;
        chosenFormsUIController.SetFormDisplay(true, form);
    }

    private void SetComputerForm(FormType form) {
        computerForm = form;
        chosenFormsUIController.SetFormDisplay(false, form);
    }

    private void DetermineResults() {
        string results = "";

        if (playerForm == computerForm) {
            roundResult = RoundResult.DRAW;

            results = $"Draw!";

            chosenFormsUIController.SetFormStateColor(true, RoundResult.DRAW);
            chosenFormsUIController.SetFormStateColor(false, RoundResult.DRAW);
        } else if ((FormType)(((int)playerForm - 1) % 3) == computerForm) {
            roundResult = RoundResult.WON;

            results = $"{playerForm} beats {computerForm}!\nYou win!";

            chosenFormsUIController.SetFormStateColor(true, RoundResult.WON);
            chosenFormsUIController.SetFormStateColor(false, RoundResult.LOST);
        } else {
            roundResult = RoundResult.LOST;

            results = $"{computerForm} beats {playerForm}!\nYou lost!";

            chosenFormsUIController.SetFormStateColor(true, RoundResult.LOST);
            chosenFormsUIController.SetFormStateColor(false, RoundResult.WON);
        }

        if (roundResult == RoundResult.LOST) {
            livesLeft--;
        }
        score++;

        playerStateUIController.SetScoreText(score);
        playerStateUIController.SetLivesText(livesLeft);
        resultsUIController.SetResultsText(results);
    }

    public void FinishRound() {
        if (livesLeft > 0) {
            ResetRound();
        } else {
            gameOverUIController.SetGameOverCanvas(score);
        }
    }

    private void ResetRound() {
        playerForm = FormType.NONE;
        computerForm = FormType.NONE;
        roundResult = RoundResult.NONE;

        SetPlayerForm(playerForm);
        SetComputerForm(computerForm);

        chosenFormsUIController.SetFormStateColor(true, RoundResult.NONE);
        chosenFormsUIController.SetFormStateColor(false, RoundResult.NONE);

        formChoicesUIController.EnableCanvas();
    }
}
