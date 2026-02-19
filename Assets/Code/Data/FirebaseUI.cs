using TMPro; using UnityEngine; using UnityEngine.Localization.Settings;

public class FirebaseUI : MonoBehaviour {
    [SerializeField] FirebaseManager firebase;
    [SerializeField] GameManager gm;

    //Profilio lange esantys teksto elementai
    [SerializeField] TMP_Text allCoinsText;
    [SerializeField] TMP_Text highscore1Text;
    [SerializeField] TMP_Text highscore2Text;
    [SerializeField] TMP_Text highscore3Text;
    [SerializeField] TMP_Text hintText;

    public void ClearRegisterFields() {
        //Išvalomi registracijos laukai
        firebase.register.usernameRegister.text = "";
        firebase.register.emailRegister.text = "";
        firebase.register.passwordRegister.text = "";
        firebase.register.passwordConfirmRegister.text = "";
    }

    public void ClearLoginFields() {
        //Išvalomi prisijungimo laukai
        firebase.login.emailLogin.text = "";
        firebase.login.passwordLogin.text = "";
    }

    public void UpdateProfileUI() {
        //Atnaujinami profilio lango elementai
        firebase.usernameField.text = firebase.User.DisplayName;
        highscore1Text.text = gm.data.level1.ToString();
        highscore2Text.text = gm.data.level2.ToString();
        highscore3Text.text = gm.data.level3.ToString();
        allCoinsText.text = gm.data.coins.ToString();
    }

    public void UpdateHintText(int code) {
        //Atnaujinamas pagalbinis tekstas pagal duotą kodą
        if (LocalizationSettings.SelectedLocale.ToString() == "Lithuanian (lt)") {
            hintText.text = code switch {
                1 => "Trūksta el. pašto!",
                2 => "Trūksta slaptažodžio!",
                3 => "Neteisingas slaptažodis!",
                4 => "Neteisingas el. paštas!",
                5 => "Paskyra neegzistuoja!",
                6 => "Prisijungta sėkmingai!",
                7 => "Paskyra jau prijungta!",

                8 => "Trūksta vartotojo vardo!",
                9 => "Slaptažodžiai nesutampa!",
                10 => "Slaptažodis turi būti bent 8 simbolių ilgio!",
                11 => "Slaptažodis turi turėti bent vieną skaičių!",
                12 => "Slaptažodis turi turėti bent vieną didžiąją raidę!",
                13 => "Slaptažodis turi turėti bent vieną mažąją raidę!",
                14 => "Trūksta el. pašto!",
                15 => "Trūksta slaptažodžio!",
                16 => "Silpnas slaptažodis!",
                17 => "El. paštas yra naudojamas!",
                18 => "Registracija sėkminga!",

                19 => "Nepavyko ištrinti paskyros!",

                _ => "Įvyko klaida!",
            };
        } else {
            hintText.text = code switch {
                1 => "Missing Email!",
                2 => "Missing Password!",
                3 => "Wrong Password!",
                4 => "Invalid Email!",
                5 => "Account Does Not Exist!",
                6 => "Logged In!",
                7 => "Account Is Already In Use!",

                8 => "Missing Username!",
                9 => "Password Does Not Match!",
                10 => "Password Must Be At Least 8 Symbols Long!",
                11 => "Password Must Have At Least 1 Number!",
                12 => "Password Must Have At Least 1 Upper Case Letter!",
                13 => "Password Must Have At Least 1 Lower Case Letter!",
                14 => "Missing Email!",
                15 => "Missing Password!",
                16 => "Weak Password!",
                17 => "Email Already In Use!",
                18 => "Registration successful!",

                19 => "Unable To Delete Account!",

                _ => "Error Ocurred!",
            };
        }
    }
}