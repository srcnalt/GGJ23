using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * 30);
    }
}
