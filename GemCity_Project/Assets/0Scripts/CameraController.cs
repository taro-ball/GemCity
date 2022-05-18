using UnityEngine;

public class CameraController : MonoBehaviour
{

    private bool doMovement = true;
    private Camera myCAM;
    public float panSpeed;
    public float panBorderThickness;
    public Joystick joystick;

    public float scrollSpeed = 5f;
    private float minCam = 30f;
    private float minX = -35;
    private float maxX = -10;
    private float minZ = -110;
    private float maxZ = -80;
    private float maxCam;
    bool IsMouseOverGameWindow
    {
        get
        { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); }
    }

    void Start()
    {
        myCAM = GetComponent<Camera>();
        maxCam = myCAM.orthographicSize;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || GameManager.gameOver)
            doMovement = false;
        if (Input.GetKeyDown("c"))
            doMovement = true;

        if (!doMovement || !IsMouseOverGameWindow)
            return;

        //joystick.Horizontal !=0 || joystick.Vertical !=0 && 
        if (joystick.gameObject.activeSelf)
        {
            transform.position = new Vector3(transform.position.x + joystick.Horizontal, transform.position.y, transform.position.z + joystick.Vertical) ;
        } else
        {
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            {
                transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            }
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minZ, maxZ));
        //Matrix4x4-20-10
        //	z-80-100
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //Vector3 pos = transform.position;

        myCAM.orthographicSize -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        myCAM.orthographicSize = Mathf.Clamp(myCAM.orthographicSize, minCam, maxCam);
        //Debug.Log("myCAM.orthographicSize " + myCAM.orthographicSize);
        //Debug.Log("minCam " + minCam);

        //transform.position = pos;

    }
}
