using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTimeSeconds;

    void Start()
    {
        Destroy(gameObject, lifeTimeSeconds);
    }
}