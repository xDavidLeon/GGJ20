using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    public float liquid = 0.0f;
   public void AddLiquid()
    {
        liquid += 1.0f;

        if (liquid > 100)
        {
            GameManager.Instance.Win();
        }
    }
}
