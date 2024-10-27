using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomCodeGame : MonoBehaviour
{
    public TMP_InputField playerInput; // Reference to the input field
    private string generatedCode; // The random code generated
    private TextMeshProUGUI codeDisplay; // The text component for displaying the code

    public TextMeshProUGUI completionText;

    void Start()
    {
        codeDisplay = GetComponent<TextMeshProUGUI>(); // Get the TextMeshProUGUI component
        GenerateCode();
        playerInput.onSubmit.AddListener(delegate { CheckInput(); });

        completionText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (playerInput.isFocused)
            {
                CheckInput();
            }
        }
    }

    void GenerateCode()
    {
        generatedCode = "";
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        for (int i = 0; i < 4; i++)
        {
            generatedCode += chars[Random.Range(0, chars.Length)];
        }

        codeDisplay.text = generatedCode;
        playerInput.text = "";
    }

    void CheckInput()
    {
        if (playerInput.text.ToUpper() == generatedCode)
        {
            Debug.Log("You win!");
            completionText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Incorrect. Try again!");
            playerInput.text = ""; // Clear input
            GenerateCode();
        }
    }
}
