using System;
using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AIRequest : MonoBehaviour
{
    [SerializeField] private Material screen;
    [SerializeField] private InputField userInput;
    [SerializeField] private Text resultTarget;
    
    private OpenAIApi openai = new OpenAIApi();

    public UnityEvent OnCompleted;

    private string Instruction =
        "Act as a fortune teller who is about to tell a stranger who they were in their past life according to how they describe themselves. " +
        "Make up a response not using the same info provided but relate the answer to it from different angles." +
        "Answer at most in 60 words.\n\nStranger: I am a ";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public async void GenerateResponse()
    {
        var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
        {
            Prompt = Instruction + userInput.text + ".\n\nFortune teller: ",
            Stop = "\n",
            Model = "text-davinci-003",
            MaxTokens = 128,
            Temperature = 0.7f
        });

        var text = completionResponse.Choices[0].Text.Trim();
        
        Debug.Log(text);

        var response = await openai.CreateImage(new CreateImageRequest
        {
            Prompt = "Photo of a person, realistic, polaroid, old photo, 8k, " + text,
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
                resultTarget.text = text;
                OnCompleted?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("No image was created from this prompt.");
        }
    }
}
