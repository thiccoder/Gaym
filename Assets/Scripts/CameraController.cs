using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int MouseBorder;
    public float Speed;
    public GameObject ChildCamera;
    public float CtrlStrength = 2;
    public float Distance;
        private int Collides(float BdUpper, float BdLower, float value) 
    { 
        var side = 0;
        if (BdLower > value)
        {
            side = -1;
        }
        else if (BdUpper < value)
        {
            side = 1;
        }
        return side;
    }
    public void Start()
    {
        if (ChildCamera.transform.parent != transform)
        {
            return;
        }
    }
    public void Update()
    {
        var horizax = Input.GetAxis("Horizontal");
        var vertax = Input.GetAxis("Vertical");
        var mousex = Input.mousePosition.x;
        var mousey = Input.mousePosition.y;
        var mousescroll = Input.GetAxis("Mouse ScrollWheel");
        var Speedx = 0.0f;
        var Speedy = 0.0f;
        var speed = Speed;
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) speed *= CtrlStrength;
        if (mousescroll != 0)
        {
            if (!(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)))
            {
                ChildCamera.transform.Rotate(2 * speed * mousescroll, 0, 0);
            }
            else
            {
                Distance -= mousescroll * speed / 10;
            }
        }
        
        ChildCamera.transform.localPosition = new Vector3(0, 0, 0);
        ChildCamera.transform.Translate(new Vector3(0, 0, -Distance),Space.Self);
        if (horizax != 0.0f || vertax != 0.0f)
        {
            Speedx = Time.deltaTime * Mathf.Sign(horizax) * speed * ((horizax == 0) ? 0 : 1);
            Speedy = Time.deltaTime * Mathf.Sign(vertax) * speed * ((vertax == 0) ? 0 : 1);
        }
        else
        {
            var c = Collides((Screen.width - MouseBorder), MouseBorder, mousex);
            if (c == 1)
            {
                Speedx = (speed * Time.deltaTime);
            }
            else if (c == -1)
            {
                Speedx = - (speed * Time.deltaTime);
            }
            c = Collides((Screen.height - MouseBorder), MouseBorder, mousey);
            if (c == 1)
            {
                Speedy = (speed * Time.deltaTime);
            }
            else if (c == -1)
            {
                Speedy = - (speed * Time.deltaTime);
            }

        }
        transform.Translate(new Vector3(Speedx, 0, Speedy));
    }
}

