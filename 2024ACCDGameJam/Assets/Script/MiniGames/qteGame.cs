using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class qteGame : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform hitZone;
    public float rotationSpeed = 100f;
    private bool isGameActive = false;

    public TextMeshProUGUI completionText; // Reference to the completion text

    public Image targetImage; // Reference to the image in Canvas that we want to replace on win
    public Sprite winSprite;  // The new sprite to set on win

    public event Action OnMiniGameSuccess;

    private float targetAngle = 90f;
    private float tolerance = 5f;
    private float hitZoneAngle1 = 45f;
    private float hitZoneAngle2 = -45f;

    void Start()
    {
        completionText.gameObject.SetActive(false); // Hide the text initially
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
        transform.SetAsLastSibling();
    }

    private void RotatePointer()
    {
        float angle = Mathf.PingPong(Time.time * rotationSpeed, targetAngle * 2) - targetAngle;
        pointer.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void CheckHit()
    {
        float pointerAngle = pointer.localEulerAngles.z;

        if (pointerAngle > 180)
        {
            pointerAngle -= 360;
        }

        bool inHitZone1 = Mathf.Abs(pointerAngle - hitZoneAngle1) <= tolerance;
        bool inHitZone2 = Mathf.Abs(pointerAngle - hitZoneAngle2) <= tolerance;

        if (inHitZone1 || inHitZone2)
        {
            GameSuccess();
        }
        else
        {
            GameFailure();
        }
    }

    private void GameSuccess()
    {
        // Display the victory text
        completionText.gameObject.SetActive(true);
        Debug.Log("win");

        // Trigger the victory event
        OnMiniGameSuccess?.Invoke();

        // Replace the target image with the win sprite if assigned
        if (targetImage != null && winSprite != null)
        {
            targetImage.sprite = winSprite;
        }
    }

    private void GameFailure()
    {
        Debug.Log("Fail");
    }

    public void StartGame()
    {
        isGameActive = true;
        completionText.gameObject.SetActive(false); // Hide the text at the start of each game
    }
}
