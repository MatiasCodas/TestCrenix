using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private float _distanceToSnap = 1f;
    [SerializeField] private List<Transform> _snappablePoints;
    [SerializeField] Vector3 onDragOffset = new Vector3(0, 0, 0);
    private Vector3 startPos;
    public Camera cam;
    private bool dragging;
    private bool snapped;


    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if(!dragging && !snapped)
        {
            transform.position = startPos;
        }
    }

    private void OnMouseDrag()
    {
        dragging = true;
        snapped = false;
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos += onDragOffset;
        pos.z = 0;
        transform.position = pos;
    }

    private void OnMouseUp()
    {
        dragging = false;
        foreach(Transform snap in _snappablePoints)
        {
            if (Vector2.Distance(snap.transform.position, transform.position) < _distanceToSnap)
            {
                transform.position = snap.transform.position;
                snapped = true;
            }
        }
    }
}
