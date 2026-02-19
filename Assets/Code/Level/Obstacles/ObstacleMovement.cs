using UnityEngine;

public abstract class ObstacleMovement : MonoBehaviour {
    //Kliūties fizikos komponentas
    protected Rigidbody2D rb;

    private void Awake() {
        //Pridedamas komponentas ir judėjimo greitis
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = Random.Range(-11f, -3f);
    }
}