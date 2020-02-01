using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ControlInputSlider : ControlInput
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
}
