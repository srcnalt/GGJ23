using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform fog;

    private void Update()
    {
        fog.Rotate(Vector3.forward, Time.deltaTime * 30);
    }
}
