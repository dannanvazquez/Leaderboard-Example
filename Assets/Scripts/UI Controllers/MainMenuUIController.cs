using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class MainMenuUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text nameText;

    public void EnableMainMenu() {
        nameText.text = $"Hello, {AuthenticationService.Instance.PlayerName}!";

        GetComponent<Canvas>().enabled = true;
    }
}
