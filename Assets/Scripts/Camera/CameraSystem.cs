using UnityEngine;
using UnityEngine.UI;

// Camera system to control planner view
public class CameraSystem : MonoBehaviour
{
    public Transform pivot; // For view rotation
    public Transform cam;
    private Vector3 prevPos;

    // Pan
    private bool isPanning = false;
    private float panSpeed = 5.0f;
    public Toggle panToggle;

    // Rotate
    private bool isRotating = false;
    private float rotateSpeed = 50.0f;
    public Toggle rotateToggle;

    // Zoom
    private float zoomSpeed = 20.0f;
    public Button zoomIn, zoomOut;
    private bool zoomInBtnTriggered = false;
    private bool zoomOutBtnTriggered = false;
    private float zoomCount = 0f;
    
    // Top Down
    public Toggle topDownToggle;

    void Start()
    {
        zoomIn.onClick.AddListener(buttonZoomIn);
        zoomOut.onClick.AddListener(buttonZoomOut);
        topDownToggle.onValueChanged.AddListener(triggerTopDownView);
    }

    void Update()
    {
        // Trigger panning when toggle is triggered or Mouse 0 + Ctrl are pressed
        if (panToggle.isOn || Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetMouseButtonDown(0)) {
                isRotating = false;
                isPanning = true;
                panToggle.isOn = true;
                prevPos = Input.mousePosition;
            }
        } else {isPanning = false;}

        // Trigger view rotation when toggle is triggered or Mouse 0 + Alt are pressed
        if (rotateToggle.isOn || Input.GetKey(KeyCode.LeftAlt)) {
            if (Input.GetMouseButtonDown(0)) {
                isPanning = false;
                isRotating = true;
                rotateToggle.isOn = true;
                prevPos = Input.mousePosition;
            } 
        } else {isRotating = false;}

        // Terminate both panning and rotation when Mouse 0 is released
        if (Input.GetMouseButtonUp(0)) {
            isPanning = false;
            isRotating = false;
        }

        cameraPan();
        cameraRotate();
        cameraZoom();
    }

    // Pan view (Pivot moves along with camera)
    void cameraPan() {
        if (isPanning) {
            Vector3 change = Input.mousePosition - prevPos;
            Vector3 move = new Vector3(-change.x, -change.y, 0) * panSpeed * Time.deltaTime;
            cam.Translate(move);
            pivot.Translate(move);

            prevPos = Input.mousePosition;
        }
    }

    // Rotate view (Camera orbits pivot)
    void cameraRotate() {
        if (isRotating) {
            float horizontal = Input.GetAxis("Mouse X") * 10.0f;
            float vertical = -Input.GetAxis("Mouse Y") * 10.0f;

            if (horizontal != 0 || vertical != 0) {
                transform.RotateAround(pivot.position, Vector3.up, horizontal * rotateSpeed * Time.deltaTime);
                transform.RotateAround(pivot.position, transform.right, vertical * rotateSpeed * Time.deltaTime);
            }

        }
    }

    // Zoom view by scrolling mouse wheel or via touchpad
    // Can also operate through Zoom buttons by clicking on them
    void cameraZoom() {
        float zoom = Input.mouseScrollDelta.y;
        
        // Scroll or touchpad
        if (zoom != 0f && !zoomInBtnTriggered && !zoomOutBtnTriggered) {
            cam.position += cam.forward * zoom * zoomSpeed * Time.deltaTime;
        } 

        // Buttons
        if ((zoomInBtnTriggered || zoomOutBtnTriggered) && zoom == 0f) {
            if (zoomInBtnTriggered) {
                cam.position += cam.forward * zoomSpeed * Time.deltaTime;
            } else if (zoomOutBtnTriggered) {
                cam.position -= cam.forward * zoomSpeed * Time.deltaTime;
            }

            zoomCount += 1.0f;
            if (zoomCount >= 35.0f) {
                zoomCount = 0.0f;
                zoomInBtnTriggered = false;
                zoomOutBtnTriggered = false;
            }
        }
    }
    
    // Triggered when clicking zoom in button
    void buttonZoomIn() {
        zoomInBtnTriggered = true;
    }

    // Triggered when clicking zoom out button
    void buttonZoomOut() {
        zoomOutBtnTriggered = true;
    }

    // Check to move camera to show top down view of planner
    void triggerTopDownView(bool isOn) {
        if (isOn) {
            cam.transform.position = new Vector3(0f, 10f, 0f);
            cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            topDownToggle.isOn = false;
        }
    }

}
