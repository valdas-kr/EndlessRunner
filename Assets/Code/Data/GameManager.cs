using TMPro; using UnityEngine; using UnityEngine.Events;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour {
    //Sukuriamas klasės objektas (singleton)
    public static GameManager Instance;

    //Valdymo nustatymai
    [SerializeField] MusicController mc;
    [SerializeField] ButtonController chosenLevel;
    [SerializeField] InputSettings input;
    [SerializeField] PauseGame pause;
    [SerializeField] Collider2D[] playerBody;
    public Data data;

    [SerializeField] TextMeshProUGUI gameOverTimeScore;
    [SerializeField] TextMeshProUGUI gameOverObstacleScore;
    [SerializeField] TextMeshProUGUI gameOverCoinsScore;
    [SerializeField] TextMeshProUGUI gameOverTotalScore;
    [SerializeField] TextMeshProUGUI gameOverHighScore;
    [SerializeField] TextMeshProUGUI gameOverTotalCoins;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TextMeshProUGUI scoreUI;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    //Rezultatai: pinigų, kliučių, laiko, bendras, geriausias ir visi surinkti pinigai
    public int coinsScore;
    public int obstaclesScore;
    public float timeScore;
    public int gameScore;
    public float highScore1;
    public float highScore2;
    public float highScore3;
    public int totalCoins;
    public bool frogBodyOwned;
    public bool thirdPlayerBodyOwned;

    //Ar žaidžiama
    public bool isPlaying = false;

    //Įvykiai žaidimo pradžiai, pabaigai, surinkus pinigą, pastiprinimą ir įveikus kliūtį
    public UnityEvent onGameOver = new();

    private void Start() {
        //Užkraunami duomenys
        LoadData();
        mc.StartMenuMusic();
        //Pasibaigus žaidimui, paleidžiamas atitinkamas meniu
        onGameOver.AddListener(ActivateGameOverUI);
    }

    private void Update() {
        //Jei žaidžiama, rezultatas didėja pagal išgyventą laiką
        if (isPlaying) {
            timeScore += Time.deltaTime;
            if (Input.GetKeyDown(input.pauseKey)) pause.StopGame();
        }
    }

    public void LoadData() {
        //Įkeliami išsaugoti duomenys
        Data loadedData = SaveSystem.Load("save");
        if (loadedData != null) {
            data = loadedData;
        } else {
            //Jei nėra duomenų, sukuriamas naujas duomenų kintamasis
            data = new Data {
                level1 = 0,
                level2 = 0,
                level3 = 0,
                coins = 0,
                frogBodyOwned = false,
                thirdPlayerBodyOwned = false,
            };
        }
        //Priskiriamos reikšmės
        totalCoins = data.coins;
        frogBodyOwned = data.frogBodyOwned;
        thirdPlayerBodyOwned = data.thirdPlayerBodyOwned;
    }

    public void StartGame() {
        //Nustatomi kintamieji pradedant žaidimą
        SaveData();
        scoreUI.gameObject.SetActive(true);
        isPlaying = true;
        totalCoins = data.coins;
        timeScore = 0;
        coinsScore = 0;
        obstaclesScore = 0;
        gameScore = 0;
        mc.ChangeGameMusic(chosenLevel.levelId);
        mc.StartGameMusic();
    }

    public void CoinCollected() {
        //Pinigo paėmimas
        coinsScore += 1;
    }

    public void EnemyDefeated() {
        //Kliūties įveikimas
        obstaclesScore += 1;
    }

    public void GameOver() {
        //Žaidimo pabaiga
        //Veikėjų dydžiai ir laikas atstatomi į pradines padėtis, kad vėliau neatsirastų klaidų
        scoreUI.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        for (int i = 0; i < playerBody.Length; i++) {
            playerBody[i].transform.localScale = Vector3.one;
            playerBody[i].transform.position = new Vector3(-4.5f, 3.5f, 0f);
        }

        //Sustabdoma muzika ir paleidžiamas žaidimo pabaigos garsas
        mc.StopAllMusic();
        mc.PlayDeathSound();

        //Apskaičiuojamas bendras rezultatas
        gameScore = coinsScore + obstaclesScore + Mathf.RoundToInt(timeScore);
        if (timeScore < 0 || coinsScore < 0 || obstaclesScore < 0) gameScore = 0;
        
        //Duomenų kintamajam priskiriami visi pinigai
        totalCoins += coinsScore;
        data.coins = totalCoins;

        //Jei rezultatas yra aukščiausias, jis tampa geriausiu
        if (chosenLevel.levelId == 0) {
            if (data.level1 < gameScore) data.level1 = gameScore;
        } else if (chosenLevel.levelId == 1) {
            if (data.level2 < gameScore) data.level2 = gameScore;
        } else {
            if (data.level3 < gameScore) data.level3 = gameScore;
        }

        //Išsaugomi duomenys, baigiamas žaidimas
        SaveData();
        isPlaying = false;
        onGameOver.Invoke();
    }

    public void SaveData() {
        //Išsaugomi surinkti pinigai ir rezultatai
        SaveSystem.Save("save", data);
    }

    private void OnGUI() {
        //Žaidimo metu atvaizduojami rezultatai
        if (LocalizationSettings.SelectedLocale.ToString() == "Lithuanian (lt)") {
            scoreUI.text = "Laikas: " + timeScore.ToString("F0") + "\nPinigai: " + coinsScore + "\nKliūtys: " + obstaclesScore;
        } else {
            scoreUI.text = "Time: " + timeScore.ToString("F0") + "\nCoins: " + coinsScore + "\nObstacles: " + obstaclesScore;
        }
    }

    public void ActivateGameOverUI() {
        //Pasibaigus žaidimui, paleidžiamas meniu ir parodomi rezultatai
        gameOverMenu.SetActive(true);
        if (LocalizationSettings.SelectedLocale.ToString() == "Lithuanian (lt)") {
            gameOverTimeScore.text = "Laikas: " + timeScore.ToString("F0");
            gameOverObstacleScore.text = "Kliūtys: " + obstaclesScore.ToString();
            gameOverCoinsScore.text = "Pinigai: " + coinsScore.ToString();
            gameOverTotalScore.text = "Žaidimo rezultatas: " + gameScore.ToString();
            gameOverTotalCoins.text = "Visi pinigai: " + totalCoins.ToString();
            if (chosenLevel.levelId == 0) {
                gameOverHighScore.text = "Geriausias rezultatas: " + data.level1.ToString("F0");
            } else if (chosenLevel.levelId == 1) {
                gameOverHighScore.text = "Geriausias rezultatas: " + data.level2.ToString("F0");
            } else {
                gameOverHighScore.text = "Geriausias rezultatas: " + data.level3.ToString("F0");
            }
        } else {
            gameOverTimeScore.text = "Time: " + timeScore.ToString("F0");
            gameOverObstacleScore.text = "Obstacles: " + obstaclesScore.ToString();
            gameOverCoinsScore.text = "Coins: " + coinsScore.ToString();
            gameOverTotalScore.text = "Game Score: " + gameScore.ToString();
            gameOverTotalCoins.text = "Total Coins: " + totalCoins.ToString();
            if (chosenLevel.levelId == 0) {
                gameOverHighScore.text = "Highscore: " + data.level1.ToString("F0");
            } else if (chosenLevel.levelId == 1) {
                gameOverHighScore.text = "Highscore: " + data.level2.ToString("F0");
            } else {
                gameOverHighScore.text = "Highscore: " + data.level3.ToString("F0");
            }
        } 
    }
}