using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ControlInputToggle : ControlInput/*, IPointerUpHandler*/
{
    Toggle ctrl;

    private void Awake()
    {
        ctrl = GetComponent<Toggle>();
    }

    public void Update()
    {
        input = (ctrl.isOn ? 1.0f : 0.0f);
        //input = slider.value;
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    slider.value = 0.0f;
    //}
}
