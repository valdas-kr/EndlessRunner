using TMPro; using UnityEngine;

public class InputSettings : MonoBehaviour {
    //Mygtukų komponentai
    public TMP_Text jumpButtonText;
    public TMP_Text pauseButtonText;

    //Pašokimo ir žaidimo sustabdymo valdymas
    public KeyCode jumpKey;
    public KeyCode pauseKey;

    //Ar keičiamas mygtukas ir kuris veiksmas atliekamas
    private bool waitingForInput = false;
    private string actionToChange = "";

    private void Update() {
        //Tikrinama, ar keičiamas mygtukas
        //Jei taip, paleidžiama mygtuko priskyrimo funkcija
        if (waitingForInput) DetectNewKey();
    }

    public void StartKeyChange(string action) {
        //Pradedamas mygtuko keitimas
        actionToChange = action;
        waitingForInput = true;
    }

    private void DetectNewKey() {
        //Priskiriamas naujas mygtukas, Randamas paspaustas mygtukas
        if (Input.anyKeyDown) {
            KeyCode key = GetPressedKey();
            //Jei naujas mygtukas nėra priskirtas kitam veiksmui, jis yra atnaujinamas
            if (key != KeyCode.None && key != jumpKey && key != pauseKey) {
                //Atnaujinamas reikiamas mygtukas
                if (actionToChange == "Jump") jumpKey = key;
                if (actionToChange == "Pause") pauseKey = key;
            }
            //Atnaujinamas tekstas ir baigiamas keitimas
            waitingForInput = false;
            UpdateButtonText();
        }
    }

    private KeyCode GetPressedKey() {
        //Gaunamas paspaustas naujas mygtukas
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(keyCode)) return keyCode;
        }
        return KeyCode.None;
    }

    public void UpdateButtonText() {
        //Atnaujinami mygtukų tekstai
        jumpButtonText.text = "(" + jumpKey + ")";
        pauseButtonText.text = "(" + pauseKey + ")";
    }
}