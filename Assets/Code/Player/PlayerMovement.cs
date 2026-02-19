using System.Collections; using UnityEngine;
using TMPro; using UnityEngine.Localization.Settings;

public class PlayerMovement : MonoBehaviour {
    //Veikėjo pašokimų jėga, kiekis, masė ir gyvybių kiekis
    [SerializeField] float jumpForce;
    [SerializeField] float secondJumpForce;
    [SerializeField] int maxJumps;
    [SerializeField] float mass;
    [SerializeField] int lives;

    //Veikėjo animacijos, dabartinis veikėjas ir fizikoas komponentas
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Animator anim;
    [SerializeField] SwitchPlayer playerPicked;
    private Rigidbody2D rb;

    //Valdymo nustatymai
    [SerializeField] InputSettings input;
    [SerializeField] MusicController mc;
    [SerializeField] GameObject player;
    public TextMeshProUGUI heartsUI;

    //Parinkto veikėjo pašokimų kiekis
    private int jumps = 0;
    public int livesUsed = 0;
    private bool canCollide = true;

    //Veikėjo būsena: ant žemės ir ore
    private bool isGrounded;
    public bool isJumping = false;

    protected void Start() {
        //Žaidimo pradžioje gaunamas veikėjo fizikos komponentas
        rb = player.GetComponentInParent<Rigidbody2D>();
    }

    public void Update() {
        //Žaidimo metu tikrinama:
        //Jei žaidėjas iškrenta iš žaidimo, jo pozicija yra atstatoma
        if (player.transform.position.x != -4.5f || player.transform.position.y > 10f || player.transform.position.y < -6f) 
            player.transform.position = new Vector3(-4.5f, 4.5f, 0f);
        
        if (!PauseGame.gameIsPaused) {
            //Jei žaidimas nėra sutabdytas, veikia valdymas
            rb.mass = mass;
            //Pašokimas
            if (isGrounded && Input.GetKeyDown(input.jumpKey)) PlayerJump();
            //Sekantys pašokimai
            if (!isGrounded && jumps < maxJumps && Input.GetKeyDown(input.jumpKey)) PlayerDoubleJump();
            //Kritimas
            if (!isGrounded && Input.GetKeyUp(input.jumpKey)) PlayerFall();
        }
        if (!GameManager.Instance.isPlaying) ResetHeartsUI();
    }

    private void LifeLost() {
        //Praradus gyvybę, veikėjo grafikos išsijungia / įsijungia
        SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
        sprite.enabled = !sprite.enabled;
    }

    private IEnumerator ResetLife() {
        //Išjungiami susidūrimai su kliūtimis
        canCollide = false;

        //Paleidžiamas veikėjo grafikos įjungimas / išjungimas
        InvokeRepeating("LifeLost", 0.0f, 0.1f);

        //Po nustatyto laiko atstatomos reikšmės
        yield return new WaitForSecondsRealtime(1.5f);
        CancelInvoke("LifeLost");
        player.GetComponent<SpriteRenderer>().enabled = true;
        canCollide = true;
    }

    private void EndGame() {
        livesUsed = 0;
        GameManager.Instance.GameOver();
        playerAnimation.PlayCharacterAnim(anim, "reset");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag("Obstacle") && canCollide) {
            //Jei paliečiama kliūtis ir galimi susidūrimai
            //Padidinamas panaudotų gyvybių kiekis
            livesUsed++;
            if (lives - livesUsed == 0) {
                //Jei gyvybių nebėra, žaidimas pasibaigia
                playerAnimation.PlayCharacterAnim(anim, "dead");
                ResetHeartsUI();
                Invoke("EndGame", 0.8f);
            } else if (lives - livesUsed > 0) {
                //Jei gyvybių yra, paleidžiama korutina
                UpdateHeartsUI();
                StartCoroutine(ResetLife());
            }
        }

        if (other.gameObject.CompareTag("Ground")) {
            //Tikrinama, ar veikėjas yra ant žemės
            isGrounded = true;
            playerAnimation.PlayCharacterAnim(anim, "ground");
            jumps = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.CompareTag("Coin")) {
            //Palietus pinigą, yra paleidžiamas įvykis
            GameManager.Instance.CoinCollected();
            mc.PlayCoinSound();
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        //Palietus pastiprinimą, yra paleižiamas įvykis
        if (other.transform.CompareTag("PowerUp")) GameManager.Instance.CoinCollected();
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            //Tikrinama, ar veikėjas yra ore
            isGrounded = false;
            playerAnimation.PlayCharacterAnim(anim, "jump");
        }
    }

    public void PlayerJump() {
        //Paleidžiamas garso efektas ir veikėjas šoka į viršų
        mc.PlayJumpSound();
        jumps++;
        rb.linearVelocity = new Vector2(0f, jumpForce);
    }

    public void PlayerFall() {
        //Veikėjas krenta žemyn
        isJumping = false;
        playerAnimation.PlayCharacterAnim(anim, "fall");
    }

    private void PlayerDoubleJump() {
        //Veikėjas antrą kartą šoka aukštyn
        rb.linearVelocity = new Vector2(0f, secondJumpForce);
        playerAnimation.PlayCharacterAnim(anim, "dJump");
        mc.PlayJumpSound();
        jumps++;
    }

    public void UpdateHeartsUI() {
        //Atnaujinamas likusių gyvybių skaičius
        if (LocalizationSettings.SelectedLocale.ToString() == "Lithuanian (lt)") {
            heartsUI.text = "Gyvybės: " + (lives - livesUsed);
        } else {
            heartsUI.text = "Lives: " + (lives - livesUsed);
        }
    }

    public void ResetHeartsUI() {
        heartsUI.text = "";
    }
}