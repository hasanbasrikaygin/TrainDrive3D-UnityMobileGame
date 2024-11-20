using System.Collections;
using UnityEngine;

public class JumpState : TrainState
{
    private Vector3 jumpDirection;
    private Vector3 startPosition;
    private Vector3 peakPosition;
    private Vector3 endPosition;
    private float jumpForceForward = 9f;
    private float jumpHeight = 5f;
    private float jumpDuration = 1f;

    public JumpState(Train train) : base(train) { }

    public override void EnterState()
    {
        // Get the current forward direction of the train
        jumpDirection = train.transform.forward;

        // Record the start position of the jump
        startPosition = train.transform.position;

        // Calculate the peak position (highest point in the jump)
        peakPosition = startPosition + jumpDirection * jumpForceForward + Vector3.up * jumpHeight;

        // Calculate the end position (landing point)
        endPosition = startPosition + jumpDirection * (jumpForceForward * 2);

        // Start the jump
        train.StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;

            // Using Mathf.Sin to create a smooth arc
            float heightFactor = Mathf.Sin(t * Mathf.PI); // This smoothly goes from 0 to 1 back to 0

            // Interpolating between start and end positions with a smooth arc
            train.transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * heightFactor * jumpHeight;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the train reaches the exact end position
        train.transform.position = endPosition;

        // Transition back to moving state
        train.TransitionToState(train.MoveState);
    }

    public override void ExitState()
    {
        // Any cleanup needed when exiting the jump state
    }

    public override void UpdateState()
    {
        FollowBodyParts();
    }

    public override void FixedUpdateState()
    {
       
    }
    private void FollowBodyParts()
    {
        for (int i = 0; i < train.bodyParts.Count; i++)
        {
            int index = Mathf.Min((i + 1) * Mathf.RoundToInt(train.bodyDistance / train.positionRecordInterval), train.positions.Count - 1);

            Vector3 targetPosition = train.positions[index];
            GameObject bodyPart = train.bodyParts[i];

            if (Vector3.Distance(bodyPart.transform.position, targetPosition) > 0.1f) // Mesafe kontrolü ekledim
            {
                bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, targetPosition, train.bodyFollowSpeed * Time.deltaTime);
                bodyPart.transform.LookAt(targetPosition);
            }
            else
            {
                bodyPart.transform.position = targetPosition; // Doðrudan pozisyon atama
            }
        }

    }
}
