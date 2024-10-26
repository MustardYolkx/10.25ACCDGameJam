using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/ExpandButton/DoubleClickButton")]
public class DoubleClickButton : Button
{
    [Serializable]

    public class DoubleClickedEvent : UnityEvent { }

    [SerializeField]
    private DoubleClickedEvent onDoubleClickEvent = new DoubleClickedEvent();

    public DoubleClickedEvent OnDoubleClick
    {
        get { return onDoubleClickEvent; }
        set { onDoubleClickEvent = value; }
    }

    private DateTime firstClickTime;
    private DateTime secondClickTime;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if(firstClickTime.Equals(default(DateTime)))
            firstClickTime = DateTime.Now;
        else
            secondClickTime= DateTime.Now;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (!firstClickTime.Equals(default(DateTime)) && !secondClickTime.Equals(default(DateTime)))
        {
            TimeSpan intervalTime = secondClickTime- firstClickTime;

            if (intervalTime.TotalMilliseconds < 400)
                DoDoubleClick();
            else
                ResetTime();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ResetTime();
    }
    private void ResetTime()
    {
        firstClickTime= default(DateTime);
        secondClickTime= default(DateTime);
    }

    private void DoDoubleClick()
    {
        if (OnDoubleClick != null)
            OnDoubleClick.Invoke();
        ResetTime();
    }
}
