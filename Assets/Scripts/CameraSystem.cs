using System;
using System.Data.Common;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem : MonoBehaviour
{
    public Transform pivot;
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

    void Start()
    {
        zoomIn.onClick.AddListener(buttonZoomIn);
        zoomOut.onClick.AddListener(buttonZoomOut);
    }

    void Update()
    {

        if (panToggle.isOn || Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetMouseButtonDown(0)) {
                isRotating = false;
                isPanning = true;
                panToggle.isOn = true;
                prevPos = Input.mousePosition;
            }
        } else {isPanning = false;}

        if (rotateToggle.isOn || Input.GetKey(KeyCode.LeftAlt)) {
            if (Input.GetMouseButtonDown(0)) {
                isPanning = false;
                isRotating = true;
                rotateToggle.isOn = true;
                prevPos = Input.mousePosition;
            } 
        } else {isRotating = false;}

        if (Input.GetMouseButtonUp(0)) {
            isPanning = false;
            isRotating = false;
        }

        cameraPan();
        cameraRotate();
        cameraZoom();
    }

    void cameraPan() {
        if (isPanning) {
            Vector3 change = Input.mousePosition - prevPos;
            Vector3 move = new Vector3(-change.x, -change.y, 0) * panSpeed * Time.deltaTime;
            cam.Translate(move);
            pivot.Translate(move);

            prevPos = Input.mousePosition;
        }
    }

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

    void cameraZoom() {
        float zoom = Input.mouseScrollDelta.y;
        
        if (zoom != 0f && !zoomInBtnTriggered && !zoomOutBtnTriggered) {
            cam.position += cam.forward * zoom * zoomSpeed * Time.deltaTime;
        } 
        
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

    void buttonZoomIn() {
        zoomInBtnTriggered = true;
    }

    void buttonZoomOut() {
        zoomOutBtnTriggered = true;
    }

}
