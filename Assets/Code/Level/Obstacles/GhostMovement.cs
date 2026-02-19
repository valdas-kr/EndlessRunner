using UnityEngine;

public class GhostMovement : ObstacleMovement {
    private void Start() {
        //Fizikos komponentui suteikiamos masės ir gravitacijos reikšmės
        rb.mass = Random.Range(300f, 4000f);
        rb.gravityScale = Random.Range(-110f, -25f) / 1000;
    }
}