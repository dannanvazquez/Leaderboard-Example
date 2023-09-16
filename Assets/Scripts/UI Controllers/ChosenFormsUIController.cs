using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class ChosenFormsUIController : MonoBehaviour {
    [Header("Form Sprite References")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite rockSprite;
    [SerializeField] private Sprite paperSprite;
    [SerializeField] private Sprite scissorsSprite;

    [Header("State Image References")]
    [SerializeField] private Image playerImage;
    [SerializeField] private Image computerImage;

    [Header("Game State Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color lostColor;
    [SerializeField] private Color drawColor;
    [SerializeField] private Color wonColor;

    public void SetFormDisplay(bool isPlayer, FormType form) {
        Image targetedImage = isPlayer ? playerImage : computerImage;

        switch (form) {
            case FormType.NONE:
                targetedImage.sprite = defaultSprite;
                break;
            case FormType.ROCK:
                targetedImage.sprite = rockSprite;
                break;
            case FormType.PAPER:
                targetedImage.sprite = paperSprite;
                break;
            case FormType.SCISSORS:
                targetedImage.sprite = scissorsSprite;
                break;
            default:
                Debug.LogError("Passed an unprepared form into SetFormDisplay.", transform);
                return;
        }
    }

    public void SetFormStateColor(bool isPlayer, RoundResult result) {
        Image targetedImage = isPlayer ? playerImage : computerImage;

        switch (result) {
            case RoundResult.NONE:
                targetedImage.color = defaultColor;
                break;
            case RoundResult.LOST:
                targetedImage.color = lostColor;
                break;
            case RoundResult.DRAW:
                targetedImage.color = drawColor;
                break;
            case RoundResult.WON:
                targetedImage.color = wonColor;
                break;
            default:
                Debug.LogError("Passed an unprepared result into SetFormStateColor.", transform);
                return;
        }
    }
}
