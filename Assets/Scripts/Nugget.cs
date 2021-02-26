using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Nugget : MonoBehaviour
{
    [SerializeField] private RectTransform _targetRectTransform;
    public TMP_Text nuggetText;
    private string oldText;
    public string newText;
    public GameObject accent;
    private bool accentActive;

    public void NuggetJump()
    {
        if (accent.activeInHierarchy)
        {
            accentActive = true;
            accent.SetActive(false);
        }

        oldText = nuggetText.text;
        nuggetText.text = newText;

        _targetRectTransform.DOPunchPosition(_targetRectTransform.transform.position, 2f).OnComplete(() =>
        {
            nuggetText.text = oldText;
            if (accentActive) accent.SetActive(true);
            accentActive = false;
        });
    }

    public void Jump()
    {
        _targetRectTransform.DOLocalMoveY(_targetRectTransform.transform.position.y + 30,1f).SetEase(Ease.OutSine).OnComplete(() => {
            _targetRectTransform.DOLocalMoveY(_targetRectTransform.transform.position.y - 10, 1f).SetEase(Ease.OutSine); });
    }
}
