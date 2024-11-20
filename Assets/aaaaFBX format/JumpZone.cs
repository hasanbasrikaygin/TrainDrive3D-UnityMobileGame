using UnityEngine;

public class JumpZone : MonoBehaviour
{
    public float jumpForce = 50f;
    public float jumpDuration = 2f;
    public Train train;
    // Diðer kodlar...

    private void OnTriggerEnter(Collider other)
    {
        // JumpZone'a girildiðinde
        if (other.CompareTag("Train"))
        {
            AudioManager.Instance.PlayJump();
            train.TransitionToState(train.JumpState);
        }
    }
}
