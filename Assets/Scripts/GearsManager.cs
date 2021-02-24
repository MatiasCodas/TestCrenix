using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearsManager : MonoBehaviour
{
    public static GearsManager Instance;
    public List<Draggable> snappedGears;

    private void Awake()
    {
        Instance = this;
    }

    public void AddGear(Draggable gear)
    {
        if (snappedGears.Contains(gear)) { return; }
        snappedGears.Add(gear);
    }

    public void RemoveGear(Draggable gear)
    {
        if (snappedGears.Contains(gear)) { snappedGears.Remove(gear); }
    }
}
