using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class sSlot
{
    public Vector2 pos;
    public int connected_to;
    public int color;
    public float data = 0;
    public bool is_output;
    public int circuit_output;
    public int circuit_input;

    public sSlot(int x, int y, bool is_output)
    {
        this.pos = new Vector2(x, y);
        this.is_output = is_output;
        connected_to = -1;
        color = 0;
        data = 0;
        circuit_output = -1;
        circuit_input = -1;
    }
};

public class Circuit : MonoBehaviour
{
    public List<RobotComponent> robotComponents;
    public List<ControlInput> controlInputs;

    public GameObject canvasObject = null;
    public Camera cam;
    public RenderTexture rt = null;
    public Texture t = null;

    public MeshRenderer circuitMR = null;
    public Image blueprintImg = null;
    public List<Texture> circuit_textures;
    public List<Sprite> blueprint_textures;

    public Vector2 cursorpos;

    public List<sSlot> slots;

    public Transform pointOfInterest;

    private int levelID = 0;
    public RectTransform panelEdit;
    public RectTransform panelControl;

    Plane plane;

    int slot_being_dragged = -1;

    public GameObject circuitCover;

    [ContextMenu("Gather Robot Components")]
    public void GatherRobotComponents()
    {
        robotComponents = GetComponentsInChildren<RobotComponent>().ToList();
    }

    [ContextMenu("Gather Control Inputs")]
    public void GatherControlInputs()
    {
        controlInputs = GetComponentsInChildren<ControlInput>().ToList();
    }

    void Awake()
    {
        slots = new List<sSlot>();
    }

    private void Start()
    {
        initBoard();
        //GatherRobotComponents();
        GatherControlInputs();
    }

    void initBoard()
    {
        levelID = GameManager.Instance.level;
        circuitMR.material.SetTexture( "_BaseMap", circuit_textures[ levelID ] );
        blueprintImg.sprite = blueprint_textures[ levelID ];

        if( levelID == 0) //Brither ideas
        {
            //circuit inputs
            sSlot W1 = new sSlot(323, 87, true);
            W1.circuit_input = 0;
            slots.Add(W1);

            //circuit outputs 4
            sSlot A = new sSlot(200, 86, false);
            A.circuit_output = 0;
            slots.Add(A);
        }
        if( levelID == 1)
        {
            //circuit inputs
            sSlot W1 = new sSlot(397, 69, true);
            W1.circuit_input = 0;
            slots.Add(W1);

            sSlot W2 = new sSlot(397, 105, true);
            W2.circuit_input = 1;
            slots.Add(W2);

            sSlot W3 = new sSlot(397, 149, true);
            W3.circuit_input = 2;
            slots.Add(W3);

            sSlot W4 = new sSlot(397, 187, true);
            W4.circuit_input = 3;
            slots.Add(W4);

            //circuit outputs 4
            sSlot A = new sSlot(139, 59, false);
            A.circuit_output = 0;
            slots.Add(A);

            sSlot B = new sSlot(139, 103, false);
            B.circuit_output = 1;
            slots.Add(B);

            sSlot C = new sSlot(139, 150, false);
            C.circuit_output = 2;
            slots.Add(C);

            sSlot D = new sSlot(139, 194, false);
            D.circuit_output = 3;
            slots.Add(D);

            //extras 8
            sSlot NotIN = new sSlot(260, 172, false);
            slots.Add(NotIN);

            sSlot NotOUT = new sSlot(192, 172, true);
            slots.Add(NotOUT);

            sSlot MultiIN = new sSlot(311, 83, false);
            slots.Add(MultiIN);

            sSlot MultiOUT1 = new sSlot(218, 78, true);
            slots.Add(MultiOUT1);
            sSlot MultiOUT2 = new sSlot(218, 102, true);
            slots.Add(MultiOUT2);

            ConnectSlots(0,10,3);
            ConnectSlots(11,4,3);
            ConnectSlots(12,8,3);
            ConnectSlots(9,5,3);
        }
        else if( levelID == 2 )
        {
            //circuit inputs
            sSlot W1 = new sSlot(397, 69, true);
            W1.circuit_input = 0;
            slots.Add(W1);

            //circuit outputs 4
            sSlot A = new sSlot(139, 59, false);
            A.circuit_output = 0;
            slots.Add(A);

            ConnectSlots(0,1,3);
        }
    }

