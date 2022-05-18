using UnityEngine;
public class LookAtCamera : MonoBehaviour
{
    public Camera cameraToLookAt1;

    private void LateUpdate()
    {
        //transform.forward = new Vector3(cameraToLookAt1.transform.forward.x, transform.forward.y, cameraToLookAt1.transform.forward.z);
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }
}
