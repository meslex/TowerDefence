using Spawners;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnsController : Singlenton<EnemySpawnsController>
{
    public event Action StartWave;

    [SerializeField] private EnemySpawner[] spawners = default;
    [SerializeField] private Button startWaveButton = default;

    public EnemySpawner[] Spawners { get { return spawners; } }
    private Text buttonText;
    private Color semiTransparent;


    protected override void Awake()
    {
        buttonText = startWaveButton.GetComponentInChildren<Text>();
        base.Awake();
    }

    private void Start()
    {
        if (spawners.Length == 0)
        {
            Debug.LogError("[EnemySpawnsController] no spawners were registered.");
        }

        if (startWaveButton == null)
        {
            Debug.LogError("[EnemySpawnsController] startWaveButton wasn'y assigned");
        }

        semiTransparent = new Color32(50, 50, 50, 50);

        //buttonText = startWaveButton.GetComponentInChildren<Text>();

        startWaveButton.onClick.AddListener(StartWaveButton_clicked);
        //startWaveButton.clicked += StartWaveButton_clicked;
    }

    private void OnDisable()
    {
        startWaveButton.onClick.RemoveListener(StartWaveButton_clicked);
       //startWaveButton.clicked -= StartWaveButton_clicked;
    }

    private void StartWaveButton_clicked()
    {
        //startWaveButton.gameObject.SetActive(false);
        StartWave?.Invoke();
    }

    public void ShowButton()
    {
        startWaveButton.interactable = true;
        buttonText.color = Color.black;

        //startWaveButton.gameObject.SetActive(true);
    }

    public void HideButton()
    {
        startWaveButton.interactable = false;
        buttonText.color = semiTransparent;
        //startWaveButton.gameObject.SetActive(false);
    }
}
