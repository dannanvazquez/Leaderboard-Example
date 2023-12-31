using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class LoginUIController : MonoBehaviour {
    [Header("Username UI References")]
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_Text usernameErrorText;

    [Header("Password UI References")]
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_Text passwordErrorText;

    [Header("Login UI References")]
    [SerializeField] private TMP_Text loginErrorText;
    [SerializeField] private MainMenuUIController mainMenuUIController;

    private ProjectSettingsScriptableObject projectSettings;

    private string username;
    private string password;

    private void Awake() {
        usernameInputField.text = PlayerPrefs.GetString("LastLoggedUsername");
        projectSettings = Resources.Load<ProjectSettingsScriptableObject>("ProjectData");
    }

    public async void SignUp() {
        if (!IsValidLoginInfo()) return;

        await SignUpWithUsernamePasswordAsync();
    }

    public async void Login() {
        if (!IsValidLoginInfo()) return;

        await SignInWithUsernamePasswordAsync();
    }

    private bool IsValidLoginInfo() {
        bool isValidUsername = IsValidUsername();
        bool isValidPassword = IsValidPassword();

        if (isValidUsername && isValidPassword) {
            return true;
        } else {
            return false;
        }
    }

    private bool IsValidUsername() {
        username = usernameInputField.text;

        if (username.Length < 3) {
            usernameErrorText.text = "Username requires a minimum of 3 and a maximum of 20 characters";
        } else {
            usernameErrorText.text = string.Empty;
            return true;
        }

        return false;
    }

    private bool IsValidPassword() {
        password = passwordInputField.text;

        if (password.Length < 8) {
            passwordErrorText.text = "Password requires a minimum of 8 and a maximum of 30 characters.";
        } else if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit) || !StringDoesContainSymbol(password)) {
            passwordErrorText.text = "Password requires at least 1 lowercase letter, 1 uppercase letter, 1 number, and 1 symbol.";
        } else {
            passwordErrorText.text = string.Empty;
            return true;
        }

        return false;
    }

    private bool StringDoesContainSymbol(string text) {
        foreach (char c in text) {
            if (!char.IsLetter(c) && !char.IsDigit(c)) {
                return true;
            }
        }

        return false;
    }

    private async Task SignUpWithUsernamePasswordAsync() {
        loginErrorText.text = string.Empty;

        try {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            await LeaderboardsService.Instance.AddPlayerScoreAsync(projectSettings.leaderboardId, 0);
            PlayerPrefs.SetString("LastLoggedUsername", username);

            GetComponent<Canvas>().enabled = false;
            passwordInputField.text = string.Empty;
            mainMenuUIController.EnableMainMenu();

            Debug.Log("SignUp is successful.");
        } catch (AuthenticationException ex) {
            AuthenticationExceptionErrorOutput(ex);
        } catch (RequestFailedException ex) {
            RequestFailedExceptionErrorOutput(ex);
        }
    }

    private async Task SignInWithUsernamePasswordAsync() {
        loginErrorText.text = string.Empty;

        try {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            PlayerPrefs.SetString("LastLoggedUsername", username);

            GetComponent<Canvas>().enabled = false;
            passwordInputField.text = string.Empty;
            mainMenuUIController.EnableMainMenu();

            Debug.Log("SignIn is successful.");
        } catch (AuthenticationException ex) {
            AuthenticationExceptionErrorOutput(ex);
        } catch (RequestFailedException ex) {
            RequestFailedExceptionErrorOutput(ex);
        }
    }

    // Compare error code to AuthenticationErrorCodes
    // Notify the player with the proper error message
    private void AuthenticationExceptionErrorOutput(AuthenticationException ex) {
        loginErrorText.text = $"Authentication error ({ex.ErrorCode}): {ex.Message}";
        Debug.LogException(ex);
    }

    // Compare error code to CommonErrorCodes
    // Notify the player with the proper error message
    private void RequestFailedExceptionErrorOutput(RequestFailedException ex) {
        loginErrorText.text = $"Request error ({ex.ErrorCode}): {ex.Message}";
        Debug.LogException(ex);
    }
}
