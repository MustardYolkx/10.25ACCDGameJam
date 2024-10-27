using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class SpinUI : MonoBehaviour
{
    public List<Button> buttons; // Assign the three buttons in the Inspector
    private Dictionary<Button, float> originalAngles = new Dictionary<Button, float>();  // Store original rotation angles

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
            float randomAngle = 90 * Random.Range(0, 4);
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

        // If all buttons match their original angles, the game is won
        Debug.Log("win!");
        completionText.gameObject.SetActive(true);
       
    }
}
