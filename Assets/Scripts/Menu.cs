using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public UnityEvent OnStart;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Application.OpenURL("https://github.com/srcnalt/OpenAI-Unity#saving-your-credentials");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnStart?.Invoke();
        }
    }
}
