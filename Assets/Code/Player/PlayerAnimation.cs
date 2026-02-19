using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    public void PlayCharacterAnim(Animator anim, string type) {
        //Veikėjų animacijos parinkimas pagal duotą tipą
        anim.SetBool("isGrounded", false);
        anim.SetBool("isFalling", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isDoubleJumping", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isReset", false);
        switch (type) {
            case "ground":
                anim.SetBool("isGrounded", true);
                break;
            case "jump":
                anim.SetBool("isJumping", true);
                break;
            case "dJump":
                anim.SetBool("isDoubleJumping", true);
                break;
            case "fall":
                anim.SetBool("isFalling", true);
                break;
            case "dead":
                anim.SetBool("isDead", true);
                break;
            case "reset":
                anim.SetBool("isReset", true);
                break;
        }
    }
}