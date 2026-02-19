using UnityEngine;

public class DestroyObject : MonoBehaviour {

    private void Update() {
        //Pasibaigus žaidimui, visos kliūtys sunaikinamos
        if (!GameManager.Instance.isPlaying) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Kliūtys sunaikinamos kai paliečia nematomą ribą
        if (other.transform.CompareTag("Destroy")) {
            if (gameObject.CompareTag("Obstacle")) {
                GameManager.Instance.EnemyDefeated();
            }
            Destroy(gameObject);
        }
    }
}