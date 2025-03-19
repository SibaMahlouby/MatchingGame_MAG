// Destroy the GameObject after a specified time.

using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2.0f;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
