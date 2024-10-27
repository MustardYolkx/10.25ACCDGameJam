using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform draggableArea; 
    private RectTransform rectTransform; 
    private Canvas canvas; // Canvas for coordinate conversions
    private Vector2 offset; // Offset between mouse and object position for smooth dragging
    private bool isDragging;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        draggableArea = transform.parent.GetComponent<RectTransform>();
        if (draggableArea == null)
        {
            Debug.LogError("Parent RectTransform not found! Ensure this element has a parent with a RectTransform.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(draggableArea, eventData.position, eventData.pressEventCamera))
        {
            isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && RectTransformUtility.ScreenPointToLocalPointInRectangle(draggableArea, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos))
        {
            Vector3[] areaCorners = new Vector3[4];
            draggableArea.GetWorldCorners(areaCorners);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, areaCorners[0], eventData.pressEventCamera, out Vector2 areaMin);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, areaCorners[2], eventData.pressEventCamera, out Vector2 areaMax);

            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(localMousePos.x - offset.x, areaMin.x, areaMax.x),
                Mathf.Clamp(localMousePos.y - offset.y, areaMin.y, areaMax.y)
            );

            rectTransform.localPosition = clampedPosition;
        }
    }
}
