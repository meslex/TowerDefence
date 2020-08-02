using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertCamera : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // EXAMPLE WITH CAMERA UPSIDEDOWN
    void OnPreCull()
    {
        cam.ResetWorldToCameraMatrix();
        cam.ResetProjectionMatrix();
        cam.projectionMatrix = cam.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }

    [System.Obsolete]
    void OnPreRender()
    {
        GL.SetRevertBackfacing(true);
    }

    [System.Obsolete]
    void OnPostRender()
    {
        GL.SetRevertBackfacing(false);
    }


}
