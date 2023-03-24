using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _headRenderer;
    [SerializeField] private MeshRenderer _bodyRenderer;

    private Material _material;

    void Awake()
    {
        _material = new Material(_headRenderer.material);
        _headRenderer.material = _material;
        _bodyRenderer.material = _material;
    }

    public void SetPlayerColor(Color color)
    {
        _material.color = color;
    }
}
