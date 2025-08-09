using UnityEngine;
using DG.Tweening;

public class FloatingPlatformTween : MonoBehaviour
{
    public float bobAmplitude = 0.5f;
    public float bobTime = 1.5f;

    void Start()
    {
        Vector3 upPos = transform.position + Vector3.up * bobAmplitude;

        transform.DOMove(upPos, bobTime)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo);
    }
}
