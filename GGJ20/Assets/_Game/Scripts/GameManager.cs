using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public GameObject UIWin;

    public GameObject decalBloodPrefab;

    private void Start()
    {
        UIWin.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    public void Win()
    {
        UIWin.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InstantiateDecalBlood(Vector3 position, Vector3 forward, Rigidbody rb)
    {
        GameObject g =GameObject.Instantiate(decalBloodPrefab);
        g.transform.position = position;
        g.transform.forward = forward;
        Vector3 lr = g.transform.localRotation.eulerAngles;
        lr.z = Random.Range(0, 360);
        g.transform.localRotation.SetEulerAngles(lr);
        g.transform.SetParent(rb.transform);
    }

}
