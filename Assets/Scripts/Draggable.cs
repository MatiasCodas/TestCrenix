using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private float _distanceToSnap = 1f;
    [SerializeField] private List<SnapPoint> _snappablePoints;
    [SerializeField] private SnapPoint _startSnap;
    [SerializeField] private SnapPoint _actualSnap;
    [SerializeField] private float speedRotation = 10f;
    [SerializeField] private Vector3 startSize = new Vector3(0.6f, 0.6f, 0.6f);
    [SerializeField] private Vector3 bigSize = new Vector3(1f, 1f, 1f);

    private SpriteRenderer spriteRenderer;
    public Camera cam;
    private bool dragging;
    private bool snapped;
    private float direction;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _actualSnap = _startSnap;
        _actualSnap.isEmpty = false;
        snapped = true;
    }


    private void Update()
    {
        //Caso solte a engrenagem longe de qualquer snapPoint
        if(!dragging && !snapped)
        {
            _actualSnap.isEmpty = false;
            snapped = true;

            SnapGear();
        }

        if (GearsManager.Instance.snappedGears.Count != 5) return;
        RotateGear();
    }

    private void OnMouseDrag()
    {
        spriteRenderer.sortingOrder = 1;
        dragging = true;
        snapped = false;
        _actualSnap.isEmpty = true;

        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
        if(_actualSnap.snapType != SnapType.Inventory) { GearsManager.Instance.RemoveGear(this); }
    }

    private void OnMouseUp()
    {
        dragging = false;
        spriteRenderer.sortingOrder = 0;

        foreach (SnapPoint snap in _snappablePoints)
        {
            if (Vector2.Distance(snap.transform.position, transform.position) < _distanceToSnap)
            {
                if (snap.isEmpty)
                {
                    _actualSnap = snap;
                }
                _actualSnap.isEmpty = false;
                snapped = true;

                SnapGear();
            }
        }
    }

    private void SnapGear()
    {
        if (_actualSnap.snapType == SnapType.Inventory)
        {
            GearsManager.Instance.RemoveGear(this);
            transform.localScale = startSize;
        }
        else
        {
            GearsManager.Instance.AddGear(this);
            transform.localScale = bigSize;
        }
        transform.position = _actualSnap.transform.position;

    }

    private void RotateGear()
    {
        if (_actualSnap.snapType == SnapType.Bottom) { direction = 1; }
        else { direction = -1; }

        transform.Rotate(new Vector3(0, 0, speedRotation * direction));
    }

}
