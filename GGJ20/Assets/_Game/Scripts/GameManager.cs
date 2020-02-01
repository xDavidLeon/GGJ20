using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Control = 0,
        Edit = 1
    };

    public GameState currentState = GameState.Control;

    [Header("Control State")]
    public RectTransform rtControl;
    public Transform camControlTransform;

    [Header("Edit State")]
    public RectTransform rtEdit;

    [Header("General")]
    public RectTransform UIWin;

    public GameObject decalBloodPrefab;
    private Circuit circuit;

    public Circuit Circuit
    {
        get
        {
            if (circuit == null)
                circuit = FindObjectOfType<Circuit>();

            return circuit;
        }
    }

    private void Start()
    {
        UIWin.GetComponent<CanvasGroup>().alpha = 0.0f;
        SetGameState(GameState.Control, true);
    }

    public void SetGameState(GameState state, bool force = false)
    {
        if (currentState == state && !force) return;
        DOTween.Kill(this);
        if (state == GameState.Edit)
        {
            Camera.main.transform.DOMove(Circuit.cam.transform.position, 1.0f);
            Camera.main.transform.DORotate(Circuit.cam.transform.eulerAngles, 1.0f);

            rtControl.GetComponent<CanvasGroup>().blocksRaycasts = false;
            rtControl.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f);
            rtEdit.GetComponent<CanvasGroup>().blocksRaycasts = true;
            rtEdit.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f).SetDelay(1.0f);

            Circuit.OpenCover();
        }
        else
        {
            Camera.main.transform.DOMove(camControlTransform.position, 1.0f);
            Camera.main.transform.DORotate(camControlTransform.eulerAngles, 1.0f);

            rtEdit.GetComponent<CanvasGroup>().blocksRaycasts = false;
            rtEdit.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f);
            rtControl.GetComponent<CanvasGroup>().blocksRaycasts = true;
            rtControl.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f).SetDelay(1.0f);

            Circuit.CloseCover();
        }
        currentState = state;
    }

    public void SetGameStateControl()
    {
        SetGameState(GameState.Control);
    }

    public void SetGameStateEdit()
    {
        SetGameState(GameState.Edit);
    }

    public void Win()
    {
        UIWin.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InstantiateDecal(GameObject prefab, Vector3 position, Vector3 forward, Rigidbody rb)
    {
        GameObject g = GameObject.Instantiate(prefab);
        g.transform.position = position;
        g.transform.forward = forward;
        Vector3 lr = g.transform.localRotation.eulerAngles;
        lr.z = Random.Range(0, 360);
        g.transform.localRotation.SetEulerAngles(lr);
        if (rb != null)
            g.transform.SetParent(rb.transform);
    }

}
