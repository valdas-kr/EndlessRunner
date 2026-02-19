using UnityEngine;

public class PauseGame : MonoBehaviour {
    //Ar sustabdytas žaidimas
    public static bool gameIsPaused;

    //Pauzės meniu
    [SerializeField] GameObject pauseMenu;

    public void StopGame() {
        //Kai paspaudžiamas sustabdymo mygtukas, gaunama atvirkštinė reikšmė
        gameIsPaused = !gameIsPaused;

        //Pagal naują reikšmę žaidimas yra sustabdomas arba veikia toliau
        if (gameIsPaused) {
            //Atidaromas meniu, sustabdomi laikas ir muzika
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            AudioListener.pause = true;
        } else {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            AudioListener.pause = false;
        }
    }
}