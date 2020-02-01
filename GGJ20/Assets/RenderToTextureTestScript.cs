using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sSlot {
    public Vector2 pos;
    public int connected_to;
    public int color;
    public float data = 0;
    public bool is_output;
    public sSlot( int x, int y, bool is_output ) { 
        this.pos = new Vector2(x,y);
        this.is_output = is_output; 
        connected_to = -1;
        color = 0;
        data = 0;
    } 
};

[ExecuteInEditMode]
class RenderToTextureTestScript : MonoBehaviour
{
    public GameObject canvasObject = null;
    public Camera cam;
    public RenderTexture rt = null;
    public Texture t = null;
    
    public Vector2 cursorpos;
    List<sSlot> slots;

    Plane plane;
    
    int slot_being_dragged = -1;
    
    void Awake()
    {
        slots = new List<sSlot>();
        initBoard();
    }
    
    void initBoard()
    {
        sSlot A = new sSlot(139,59, false);
        slots.Add( A );
        
        sSlot B = new sSlot(139, 103, false);
        slots.Add( B );

        sSlot C = new sSlot(139, 150, false);
        slots.Add( C );

        sSlot D = new sSlot(139, 195, false);
        slots.Add( D );
        
        sSlot W1 = new sSlot(397, 69, true);
        slots.Add( W1 );        
        
        sSlot W2 = new sSlot(397, 105, true);
        slots.Add( W2 );        

        sSlot W3 = new sSlot(397, 149, true);
        slots.Add( W3 );        

        sSlot W4 = new sSlot(397, 187, true);
        slots.Add( W4 );        
    }
    
    void UpdateBoard(){
        
        //compute board logic here
    }
    
