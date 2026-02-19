using System.Collections.Generic; using System.Linq;
using TMPro; using UnityEngine; using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour {
    //Visos galimos ekrano rezoliucijos, naudojamos rezoliucijos ir laukelio užpildymas
    private Resolution[] allResolutions;
    private List<Vector2Int> validRes = new();

    //rezoliucijų keitimo ir ekrano režimo mygtukai
    [SerializeField] TMP_Dropdown resolutionDropdown;
    public Toggle screenTypeToggle;

    //Išsaugomas rezoliucijos indeksas, aukštis ir plotis
    public int savedIndex;
    public int selectedWidth;
    public int selectedHeight;

    public void FillResolutionDropdown() {
        //Leidžiamos rezoliucijos
        List<Vector2Int> allowedResolutions = new() {
            new(720, 480), new(800, 600), new(1024, 768), new(1280, 960), new(1280, 1024),
            new(800, 1280),new(1280, 720), new(1366, 768), new(1440, 900), new(1600, 900), new(1680, 1050),
            new(1920, 1200), new(1920, 1080), new(2560, 1600), new(2560, 1440), new(3840, 2160)
        };

        //Išvalomas dropdown laukelis, kad nebūtų neteisingų ar atsitiktinių reikšmių
        resolutionDropdown.ClearOptions();

        //Priskiriamos rezoliucijos
        allResolutions = Screen.resolutions;

        //Gaunamos visos tinkamos rezoliucijos
        for (int i = 0; i < allResolutions.Length; i++) {
            Vector2Int tempRes = new (allResolutions[i].width, allResolutions[i].height);
            if (allowedResolutions.Contains(tempRes) && !validRes.Contains(tempRes)) validRes.Add(tempRes);
        }

        //Rezoliucijos paverčiamos į teksto tipą, kad būtų galima atvaizduoti
        List<string> options = validRes.Select(res => $"{res.x}x{res.y}").ToList();

        //Į laukelį pridedamos tinkamos reikšmės
        resolutionDropdown.AddOptions(options);

        //Surandama dabartinė rezoliucija ir jos vertė įdedama į pirmą laukelį
        Vector2Int currentRes = new(Screen.currentResolution.width, Screen.currentResolution.height);
        int currentIndex = validRes.IndexOf(currentRes);
        resolutionDropdown.value = currentIndex;

        //Atnaujinamas laukelis
        resolutionDropdown.RefreshShownValue();

        SetNewResolution(currentIndex);
    }

    public void SetNewResolution(int index) {
        //Randama rezoliucija, kurią norima naudoti
        selectedWidth = validRes[index].x;
        selectedHeight = validRes[index].y;

        //Nustatoma nauja rezoliucija ir pritaikomas ekrano režimas
        if (Screen.fullScreen == true) {
            Screen.SetResolution(selectedWidth, selectedHeight, FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRateRatio);
        } else {
            Screen.SetResolution(selectedWidth, selectedHeight, FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio);
        }
    }

    public void SetFullScreen(bool isFullScreen) {
        //Priskiriamas ekrano režimas
        Screen.fullScreen = isFullScreen;
    }

    public void UpdateScreenCheck() {
        //Žaidimo pradžioje atnaujinamas ekrano režimo mygtukas
        if (Screen.fullScreen == true) {
            screenTypeToggle.isOn = true;
        } else {
            screenTypeToggle.isOn = false;
        }
    }
}