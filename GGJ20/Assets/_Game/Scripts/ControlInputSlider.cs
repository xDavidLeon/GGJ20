using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ControlInputSlider : ControlInput, IPointerUpHandler
{
    public AnimationCurve inputCurve;

    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Update()
    {
        input = inputCurve.Evaluate(slider.value);
        //input = slider.value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        slider.value = 0.0f;
    }
}
