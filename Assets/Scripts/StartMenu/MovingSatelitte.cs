using DG.Tweening;
using UnityEngine;

public class MovingSatelitte : MonoBehaviour
{
    public float moveDistance = 0.5f; // Distance to float up
    public float duration = 1.5f;     // Time taken to move up
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         // Target position is just above the starting position
        Vector3 targetPos = transform.position + new Vector3(0, moveDistance, 0);

        // Moves to target and back to start continuously
        transform.DOMove(targetPos, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

   
}
