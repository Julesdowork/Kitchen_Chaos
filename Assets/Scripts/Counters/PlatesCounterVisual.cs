using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _platesCounter;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _plateVisualPrefab;

    private List<GameObject> _plateVisualGOs;

    void Awake()
    {
        _plateVisualGOs = new List<GameObject>();
    }

    void Start()
    {
        _platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        _platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(_plateVisualPrefab, _topPoint);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * _plateVisualGOs.Count, 0);

        _plateVisualGOs.Add(plateVisualTransform.gameObject);
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        GameObject plateGO = _plateVisualGOs[_plateVisualGOs.Count - 1];
        _plateVisualGOs.Remove(plateGO);
        Destroy(plateGO);
    }
}
