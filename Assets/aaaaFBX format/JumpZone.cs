using UnityEngine;

public class JumpZone : MonoBehaviour
{
    public float jumpForce = 50f;
    public float jumpDuration = 2f;
    public Train train;
    // Di�er kodlar...

    private void OnTriggerEnter(Collider other)
    {
        // JumpZone'a girildi�inde
        if (other.CompareTag("Train"))
        {
            AudioManager.Instance.PlayJump();
            train.TransitionToState(train.JumpState);
        }
    }
}
