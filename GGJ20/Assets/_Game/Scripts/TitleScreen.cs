using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public AudioClip slashClip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 15.0f || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("Level01");
    }

    public void PlaySlash()
    {
        GetComponent<AudioSource>().PlayOneShot(slashClip);
    }
}
