using UnityEngine;

public class PickupPosition : MonoBehaviour {
    //Objekto fizikos komponentas
    private Rigidbody2D rb;

    //X ir Y reikšmės
    private float posY, posX;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        //Sugeneruojamos atsitiktinės X ir Y reikšmės
        posY = Random.Range(2.5f, 7.5f);
        posX = Random.Range(15f, 30f);

        //Objektui suteikiama pozicija
        rb.transform.position = new Vector2(posX, posY);
    }
}