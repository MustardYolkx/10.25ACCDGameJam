using UnityEngine;

public class RotationPuzzle2D : MonoBehaviour
{
    public string[] targetObjectNames = { "Object1", "Object2", "Object3" }; // Names of child objects
    public float[] targetRotations = { 90f, 180f, 270f }; // Target rotations for each object
    public float rotationSpeed = 100f; // Speed of rotation with mouse

    [Range(0.1f, 5f)]
    public float tolerance = 2f; // Tolerance for winning rotation check

    private Transform[] rotatingObjects; // Array of rotating objects
    private Transform currentObject; // Currently selected object

    void Start()
    {
        // Initialize the rotating objects array by finding child objects
        rotatingObjects = new Transform[targetObjectNames.Length];
        for (int i = 0; i < targetObjectNames.Length; i++)
        {
            rotatingObjects[i] = transform.Find(targetObjectNames[i]);
            if (rotatingObjects[i] == null)
            {
                Debug.LogWarning("Object named " + targetObjectNames[i] + " not found as a child of " + gameObject.name);
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
            SelectObjectUnderMouse();
        }

        if (Input.GetMouseButton(0) && currentObject != null)
        {
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            currentObject.Rotate(Vector3.forward, rotationAmount);
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentObject = null;
        }
    }

    void SelectObjectUnderMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && System.Array.Exists(rotatingObjects, obj => obj == hit.transform))
        {
            currentObject = hit.transform;
            Debug.Log("Selected object: " + currentObject.name);
        }
    }

    void CheckWinCondition()
    {
        for (int i = 0; i < rotatingObjects.Length; i++)
        {
            if (!IsRotationClose(rotatingObjects[i], targetRotations[i]))
            {
                return; // Exit if any object is not close to its target rotation
            }
        }
        Win();
    }

    // Called when the player wins
    void Win()
    {
        Debug.Log("You Win!");
        ChangeColorToGreen(); // Change color of objects to green
    }

    // Change all rotating objects to green
    void ChangeColorToGreen()
    {
        foreach (var obj in rotatingObjects)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.green;
            }
        }
    }

    bool IsRotationClose(Transform obj, float targetRotation)
    {
        float angle = Mathf.Abs(Mathf.DeltaAngle(obj.eulerAngles.z, targetRotation));
        return angle <= tolerance;
    }
}
