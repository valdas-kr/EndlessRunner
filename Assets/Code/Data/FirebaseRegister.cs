using System.Collections; using UnityEngine; using Firebase;
using Firebase.Auth; using System.Threading.Tasks; using TMPro;
using System.Text.RegularExpressions; using UnityEngine.UI;

public class FirebaseRegister : MonoBehaviour {
    [SerializeField] FirebaseManager firebase;
    [SerializeField] MenuController menu;

    //Registracijos ekrano laukai
    public TMP_InputField emailRegister;
    public TMP_InputField passwordRegister;
    public TMP_InputField passwordConfirmRegister;
    public TMP_InputField usernameRegister;
    public TMP_Text registerText;
    public Button registerButton;

    public IEnumerator Register() {
        //Atliekama vartotojo registracija
        //Tikrinama, ar užpildyti laukai, slaptažodžiai sutampa, slaptažodžių pakankamas ilgis, turi skaičių, didžiąją ir mažąją raides
        if (usernameRegister.text == "") {
            firebase.ui.UpdateHintText(8);
        } else if (passwordRegister.text != passwordConfirmRegister.text) {
            firebase.ui.UpdateHintText(9);
        } else if (passwordRegister.text.Length < 8 || passwordConfirmRegister.text.Length < 8) {
            firebase.ui.UpdateHintText(10);
        }
        else if (!Regex.IsMatch(passwordRegister.text, @"\d")) {
            firebase.ui.UpdateHintText(11);
        }
        else if (!Regex.IsMatch(passwordRegister.text, @"[A-Z]")) {
            firebase.ui.UpdateHintText(12);
        }
        else if (!Regex.IsMatch(passwordRegister.text, @"[a-z]")) {
            firebase.ui.UpdateHintText(13);
        }
        else {
            Task<AuthResult> RegisterTask = firebase.auth.CreateUserWithEmailAndPasswordAsync(emailRegister.text, passwordRegister.text);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            if (RegisterTask.Exception != null) {
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode) {
                    case AuthError.MissingEmail:
                        firebase.ui.UpdateHintText(14);
                        break;
                    case AuthError.MissingPassword:
                        firebase.ui.UpdateHintText(15);
                        break;
                    case AuthError.WeakPassword:
                        firebase.ui.UpdateHintText(16);
                        break;
                    case AuthError.EmailAlreadyInUse:
                        firebase.ui.UpdateHintText(17);
                        break;
                    default:
                        firebase.ui.UpdateHintText(-1);
                        break;
                }
                firebase.ui.ClearRegisterFields();
                registerButton.enabled = false;
                Invoke("EnableRegisterButton", 3f);
            } else {
                firebase.User = RegisterTask.Result.User;
                if (firebase.User != null) {
                    UserProfile profile = new() { DisplayName = usernameRegister.text };
                    Task ProfileTask = firebase.User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
                    if (ProfileTask.Exception == null) {
                        firebase.ui.UpdateHintText(18);
                        firebase.usernameField.text = firebase.User.DisplayName;
                        StartCoroutine(ShowLogin());
                    }
                }
            }
        }
    }

    public void EnableRegisterButton() {
        registerText.text = "";
        registerButton.enabled = true;
    }

    IEnumerator ShowLogin() {
        //Pereinama į prisijungimo ekraną
        yield return new WaitForSecondsRealtime(0.3f);
        firebase.ui.ClearRegisterFields();
        EnableRegisterButton();
        menu.OpenLoginMenu();
    }
}