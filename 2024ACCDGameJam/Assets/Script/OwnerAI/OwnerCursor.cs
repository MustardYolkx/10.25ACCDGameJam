using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerCursor : MonoBehaviour
{
    public OwnerAI ownerAI;
    private Transform cursorTransform;

    public float cursorMoveTime = 2f;

    public Transform targetTransform;

    public Transform idlePos;
    
   public enum CursorState
    {
        None,
        MovetoFile,
        Idle,
    }
    public CursorState cursorState;

    private void Start()
    {
        // grab the cursor's transform
        cursorTransform = transform;
        idlePos = GameRoot.GetInstance().idlePos;
        ownerAI = FindObjectOfType<OwnerAI>();
        ownerAI.cursor = this;
    }

    private void Update()
    {
        if (cursorState == CursorState.MovetoFile)
        {
            MoveCursor(targetTransform);
            CheckCurrentPos();
        }
        else if(cursorState == CursorState.None)
        {

        }
        else
        {
            MoveCursor(idlePos);
        }

        transform.SetAsLastSibling();
    }

    public IEnumerator StayForAWhile()
    {
        cursorState = CursorState.None;
        yield return new WaitForSeconds(1f);
        if(cursorState == CursorState.MovetoFile)
        {

        }
        else
        {
            cursorState= CursorState.Idle;

        }
    }
    // move cursor function
    public void MoveCursor(Transform target)
    {
        // find target position, then interpolate (lerp) between current position and target for set period of time
        transform.position = Vector2.MoveTowards(cursorTransform.position, target.position, cursorMoveTime);
    }

    public void CheckCurrentPos()
    {
        if(Vector2.Distance(transform.position,targetTransform.position) < 0.1f)
        {
            if (ownerAI.isFileOpen)
            {
                ownerAI.ClosePage();
                //cursorState= CursorState.Idle;
            }
            else
            {
                ownerAI.OpenTargetFile();
                
            }
            
        }
    }
}
