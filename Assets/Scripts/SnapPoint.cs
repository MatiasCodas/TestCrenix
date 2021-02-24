using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public SnapType snapType;
    public bool isEmpty;


    private void Awake()
    {
        isEmpty = true;
    }
}
public enum SnapType { Inventory, Top, Bottom }
