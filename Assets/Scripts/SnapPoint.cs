using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public SnapType snapType;
    public RectTransform rectTransform;
    public bool isEmpty;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        isEmpty = true;
    }
}
public enum SnapType { Inventory, Top, Bottom }
