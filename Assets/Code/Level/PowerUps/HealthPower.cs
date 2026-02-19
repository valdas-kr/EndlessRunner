using UnityEngine;

public class HealthPower : PowerUp {
    protected override void StartLogic(Collider2D player) {
        //Paleidžiamas garsas, vykdomas efektas, išjungiami komponentai
        sound.Play();
        player.GetComponentInChildren<PlayerMovement>().livesUsed = 0;
        player.GetComponentInChildren<PlayerMovement>().UpdateHeartsUI();
    }
}