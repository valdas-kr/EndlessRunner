using System.Collections; using UnityEngine; using Firebase;
using Firebase.Database; using System.Threading.Tasks;
using TMPro; using UnityEngine.UI; using System; using Firebase.Auth;

public class FirebaseLogin : MonoBehaviour {
    [SerializeField] FirebaseManager firebase;
    [SerializeField] GameManager gm;
    [SerializeField] MenuController mc;
    [SerializeField] SwitchPlayer player;

    //Prisijungimo meniu laukai
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    public TMP_Text loginText;
    public Button loginButton;

    public IEnumerator Login() {
        //Atliekamas vartotojo prisijungimas
        Task<AuthResult> LoginTask = firebase.auth.SignInWithEmailAndPasswordAsync(emailLogin.text, passwordLogin.text);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if (LoginTask.Exception != null) {
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            switch (errorCode) {
                case AuthError.MissingEmail:
                    firebase.ui.UpdateHintText(1);
                    break;
                case AuthError.MissingPassword:
                    firebase.ui.UpdateHintText(2);
                    break;
                case AuthError.WrongPassword:
                    firebase.ui.UpdateHintText(3);
                    break;
                case AuthError.InvalidEmail:
                    firebase.ui.UpdateHintText(4);
                    break;
                case AuthError.UserNotFound:
                    firebase.ui.UpdateHintText(5);
                    break;
                case AuthError.CredentialAlreadyInUse:
                    firebase.ui.UpdateHintText(7);
                    break;
                default:
                    firebase.ui.UpdateHintText(-1);
                    break;
            }
            firebase.ui.ClearLoginFields();
            loginButton.enabled = false;
            Invoke("EnableLoginButton", 3f);
        } else {
            firebase.User = LoginTask.Result.User;
            firebase.usernameField.text = firebase.User.DisplayName;
            firebase.isLoggedIn = true;
            firebase.ui.UpdateHintText(6);
            StartCoroutine(LoadUserData());
        }
    }

    public IEnumerator LoadUserData() {
        //Užkraunami vartotojo duomenys
        yield return new WaitForSecondsRealtime(0.3f);
        EnableLoginButton();
        Task<DataSnapshot> DBTask = firebase.DBreference.Child("users").Child(firebase.User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Result.Value == null) {
            firebase.SaveDataButton();
        } else {
            firebase.usernameField.text = firebase.User.DisplayName;
            DataSnapshot snapshot = DBTask.Result;
            gm.data.level1 = int.Parse(snapshot.Child("highscore1").Value.ToString());
            gm.data.level2 = int.Parse(snapshot.Child("highscore2").Value.ToString());
            gm.data.level3 = int.Parse(snapshot.Child("highscore3").Value.ToString());
            gm.data.coins = int.Parse(snapshot.Child("coins").Value.ToString());
            gm.data.frogBodyOwned = Boolean.Parse(snapshot.Child("characters1").Value.ToString());
            gm.data.thirdPlayerBodyOwned = Boolean.Parse(snapshot.Child("characters2").Value.ToString());
            player.ChangePlayer(0);
            gm.SaveData();
            firebase.SaveDataButton();
        }
        mc.ClickProfileButton();
        firebase.ui.ClearLoginFields();
    }

    public void EnableLoginButton() {
        loginButton.enabled = true;
        loginText.text = "";
    }
}