using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Fields
    public Transform cameraTransfrom;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
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
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransfrom.localPosition;

        GetComponent<MeshRenderer>().enabled = false;

        
        if(SceneController.Instance != null)
            SceneController.Instance.AfterSceneLoad += MoveCameraToInitialPosition;
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
        if(Input.mouseScrollDelta.y != 0)
        {
            //Zoom constraint
            if(Input.mouseScrollDelta.y < 0)
            {
                if (newZoom.y >= maxZoom)
                    return;
            }
            else
                if (newZoom.y <= minZoom)
                    return;

            newZoom += Input.mouseScrollDelta.y * zoomAmount;

        }

        //Move around level with mouse
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }


        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward * -movementSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right * -movementSpeed;
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        /*if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }*/

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransfrom.localPosition = Vector3.Lerp(cameraTransfrom.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
