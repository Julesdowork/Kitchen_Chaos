using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string objectName;

    public Transform Prefab { get { return prefab; } }
    public Sprite Sprite { get { return sprite; } }
    public string ObjectName { get { return objectName; } }
}
