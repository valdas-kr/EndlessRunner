using UnityEngine; using UnityEngine.Localization.Settings;

public class SettingsData : MonoBehaviour {
    //Nustatymų valdymas ir garso lygis
    [SerializeField] SettingsController settings;
    private float sliderValue;

    public void SaveSettingsData() {
        //Išsaugomi nustatymai, kurie bus gaunami perkrovus žaidimą
        //Žaidimo valdymas
        PlayerPrefs.SetInt("JumpKey", (int)settings.input.jumpKey);
        PlayerPrefs.SetInt("PauseKey", (int)settings.input.pauseKey);

        //Muzika ir efektai
        settings.sound.audioMixer.GetFloat("MusicVolume", out sliderValue);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        settings.sound.audioMixer.GetFloat("EffectsVolume", out sliderValue);
        PlayerPrefs.SetFloat("EffectsVolume", sliderValue);

        //Kalba
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1]) {
            PlayerPrefs.SetString("Language", "lt");
        } else {
            PlayerPrefs.SetString("Language", "en");
        }

        //Paskutinis pasirinktas lygis
        PlayerPrefs.SetInt("lastLevel", settings.level.levelId);

        //Paskutinis naudotas veikėjas
        PlayerPrefs.SetInt("PlayerPicked", settings.player.playerPicked);
        PlayerPrefs.Save();
    }

    public void LoadSettingsData() {
        //Užkraunami visi išsaugoti nustatymai
        //Žaidimo valdymas
        if (PlayerPrefs.HasKey("JumpKey")) {
            settings.input.jumpKey = (KeyCode)PlayerPrefs.GetInt("JumpKey");
        } else {
            settings.input.jumpKey = KeyCode.Space;
        }
        if (PlayerPrefs.HasKey("PauseKey")) {
            settings.input.pauseKey = (KeyCode)PlayerPrefs.GetInt("PauseKey");
        } else {
            settings.input.pauseKey = KeyCode.Escape;
        }

        //Muzika ir efektai
        if (PlayerPrefs.HasKey("MusicVolume")) {
            settings.sound.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        } else {
            settings.sound.audioMixer.SetFloat("MusicVolume", -20f);
        }
        if (PlayerPrefs.HasKey("EffectsVolume")) {
            settings.sound.audioMixer.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("EffectsVolume"));
        } else {
            settings.sound.audioMixer.SetFloat("EffectsVolume", -20f);
        }

        //Kalba
        if (PlayerPrefs.HasKey("Language")) {
            if (PlayerPrefs.GetString("Language") == "lt") {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            } else {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            }
        } else {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }

        //Paskutinis pasirinktas lygis
        if (PlayerPrefs.HasKey("lastLevel")) {
            if (PlayerPrefs.GetInt("lastLevel") == 1) {
                settings.level.ChangeGameLevel(1);
            } else if (PlayerPrefs.GetInt("lastLevel") == 2) {
                settings.level.ChangeGameLevel(2);
            } else {
                settings.level.ChangeGameLevel(0);
            }
        } else {
            settings.level.ChangeGameLevel(0);
        }

        //Paskutinis naudotas veikėjas
        if (PlayerPrefs.HasKey("PlayerPicked")) {
            if (PlayerPrefs.GetInt("PlayerPicked") == 1 && settings.gm.data.frogBodyOwned == true) {
                settings.player.ChangePlayer(1);
            } else if (PlayerPrefs.GetInt("PlayerPicked") == 2 && settings.gm.data.thirdPlayerBodyOwned == true) {
                settings.player.ChangePlayer(2);
            } else {
                settings.player.ChangePlayer(0);
            }
        } else {
            settings.player.ChangePlayer(0);
        }

        UpdateSettingsUI();
    }
    public void UpdateSettingsUI() {
        //Atnaujinami nustatymų lango vartotojo sąsajos elementai
        settings.input.UpdateButtonText();
        settings.sound.UpdateSliders();
        settings.screen.UpdateScreenCheck();
        settings.screen.FillResolutionDropdown();
    }
}