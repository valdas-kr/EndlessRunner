using System.Collections; using UnityEngine;

public class SizeDownPower : PowerUp {
    protected override void StartLogic(Collider2D player) {
        //Paleidžiame korutina
        StartCoroutine(Pickup(player));
    }

    public IEnumerator Pickup(Collider2D player) {
        //Paleidžiamas garsas, vykdomas efektas, išjungiami komponentai
        sound.Play();
        player.transform.localScale /= multiplier;

        //Pasibaigus laikui, efektai atgauna pradines reikšmes ir pastiprinimas sunaikinamas
        yield return new WaitForSecondsRealtime(duration);
        player.transform.localScale *= multiplier;
    }
}