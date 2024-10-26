using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerCursor : MonoBehaviour
{
    private Transform cursorTransform;

    public float cursorMoveTime = 0.5f;

    private void Start()
    {
        // grab the cursor's transform
        cursorTransform = transform;
    }
    // move cursor function
    public void MoveCursor(Transform target)
    {
        // find target position, then interpolate (lerp) between current position and target for set period of time
        transform.position = Vector2.Lerp(cursorTransform.position, target.position, cursorMoveTime);
    }
}
