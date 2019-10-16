using UnityEngine;
using Phenix.Unity.Utilities;

public class blink : MonoBehaviour
{
    public Vector3 scaleSmooth = new Vector3(0.2f, 0.2f, 0.2f);
    public float speed = 3;

    // Use this for initialization
    void Start()
    {
        TransformTools.Instance.Blink(transform, scaleSmooth, speed);
    }
}
