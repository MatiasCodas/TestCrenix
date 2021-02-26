using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class Draggable : MonoBehaviour,  IDragHandler, IEndDragHandler
{
    [SerializeField] private float _distanceToSnap = 1f;
    [SerializeField] Vector3 onDragOffset = new Vector3(0, 0, 0);
    [SerializeField] private List<SnapPoint> _snappablePoints;
    [SerializeField] private SnapPoint _startSnap;
    [SerializeField] private SnapPoint _actualSnap;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _rotationDuration = 5f;
    [SerializeField] private Ease _rotationEase = Ease.Linear;
    [SerializeField] private Vector3 startSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 bigSize = new Vector3(1.5f, 1.5f, 1.5f);

    //private SpriteRenderer spriteRenderer;
    public Sprite[] gearSprite;
    public RectTransform rectTransform;
    private Image image;
    public Camera cam;
    private bool dragging;
    private bool snapped;
    public float direction =1;
    private bool spinning;
    private bool resetGear;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
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
    }

    public void OnDrag(PointerEventData eventData)
    {   
        transform.SetAsLastSibling();
        dragging = true;
        snapped = false;
        _actualSnap.isEmpty = true;

        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos += onDragOffset;
        pos.z = 0;
        transform.position = pos;
        if(_actualSnap.snapType != SnapType.Inventory) { GearsManager.Instance.RemoveGear(this); }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        //spriteRenderer.sortingOrder = 0;

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
        rectTransform.DOMove(_actualSnap.transform.position,0.1f);
        if (_actualSnap.snapType == SnapType.Inventory)
        {
            rectTransform.DOScale(startSize, 0.5f);
            image.sprite = gearSprite[0];
            GearsManager.Instance.RemoveGear(this);
        }
        else
        {
            rectTransform.DOScale(bigSize, 0.5f);
            image.sprite = gearSprite[1];
            GearsManager.Instance.AddGear(this);
        }
    }

    public void RotateGear()
    {
        if (_actualSnap.snapType == SnapType.Bottom) { direction = 1; }
        else { direction = -1; }

        rectTransform.DOLocalRotate(new Vector3(0, 0, _rotationSpeed * direction), _rotationDuration, RotateMode.WorldAxisAdd).SetEase(Ease.InQuad).OnComplete(() => 
        {
            rectTransform.DOLocalRotate(new Vector3(0, 0, _rotationSpeed * direction), _rotationDuration/2, RotateMode.WorldAxisAdd).SetEase(_rotationEase).SetLoops(-1);
        });

        spinning = true;
    }

    public void ResetPosition()
    {
        if (_actualSnap == _startSnap) return;
        GearsManager.Instance.RemoveGear(this);

        resetGear = true;
        _actualSnap.isEmpty = true;
        _actualSnap = _startSnap;
        image.sprite = gearSprite[0];
        _actualSnap.isEmpty = false;

        rectTransform.DOMove(_actualSnap.transform.position, 1f).SetEase(Ease.InOutSine).OnComplete(() => { rectTransform.DOScale(startSize, 0.5f); });
        resetGear = false;
    }

    public void StopSpinnig()
    {
        rectTransform.DOKill();
        rectTransform.DOLocalRotate(new Vector3(0, 0,rectTransform.localRotation.z + (180*direction)), 1f,RotateMode.WorldAxisAdd).SetEase(Ease.OutSine);
    }

}
