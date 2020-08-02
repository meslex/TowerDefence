using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public Transform MuzzlePos { get; set; }

    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        MuzzlePos = transform.GetChild(0).transform;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Hide()
    {
        if(meshRenderer != null)
            meshRenderer.enabled = false;

    }

    public void Show()
    {
        if (meshRenderer != null)
            meshRenderer.enabled = true;
    }

}
