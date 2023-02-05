using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform pointOne;
    [SerializeField] private Transform pointTwo;
    [SerializeField] private bool runOnStart;

    public UnityEvent OnCompleted;

    private void Start()
    {
        if (runOnStart)
        {
            MoveCamera();
        }
    }

    public async void MoveCamera()
    {
        float progress = 0;

        while (progress <= 1)
        {
            camera.transform.position = Vector3.Lerp(pointOne.position, pointTwo.position, progress);
            camera.transform.eulerAngles = Vector3.Lerp(pointOne.eulerAngles, pointTwo.eulerAngles, progress);
            progress += speed * Time.deltaTime;
            await Task.Yield();
        }
        
        OnCompleted?.Invoke();
    }
}
