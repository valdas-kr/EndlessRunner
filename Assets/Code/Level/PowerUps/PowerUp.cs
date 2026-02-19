using UnityEngine;

public abstract class PowerUp : MonoBehaviour {
    //Pastiprinimo modifikacijos reikšmė, veikimo laikas ir garso efektas
    [SerializeField] protected float multiplier;
    [SerializeField] protected float duration;
    [SerializeField] protected AudioSource sound;
    [SerializeField] protected Animator anim;

    protected void OnTriggerEnter2D(Collider2D other) {
        //Palietus žaidėją, paleidžiamas metodas
        if (other.CompareTag("Player")) {
            anim.SetBool("collect", true);
            StartLogic(other);
        }
    }

    protected abstract void StartLogic(Collider2D player);
}