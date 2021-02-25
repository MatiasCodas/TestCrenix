using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearsManager : MonoBehaviour
{
    public static GearsManager Instance;
    public List<Draggable> snappedGears;
    public List<Draggable> allGears;

    public TMP_Text nuggetText;
    public string[] nuggetMessages;

    private void Awake()
    {
        Instance = this;

        nuggetText.text = nuggetMessages[0];
    }

    public void AddGear(Draggable gear)
    {
        if (snappedGears.Contains(gear)) { return; }
        snappedGears.Add(gear);

        if (snappedGears.Count == 5)
        {
            nuggetText.text = nuggetMessages[1];

            for (int i = 0; i < snappedGears.Count; i++)
            {
                snappedGears[i].RotateGear();
            }
        }
    }

    public void RemoveGear(Draggable gear)
    {
        if(snappedGears.Count == 5)
        {
            nuggetText.text = nuggetMessages[0];

            for(int i =0; i < snappedGears.Count; i++)
            {
                snappedGears[i].StopSpinnig();
            }
        }
        if (snappedGears.Contains(gear)) { snappedGears.Remove(gear); }
    }

    public void ResetGears()
    {
        for(int i = 0; i<allGears.Count; i++)
        {
            allGears[i].ResetPosition();
        }
    }
}
