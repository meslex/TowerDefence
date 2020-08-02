using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    #region Fields
    public Transform cameraTransfrom;

    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;
    public float maxZoom = 350;
    public float minZoom = 10;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;

    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Transform player;
    #endregion


    // Start is called before the first frame update
    void Start()
    {

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransfrom.localPosition;

        GetComponent<MeshRenderer>().enabled = false;


        if (SceneController.Instance != null)
            SceneController.Instance.AfterSceneLoad += MoveCameraToInitialPosition;
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {
        if (SceneController.Instance != null)
            SceneController.Instance.AfterSceneLoad -= MoveCameraToInitialPosition;
    }

    //Move camera to predetermined location after scene load  
    private void MoveCameraToInitialPosition()
    {
        CameraInitialPosition pos = FindObjectOfType<CameraInitialPosition>();
        if (pos != null)
        {
            transform.position = pos.transform.position;
        }
    }

    private void Update()
    {
        HandleMouseInput();
        HandleCameraMovement();
    }

    void HandleMouseInput()
    {
        //Zoom 
        if (Input.mouseScrollDelta.y != 0)
        {
            //Zoom constraint
            if (Input.mouseScrollDelta.y < 0)
            {
                if (newZoom.y >= maxZoom)
                    return;
            }
            else
                if (newZoom.y <= minZoom)
                return;

            newZoom += Input.mouseScrollDelta.y * zoomAmount;

        }

        //Rotate camera
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    private void HandleCameraMovement()
    {
        newPosition = player.position;

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransfrom.localPosition = Vector3.Lerp(cameraTransfrom.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