    void TransferSlotData()
    {
        for(int i = 0; i < 5; ++i)
        {
            for( int j = 0; j < slots.Count; ++j)
            {
                sSlot slot = slots[j];
                if(!slot.is_output || slot.connected_to == -1 )
                    continue;
                
                sSlot target_slot = slots[ slot.connected_to ];
                DrawWire( (int)slot.pos.x, (int)slot.pos.y, (int)target_slot.pos.x, (int)target_slot.pos.y );
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(cursorpos, 0.01);
    }
    
    void DisconnectSlot( int slotnum )
    {
        sSlot slot = slots[ slotnum ];
        if( slot.connected_to == -1)
            return;
        sSlot target_slot = slots[ slot.connected_to ];
        slot.connected_to = -1;
        target_slot.connected_to = -1;
    }
    
    void ConnectSlots( int origin_slot_num, int target_slot_num )
    {
        sSlot origin_slot = slots[origin_slot_num]; 
        sSlot target_slot = slots[target_slot_num];
        if( origin_slot.is_output != target_slot.is_output )
        {
            DisconnectSlot( origin_slot_num );
            DisconnectSlot( target_slot_num );
            origin_slot.connected_to = target_slot_num;
            target_slot.connected_to = origin_slot_num;
            target_slot.color = origin_slot.color;
        }
        else
            Debug.Log("both are inputs" );
    }
    
    void Update()
    {
        if(!cam)
            return;
        
        UpdateBoard();
        
        plane = new Plane(canvasObject.transform.forward, canvasObject.transform.position);
        Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        float enter = 0.0f;
        if( plane.Raycast( ray, out enter ) )
        {
            Vector2 hitpos = ray.GetPoint(enter);
            cursorpos = canvasObject.transform.InverseTransformPoint( hitpos );
            cursorpos.x = cursorpos.x * 512 + 256;
            cursorpos.y = cursorpos.y * -256 + 128;
            
            if (Input.GetMouseButtonDown(0))
            {
                //check if slot
                int slotnum = findSlot( cursorpos );
                if(slotnum != -1)
                {
                    sSlot slot = slots[slotnum];
                    if(slot.is_output)
                    {
                        //CONNECT
                        Debug.Log("SLOT!!  " + slotnum );
                        DisconnectSlot( slotnum );
                        slot_being_dragged = slotnum;
                        slots[slotnum].color = (int)Mathf.Floor(Random.Range(0,3));
                    }
                }
                else
                    Debug.Log("No slot found " );
            }
            
            if (Input.GetMouseButtonUp(0) && slot_being_dragged != -1)
            {
                Debug.Log("trying to connect... " );
                int slotnum = findSlot( cursorpos );
                if(slotnum != -1)
                {
                    Debug.Log("connecting... " );
                    ConnectSlots( slot_being_dragged, slotnum );
                }
                else
                    Debug.Log("No slot found " );
                slot_being_dragged = -1;
            }
        }

        DrawTexture();
    }
    
    int findSlot( Vector2 pos )
    {
        float min_dist = 100000000;
        int closer_slot = -1;
        
        for( int i = 0; i < slots.Count; ++i)
        {
            sSlot slot = slots[i];
            float d = Vector2.Distance(slot.pos, pos);
            if( d > min_dist || d > 20.0f )
                continue;
            min_dist = d;
            closer_slot = i;
        }
        return closer_slot;
    }

    void DrawIcon(int numx, int numy, Vector2 pos, bool center = true){
        //Rect frame = new Rect( (512.0f - numx*50.0f) / 512.0f, numy*50.0f / 512.0f, 50.0f / 512.0f, 50.0f / 512.0f);
        float s = 50.0f / 512.0f;
        float x = (numx*50.0f) / 512.0f;
        float y = (512.0f - numy*50.0f) / 512.0f - s;
        Rect frame = new Rect( x,y, s, s);
        Graphics.DrawTexture( new Rect(pos.x - (center ? 25 : 0),pos.y - (center ? 25 : 0),50,50), t, frame,0,0,0,0 );
    }

    float sgn(float v) { return v < 0.0f ? -1.0f : 1.0f; }
    
    void DrawWire(int x1, int y1, int x2, int y2, int color = 0)
    {
        float d, x, y;
        float dx = (x2-x1);
        float dy = (y2-y1);
        if ( Mathf.Abs(dx) >= Mathf.Abs(dy) )
           d = Mathf.Abs(dx);
        else
           d = Mathf.Abs(dy);
        float vx = dx / d;
        float vy = dy / d;
        x = x1+sgn(x1)*0.5f;
        y = y1+sgn(y1)*0.5f;
        for (int i = 0; i <= d; i++)
        {
            DrawIcon( 5+color,1, new Vector2(Mathf.Floor(x), Mathf.Floor(y)) );
            x = x + vx;
            y = y + vy;
        }
    }

    void DrawTexture()
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, 512,256,0);
        GL.Clear(true, true, new Color(1.0f,1.0f,1.0f,0.0f));
        //Graphics.DrawTexture(new Rect(cursorpos.x,cursorpos.y,50,50), t);
        //DrawIcon(0,0,cursorpos);

        DrawIcon(4,1, new Vector2(512,256),true );

        //draw markers
        if( Mathf.Round(Time.time*2) % 2 == 0 )
        {
            for( int i = 0; i < slots.Count; ++i)
            {
                sSlot slot = slots[i];
                if( slot.is_output != (slot_being_dragged != -1) )
                    DrawIcon(4, slot.is_output ? 1 : 0, slot.pos );
            }
        }
        
        //draw wires
        for( int i = 0; i < slots.Count; ++i)
        {
            sSlot slot = slots[i];
            if(!slot.is_output || slot.connected_to == -1 )
                continue;
            sSlot target_slot = slots[ slot.connected_to ];
            DrawWire( (int)slot.pos.x, (int)slot.pos.y, (int)target_slot.pos.x, (int)target_slot.pos.y );
        }
        
        //draw wire being dragged
        if( slot_being_dragged != -1 )
        {
            sSlot slot = slots[ slot_being_dragged ];
            DrawWire( (int)slot.pos.x, (int)slot.pos.y, (int)cursorpos.x, (int)cursorpos.y, slot.color );
        }
        
        GL.PopMatrix();
        RenderTexture.active = null;
        //Graphics.DrawTexture(rect, m_renderTexture, new Rect(rect.x / Screen.width, rect.y / Screen.height, (rect.x + rect.width) / Screen.width, (rect.y + rect.height) / Screen.height), 0, 0, 0, 0);
    }
}
