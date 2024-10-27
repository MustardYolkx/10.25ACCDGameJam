using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SpinUI : MonoBehaviour
{
    public List<Button> buttons; // Assign the three buttons in the Inspector
    public List<Sprite> winImages; // Assign replacement images for each button in the Inspector
    private Dictionary<Button, float> originalAngles = new Dictionary<Button, float>(); // Store original rotation angles

    public event Action OnMiniGameSuccess;
    public TextMeshProUGUI completionText;

    void Start()
    {
        completionText.gameObject.SetActive(false);

        foreach (Button button in buttons)
        {
            // Store each button's original angle (0 degrees)
            originalAngles[button] = 0f;

            // Set initial rotation of the button to 0 degrees (the original angle)
            button.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Apply a random rotation (0, 90, 180, or 270 degrees) to the button
            float randomAngle = 90 * UnityEngine.Random.Range(0, 4);
            button.transform.Rotate(0, 0, randomAngle);

            // Add a click listener to rotate the button
            button.onClick.AddListener(() => RotateButton(button));
        }
    }

    private void RotateButton(Button button)
    {
        // Rotate the button by 90 degrees
        button.transform.Rotate(0, 0, 90);

        // Check if the player has won
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        foreach (Button button in buttons)
        {
            // Get the current z rotation of the button and normalize it to 0, 90, 180, or 270
            float currentAngle = button.transform.eulerAngles.z % 360;
            currentAngle = Mathf.Round(currentAngle / 90) * 90;

            // If the current angle does not match the original (0 degrees), return
            if (currentAngle != originalAngles[button])
                return;
        }

        // If all buttons match their original angles, call GameSuccess
        GameSuccess();
    }

    private void GameSuccess()
    {
        Debug.Log("win!");
        completionText.gameObject.SetActive(true);

        // Replace each button¡¯s specified child with a new image
        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];

            // Find and remove a specific child of the button (e.g., by name)
            Transform childToRemove = button.transform.Find("C1"); // Replace with the actual name of the child to remove
            if (childToRemove != null)
            {
                Destroy(childToRemove.gameObject); // Delete the specific child
            }

            // Add a new Image component as a replacement
            GameObject newImageObject = new GameObject("WinImage");
            newImageObject.transform.SetParent(button.transform, false);
            Image newImage = newImageObject.AddComponent<Image>();

            if (i < winImages.Count)
            {
                newImage.sprite = winImages[i]; // Set the win image sprite
            }
        }

        // Invoke the OnMiniGameSuccess event
        OnMiniGameSuccess?.Invoke();
    }
}