    void UpdateBoard()
    {
        ReadCircuitInputs();

        if( levelID == 0 )
        {
        }
        else if( levelID == 1 )
        {
            float f = ReadSlotData(8);
            WriteSlotData(9,-f);

            f = ReadSlotData(10);
            WriteSlotData(11,f);
            WriteSlotData(12,f);
        }
        else if( levelID == 2 )
        {
        }


        WriteCircuitOutputs();
    }

    float ReadSlotData(int slotnum)
    {
        sSlot slot = slots[slotnum];
        if (slot.connected_to == -1)
            return slot.data;
        sSlot target_slot = slots[slot.connected_to];
        slot.data = target_slot.data;
        return target_slot.data;
    }

    void WriteSlotData(int slotnum, float data)
    {
        sSlot slot = slots[slotnum];
        slot.data = data;
        if (slot.connected_to == -1)
            return;
        sSlot target_slot = slots[slot.connected_to];
        target_slot.data = data;
    }

    void ReadCircuitInputs()
    {
        for (int j = 0; j < slots.Count; ++j)
        {
            sSlot slot = slots[j];
            if (slot == null)
            {
                Debug.LogWarning($"Null slot {j}", this);
                continue;
            }
            if (slot.circuit_input == -1) //because a board input slot is a data output slot
                continue;
            if (controlInputs.Count < slot.circuit_input)
                continue;

            slot.data = controlInputs[slot.circuit_input].input;
        }
    }

