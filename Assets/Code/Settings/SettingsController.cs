using UnityEngine;

public class SettingsController : MonoBehaviour {
    //Valdymo, duomenų apdorojimo, ekrano ir garsų nustatymai
    public InputSettings input;
    [SerializeField] SettingsData data;
    public ScreenSettings screen;
    public MusicController sound;
    public LanguageSettings language;
    public SwitchPlayer player;
    public GameManager gm;
    public ButtonController level;

    void Start() {
        //Užkraunami visi išsaugoti nustatymai
        LoadSettings();
    }

    public void LoadSettings() {
        //Nustatymų užkrovimo paleidimas
        data.LoadSettingsData();
    }

    public void SaveSettings() {
        //Nustatymų išsaugojimo paleidimas
        data.SaveSettingsData();
    }

    public void ChangeJumpButton() {
        //Pašokimo valdymo keitimo paleidimas
        input.StartKeyChange("Jump");
    }

    public void ChangePauseButton() {
        //Pauzės valdymo keitimo paleidimas
        input.StartKeyChange("Pause");
    }

    public void ChangeFullScreen(bool isFullScreen) {
        //Ekrano režimo keitimo paleidimas
        screen.SetFullScreen(isFullScreen);
    }

    public void ChangeResolution(int index) {
        //Ekrano rezoliucijos keitimo paleidimas
        screen.SetNewResolution(index);
    }

    public void ChangeMusicVolume(float volume) {
        //Muzikos keitimo paleidimas
        sound.SetMusicVolume(volume);
    }

    public void ChangeEffectsVolume(float volume) {
        //Garso efektų keitimo paleidimas
        sound.SetEffectsVolume(volume);
    }

    public void ChangeLanguage(string code) {
        //Kalbos keitimo paleidimas
        language.SetLocale(code);
    }
}