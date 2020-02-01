using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
class RenderToTextureTestScript : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt = null;
    public Texture t = null;

    void Awake()
    {

    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(200, 200, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Input.GetMouseButtonDown(0))
        {
        }


        DrawTexture();


    }

    void DrawTexture()
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, 512,256,0);
        GL.Clear(true, true, new Color(1.0f,1.0f,1.0f,0.0f));
        Graphics.DrawTexture(new Rect(0,0,100,100), t);
        RenderTexture.active = null;
        GL.PopMatrix();
        //Graphics.DrawTexture(rect, m_renderTexture, new Rect(rect.x / Screen.width, rect.y / Screen.height, (rect.x + rect.width) / Screen.width, (rect.y + rect.height) / Screen.height), 0, 0, 0, 0);
    }
}