    void WriteCircuitOutputs()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            sSlot slot = slots[i];
            if (slot.circuit_output == -1) //because a board input slot is a data output slot
                continue;
            if (robotComponents.Count < slot.circuit_output)
                continue;
            ReadSlotData(i); //update slot
            if (robotComponents.Count > slot.circuit_output)
                robotComponents[slot.circuit_output].input = slot.data;
        }
    }

    //not used
    void TransferSlotData()
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < slots.Count; ++j)
            {
                sSlot slot = slots[j];
                if (!slot.is_output || slot.connected_to == -1)
                    continue;

                sSlot target_slot = slots[slot.connected_to];
                target_slot.data = slot.data;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(cursorpos, 0.01);
    }

    void DisconnectSlot(int slotnum)
    {
        sSlot slot = slots[slotnum];
        if (slot.connected_to == -1)
            return;
        sSlot target_slot = slots[slot.connected_to];
        slot.connected_to = -1;
        target_slot.connected_to = -1;
    }

    void ConnectSlots(int origin_slot_num, int target_slot_num, int color = -1)
    {
        sSlot origin_slot = slots[origin_slot_num];
        sSlot target_slot = slots[target_slot_num];
        if (origin_slot.is_output != target_slot.is_output)
        {
            DisconnectSlot(origin_slot_num);
            DisconnectSlot(target_slot_num);
            origin_slot.connected_to = target_slot_num;
            target_slot.connected_to = origin_slot_num;
            if(color == -1)
                target_slot.color = origin_slot.color;
            else
                target_slot.color = origin_slot.color = color;
        }
        else
            Debug.Log("both are inputs");
    }

    void Update()
    {
        if (!cam)
            return;

        UpdateBoard();

        plane = new Plane(canvasObject.transform.forward, canvasObject.transform.position);
        Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        float enter = 0.0f;
        if ( GameManager.Instance.currentState == GameManager.GameState.Edit && plane.Raycast(ray, out enter) )
        {
            Vector2 hitpos = ray.GetPoint(enter);
            cursorpos = canvasObject.transform.InverseTransformPoint(hitpos);
            cursorpos.x = cursorpos.x * 512 + 256;
            cursorpos.y = cursorpos.y * -256 + 128;

            if (Input.GetMouseButtonDown(0))
            {
                //check if slot
                int slotnum = findSlot(cursorpos);
                if (slotnum != -1)
                {
                    sSlot slot = slots[slotnum];
                    if (slot.is_output)
                    {
                        //CONNECT
                        Debug.Log("SLOT!!  " + slotnum);
                        DisconnectSlot(slotnum);
                        slot_being_dragged = slotnum;
                        slots[slotnum].color = (int)Mathf.Floor(Random.Range(0, 3));
                    }
                }
                else
                    Debug.Log("No slot found ");
            }

            if (Input.GetMouseButtonUp(0) && slot_being_dragged != -1)
            {
                Debug.Log("trying to connect... ");
                int slotnum = findSlot(cursorpos);
                if (slotnum != -1)
                {
                    Debug.Log("connecting... ");
                    ConnectSlots(slot_being_dragged, slotnum);
                }
                else
                    Debug.Log("No slot found ");
                slot_being_dragged = -1;
            }
        }

        DrawTexture();
    }

    int findSlot(Vector2 pos)
    {
        float min_dist = 100000000;
        int closer_slot = -1;

        for (int i = 0; i < slots.Count; ++i)
        {
            sSlot slot = slots[i];
            float d = Vector2.Distance(slot.pos, pos);
            if (d > min_dist || d > 20.0f)
                continue;
            min_dist = d;
            closer_slot = i;
        }
        return closer_slot;
    }

    void DrawIcon(int numx, int numy, Vector2 pos, bool center = true)
    {
        //Rect frame = new Rect( (512.0f - numx*50.0f) / 512.0f, numy*50.0f / 512.0f, 50.0f / 512.0f, 50.0f / 512.0f);
        float s = 50.0f / 512.0f;
        float x = (numx * 50.0f) / 512.0f;
        float y = (512.0f - numy * 50.0f) / 512.0f - s;
        Rect frame = new Rect(x, y, s, s);
        Graphics.DrawTexture(new Rect(pos.x - (center ? 25 : 0), pos.y - (center ? 25 : 0), 50, 50), t, frame, 0, 0, 0, 0);
    }

    float sgn(float v) { return v < 0.0f ? -1.0f : 1.0f; }

    void DrawWire(int x1, int y1, int x2, int y2, int color = 0)
    {
        float d, x, y;
        float dx = (x2 - x1);
        float dy = (y2 - y1);
        if (Mathf.Abs(dx) >= Mathf.Abs(dy))
            d = Mathf.Abs(dx);
        else
            d = Mathf.Abs(dy);
        float vx = dx / d;
        float vy = dy / d;
        x = x1 + sgn(x1) * 0.5f;
        y = y1 + sgn(y1) * 0.5f;
        for (int i = 0; i <= d; i++)
        {
            DrawIcon(5 + color, 1, new Vector2(Mathf.Floor(x), Mathf.Floor(y)));
            x = x + vx;
            y = y + vy;
        }
    }

    void DrawTexture()
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, 512, 256, 0);
        GL.Clear(true, true, new Color(1.0f, 1.0f, 1.0f, 0.0f));
        //Graphics.DrawTexture(new Rect(cursorpos.x,cursorpos.y,50,50), t);
        //DrawIcon(0,0,cursorpos);

        DrawIcon(4, 1, new Vector2(512, 256), true);

        //draw markers
        if (Mathf.Round(Time.time * 2) % 2 == 0)
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                sSlot slot = slots[i];
                if (slot.is_output != (slot_being_dragged != -1))
                    DrawIcon(4, slot.is_output ? 1 : 0, slot.pos);
            }
        }

        //draw wires
        for (int i = 0; i < slots.Count; ++i)
        {
            sSlot slot = slots[i];
            if (!slot.is_output || slot.connected_to == -1)
                continue;
            sSlot target_slot = slots[slot.connected_to];
            DrawWire((int)slot.pos.x, (int)slot.pos.y, (int)target_slot.pos.x, (int)target_slot.pos.y, slot.color);
        }

        //draw wire being dragged
        if (slot_being_dragged != -1)
        {
            sSlot slot = slots[slot_being_dragged];
            DrawWire((int)slot.pos.x, (int)slot.pos.y, (int)cursorpos.x, (int)cursorpos.y, slot.color);
        }

        GL.PopMatrix();
        RenderTexture.active = null;
        //Graphics.DrawTexture(rect, m_renderTexture, new Rect(rect.x / Screen.width, rect.y / Screen.height, (rect.x + rect.width) / Screen.width, (rect.y + rect.height) / Screen.height), 0, 0, 0, 0);
    }

    public void OpenCover()
    {
        circuitCover.transform.DOLocalMoveX(-0.3f, 1.0f);
    }

    public void CloseCover()
    {
        circuitCover.transform.DOLocalMoveX(0.0f, 1.0f);
    }

    public void ExitEditMode()
    {
        GameManager.Instance.SetGameStateControl();
    }

    public void EnterEditMode()
    {
        GameManager.Instance.SetGameStateEdit();
    }

    public void Retry()
    {
        GameManager.Instance.Restart();
    }
}
