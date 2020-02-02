using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    public ParticleSystem[] candles;

    public void EnableClandles()
    {
        if (GameManager.Instance.level == 2)
        {
            GameManager.Instance.Win();
        }

        foreach (ParticleSystem p in candles)
        {
            var e = p.emission;
            e.enabled = true;
            p.Play();
        }
    }
}
