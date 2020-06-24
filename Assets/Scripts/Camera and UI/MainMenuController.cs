using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private string levelToLoad = default;

    public void LoadLevelOne()
    {
        SceneController.Instance.FadeAndLoadScene(levelToLoad);
    }

}
