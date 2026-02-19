using UnityEngine; using UnityEngine.Audio; using UnityEngine.UI;

public class MusicController : MonoBehaviour {
    //Meniu ir lygių muzika
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource sandyDessertMusic;
    [SerializeField] AudioSource spookyForrestMusic;
    [SerializeField] AudioSource pixelCityMusic;

    //Mirties, pinigų ir pašokimo garso efektai
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource coinSound;
    [SerializeField] AudioSource jumpSound;

    //Garso keitimo elementai
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;

    //Garso nustatymų komponentas, pasirinkta žaidimo muzika ir garso lygis
    public AudioMixer audioMixer;
    private AudioSource gameMusic;
    private float soundValue;

    public void ChangeGameMusic(int id) {
        //Paleidžiama reikiamo lygio muzika, vertė gaunama pagal lygio id
        gameMusic = id switch {
            0 => sandyDessertMusic,
            1 => spookyForrestMusic,
            2 => pixelCityMusic,
            _ => sandyDessertMusic,
        };
    }

    public void StartMenuMusic() {
        //Paleidžiama meniu muzika
        menuMusic.time = Random.Range(0f, menuMusic.clip.length);
        menuMusic.Play();
    }

    public void StartGameMusic() {
        //Paleidžiama lygio muzika
        menuMusic.Stop();
        gameMusic.time = Random.Range(0f, gameMusic.clip.length);
        gameMusic.Play();
    }

    public void StopAllMusic() {
        //Sustabdoma visa muzika
        menuMusic.Stop();
        gameMusic.Stop();
    }

    public void PlayDeathSound() {
        //Paleidžiamas mirties efektas
        deathSound.Play();
    }

    public void PlayCoinSound() {
        //Paleidžiamas pinigų efektas
        coinSound.Play();
    }

    public void PlayJumpSound() {
        //Paleidžiamas pašokimo efektas
        jumpSound.Play();
    }

    public void SetMusicVolume(float volume) {
        //Pakeičiama muzikos nustatymo vertė
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume) {
        //Pakeičiama efektų nustatymo vertė
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    public void UpdateSliders() {
        //Žaidimo pradžioje atnaujinami nustatymų mygtukai
        bool result = audioMixer.GetFloat("MusicVolume", out soundValue);
        if (result) {
            musicSlider.value = soundValue;
        } else {
            musicSlider.value = -20f;
        }
        result = audioMixer.GetFloat("EffectsVolume", out soundValue);
        if (result) {
            effectsSlider.value = soundValue;
        } else {
            effectsSlider.value = -20f;
        }
    }
}