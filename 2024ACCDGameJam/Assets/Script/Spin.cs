using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Spin : MonoBehaviour
{
    public string[] targetObjectNames = { "Object1", "Object2", "Object3" }; // Names of UI elements
    public float[] targetRotations = { 90f, 180f, 270f }; // Target rotations for each UI element
    public float rotationSpeed = 100f; // Speed of rotation with mouse

    [Range(0.1f, 5f)]
    public float tolerance = 2f; // Tolerance for winning rotation check

    private RectTransform[] rotatingObjects; // Array of UI elements as RectTransforms
    private RectTransform currentObject; // Currently selected UI element
    private GraphicRaycaster raycaster; // GraphicRaycaster for UI raycasting
    private EventSystem eventSystem; // EventSystem for UI interaction

    void Start()
    {
        // Set up GraphicRaycaster and EventSystem for Canvas interaction
        raycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        // Initialize rotatingObjects by finding children under this parent
        rotatingObjects = new RectTransform[targetObjectNames.Length];
        for (int i = 0; i < targetObjectNames.Length; i++)
        {
            Transform foundTransform = transform.Find(targetObjectNames[i]);
            if (foundTransform != null && foundTransform is RectTransform rectTransform)
            {
                rotatingObjects[i] = rectTransform;
            }
            else
            {
                Debug.LogWarning("UI element named " + targetObjectNames[i] + " not found as a child of " + gameObject.name);
            }
        }
    }

    void Update()
    {
        HandleMouseInput();
        CheckWinCondition();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectUIElementUnderMouse();
        }

        if (Input.GetMouseButton(0) && currentObject != null)
        {
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            Vector3 newRotation = currentObject.localEulerAngles;
            newRotation.z += rotationAmount;
            currentObject.localEulerAngles = newRotation;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentObject = null;
        }
    }

    void SelectUIElementUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            RectTransform clickedObject = result.gameObject.GetComponent<RectTransform>();
            if (clickedObject != null && System.Array.Exists(rotatingObjects, obj => obj == clickedObject))
            {
                currentObject = clickedObject;
                Debug.Log("Selected UI element: " + currentObject.name);
                return;
            }
        }
    }

    void CheckWinCondition()
    {
        for (int i = 0; i < rotatingObjects.Length; i++)
        {
            if (!IsRotationClose(rotatingObjects[i], targetRotations[i]))
            {
                return; // Exit if any UI element is not close to its target rotation
            }
        }
        Win();
    }

    // Called when the player wins
    void Win()
    {
        Debug.Log("You Win!");
        ChangeColorToGreen(); // Change color of UI elements to green
    }

    // Change all rotating UI elements to green
    void ChangeColorToGreen()
    {
        foreach (var obj in rotatingObjects)
        {
            Image img = obj.GetComponent<Image>();
            if (img != null)
            {
                img.color = Color.green;
            }
        }
    }

    bool IsRotationClose(RectTransform obj, float targetRotation)
    {
        float angle = Mathf.Abs(Mathf.DeltaAngle(obj.localEulerAngles.z, targetRotation));
        return angle <= tolerance;
    }
}
