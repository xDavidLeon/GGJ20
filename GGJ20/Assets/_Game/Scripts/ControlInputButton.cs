using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ControlInputButton : ControlInput, IPointerUpHandler
{
    Toggle ctrl;

    private void Awake()
    {
        ctrl = GetComponent<Toggle>();
    }

    public void Update()
    {
        input = (ctrl.isOn ? 1.0f : 0.0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ctrl.isOn = false;
    }
}
