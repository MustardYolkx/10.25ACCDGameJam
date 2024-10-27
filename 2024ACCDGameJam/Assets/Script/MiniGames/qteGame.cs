using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class qteGame : MonoBehaviour
{
    public RectTransform pointer; // The pointer UI element
    public float rotationSpeed = 100f; // Speed of the pointer
    private bool isGameActive = false; // Whether the game is running

    private float targetAngle = 90f; // Half-range for the swing angle
    private float tolerance = 5f; // Allowed deviation for the hit
    private float hitZoneAngle1 = 45f; // First hit zone angle
    private float hitZoneAngle2 = -45f; // Second hit zone angle

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (isGameActive)
        {
            RotatePointer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckHit();
            }
        }
    }

    private void RotatePointer()
    {
        // Use PingPong to swing between 90 and -90 degrees
        float angle = Mathf.PingPong(Time.time * rotationSpeed, targetAngle * 2) - targetAngle;
        pointer.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void CheckHit()
    {
        // Get the current angle of the pointer
        float pointerAngle = pointer.localEulerAngles.z;

        // Adjust angle to be within -180 to 180 degrees for easy comparison
        if (pointerAngle > 180)
        {
            pointerAngle -= 360;
        }

        // Check if the pointer is within either hit zone
        bool inHitZone1 = Mathf.Abs(pointerAngle - hitZoneAngle1) <= tolerance;
        bool inHitZone2 = Mathf.Abs(pointerAngle - hitZoneAngle2) <= tolerance;

        if (inHitZone1 || inHitZone2)
        {
            GameSuccess(); // Player succeeded
        }
        else
        {
            GameFailure(); // Player missed
        }
    }

    private void GameSuccess()
    {
        isGameActive = false; // Stop the game
        Debug.Log("Win"); // Log success
        // Add additional success logic here
    }

    private void GameFailure()
    {
        Debug.Log("Fail"); // Log failure
        // Add additional failure logic here
    }

    public void StartGame()
    {
        isGameActive = true;
    }
}
