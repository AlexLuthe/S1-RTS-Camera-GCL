using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Camera_Controls {
    public string forward;
    public string backward;
    public string left;
    public string right;
    public string rotateLeft;
    public string rotateRight;

    public Camera_Controls(string f, string b, string l, string r, string rotL, string rotR) {
        forward = f;
        backward = b;
        left = l;
        right = r;
        rotateLeft = rotL;
        rotateRight = rotR;
    }
}

public class CameraController : MonoBehaviour {

    // Camera Controls
    [SerializeField]
    public Camera_Controls _Camera_Controls = new Camera_Controls(/* Set Camera Controls here, or the inspector */ "w", "s", "a", "d", "q", "r");

    public Camera currentCamera;

    [SerializeField]
    float movementSpeed = 20;
    [SerializeField]
    float rotationSpeed = 5;
    [SerializeField]
    float zoomSpeed = 5;
    [SerializeField]
    float currentZoom = 0.5f;
    [SerializeField]
    float minHeight = 10;
    [SerializeField]
    float maxHeight = 80;
    [SerializeField]
    float minAngle = 30;
    [SerializeField]
    float maxAngle = 80;
    [SerializeField]
    float scrollDistance = 5;
    [SerializeField]
    float scrollSpeed = 25;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    MoveCamera();
        RotateCamera();
        ZoomCamera();
	}

    void MoveCamera() {
        if (Input.GetKey(_Camera_Controls.forward))
            currentCamera.transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        else if (Input.GetKey(_Camera_Controls.backward))
            currentCamera.transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        if (Input.GetKey(_Camera_Controls.left))
            currentCamera.transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        else if (Input.GetKey(_Camera_Controls.right))
            currentCamera.transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);

        // read the Horizontal and Vertical axes and move camera accordingly

        if (Input.mousePosition.x < scrollDistance)
            currentCamera.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        else if (Input.mousePosition.x > (Screen.width - scrollDistance))
            currentCamera.transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);

        if (Input.mousePosition.y < scrollDistance)
            currentCamera.transform.Translate(Vector3.forward * -scrollSpeed * Time.deltaTime);
        else if (Input.mousePosition.y > (Screen.height - scrollDistance))
            currentCamera.transform.Translate(Vector3.forward * scrollSpeed * Time.deltaTime);
    }

    void RotateCamera() {
        if (Input.GetKey(_Camera_Controls.rotateLeft))
            currentCamera.transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        else if (Input.GetKey(_Camera_Controls.rotateRight))
            currentCamera.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void ZoomCamera() {
        // Get Scroll Wheel input
        float scrollInput = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        currentZoom = Mathf.Clamp01(currentZoom + scrollInput);

        // Increase camera resolution at top and bottom of range
        float lerpFactor = Mathf.Clamp01(0.5f - Mathf.Cos(currentZoom * Mathf.PI));
        
        // Calculate new height and camera angle
        float newHeight = Mathf.Lerp(minHeight, maxHeight, lerpFactor);
        float newAngle = Mathf.Lerp(minAngle, maxAngle, lerpFactor);
        
        // Set camera angle
        currentCamera.transform.localEulerAngles = new Vector3(newAngle, 0f, 0f);


        // Raycast so that the camera rises with terrain
        RaycastHit hit;

        Ray ray = (currentCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));

        if (Physics.Raycast(ray, out hit))
            newHeight += hit.point.y;

        // Set camera position
        currentCamera.transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    void ScreenShake() {

    }

}
