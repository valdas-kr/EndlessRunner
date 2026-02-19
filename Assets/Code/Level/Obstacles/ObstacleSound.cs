using UnityEngine;

public class ObstacleSound : MonoBehaviour {
    //Garso komponentas
    [SerializeField] AudioSource sound;
    //Laikas tarp garsų ir kaupiamas laikas
    [SerializeField] float soundTime;
    private float soundTimer = 0f;

    private void Update() {
        //Kaupiamas laikas iki kito garso paleidimo
        soundTimer += Time.deltaTime;
        if (soundTimer >= soundTime) {
            //Paleidžiamas kliūties garsas ir laikmatis atstatomas į 0
            sound.Play();
            soundTimer = 0f;
        }
    }
}