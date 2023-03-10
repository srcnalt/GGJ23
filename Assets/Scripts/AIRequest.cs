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
    [SerializeField] private Texture defaultTexture;
    
    private OpenAIApi openai = new OpenAIApi();

    public UnityEvent OnCompleted;

    private string instruction =
        "Act as a fortune teller who is about to tell a stranger who they were in their past life according to how they describe themselves. " +
        "Make up a response not using the same info provided but relate the answer to it from different angles." +
        "If the strangers input is too short, make up some random answer for fortune teller." +
        "Answer at most in 60 words.\n\nStranger: I am a ";

    private string photoInstruction = "Photo of a person, face centered, realistic, old photo, pastel colors, 8k, ";

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
            Prompt = instruction + userInput.text + ".\n\nFortune teller: ",
            Stop = "\n",
            Model = "text-davinci-003",
            MaxTokens = 128,
            Temperature = 0.7f
        });

        var text = completionResponse.Choices[0].Text.Trim();
        
        Debug.Log(text);

        var response = await openai.CreateImage(new CreateImageRequest
        {
            Prompt = photoInstruction + text,
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

    public void CleanScreen()
    {
        screen.SetTexture("_BaseMap", defaultTexture);
    }
}
