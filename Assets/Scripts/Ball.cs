using System.Threading.Tasks;
using OpenAI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform fog;
    [SerializeField] private Material screen;
    
    private OpenAIApi openai = new OpenAIApi();

    private void Update()
    {
        fog.Rotate(Vector3.forward, Time.deltaTime * 30);
    }

    public async void SetImage()
    {
        var response = await openai.CreateImage(new CreateImageRequest
        {
            Prompt = "forest",
            Size = ImageSize.Size256
        });
        
        if (response.Data != null)
        {
            using(var request = new UnityWebRequest(response.Data[0].Url))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SendWebRequest();

                while (!request.isDone) await Task.Yield();

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(request.downloadHandler.data);
                
                screen.SetTexture("_BaseMap", texture);
            }
        }
        else
        {
            Debug.LogWarning("No image was created from this prompt.");
        }
    }
}
