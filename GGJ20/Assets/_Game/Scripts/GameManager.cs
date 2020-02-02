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

    private Transform camControlTransform;

    [Header("General")]
    public RectTransform UIWin;

    [Header("Effects")]
    public GameObject decalBloodPrefab;
    public GameObject particlesConfetti;

    private Circuit circuit;
    private Character character;

    public int level = 0;
    private bool isWin = false;

    public Circuit Circuit
    {
        get
        {
            if (circuit == null)
                circuit = FindObjectOfType<Circuit>();

            return circuit;
        }
    }

    public Character Character
    {
        get
        {
            if (character == null)
                character = FindObjectOfType<Character>();

            return character;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Scene activeScene = SceneManager.GetActiveScene();
        level = activeScene.buildIndex - 1;
    }

    private void Start()
    {
        UIWin.GetComponent<CanvasGroup>().alpha = 0.0f;
        camControlTransform = GameObject.Find("CameraControlPosition").transform;
        SetGameState(GameState.Control, true);
    }

    private void Update()
    {
        CheckInput();
        CheckWinCondition();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("TitleScreen");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Level01");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Level02");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Level03");
        }
    }

    private void CheckWinCondition()
    {
        if (isWin) return;

        switch (level)
        {
            case -1:
                break;
            case 0:
                if (Circuit.robotComponents[0].input > 0.5f)
                {
                    Win();
                }
                break;
        }
    }

    public void SetGameState(GameState state, bool force = false)
    {
        if (currentState == state && !force) return;
        DOTween.Kill(this);
        if (state == GameState.Edit)
        {
            Camera.main.transform.DOMove(Circuit.cam.transform.position, 1.0f).SetId(this);
            Camera.main.transform.DORotate(Circuit.cam.transform.eulerAngles, 1.0f).SetId(this);

            Circuit.panelControl.GetComponent<CanvasGroup>().blocksRaycasts = false;
            Circuit.panelControl.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f).SetId(this);
            Circuit.panelEdit.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Circuit.panelEdit.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f).SetDelay(1.0f).SetId(this);

            Circuit.OpenCover();
        }
        else
        {
            Camera.main.transform.DOMove(camControlTransform.position, 1.0f).SetId(this);
            Camera.main.transform.DORotate(camControlTransform.eulerAngles, 1.0f).SetId(this);

            Circuit.panelEdit.GetComponent<CanvasGroup>().blocksRaycasts = false;
            Circuit.panelEdit.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f).SetId(this);
            Circuit.panelControl.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Circuit.panelControl.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f).SetDelay(1.0f).SetId(this);

            Circuit.CloseCover();

            if (Circuit.pointOfInterest != null && Character != null)
                Character.SetPointOfInterest(Circuit.pointOfInterest);

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
        isWin = true;
        GameObject.Instantiate(particlesConfetti);
        UIWin.GetComponent<CanvasGroup>().DOFade(1.0f, 2.0f).SetDelay(2.0f);
        Invoke("LoadNextLevel", 6);
    }

    private void LoadNextLevel()
    {
        int toLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (toLoad >= SceneManager.sceneCountInBuildSettings) toLoad = 0;
        SceneManager.LoadScene(toLoad);
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
