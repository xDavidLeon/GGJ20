using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    public void EnableClandles()
    {
        if (GameManager.Instance.level == 2)
        {
            GameManager.Instance.Win();
        }
    }
}
