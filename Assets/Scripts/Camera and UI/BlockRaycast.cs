using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BlockRaycast : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action PointerEnter;
    public event Action PointerExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }

    private void OnDisable()
    {
       // StartCoroutine(SetPointExit());
    }

    IEnumerator SetPointExit()
    {
        yield return new WaitForEndOfFrame();
        PointerExit();
    }
}
