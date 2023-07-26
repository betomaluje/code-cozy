using System.Collections;
using DG.Tweening;
using UnityEngine;

public class VillagerExpressions
{
    private readonly MonoBehaviour _owner;
    private readonly SpriteRenderer _spriteRenderer;

    public VillagerExpressions(MonoBehaviour owner, SpriteRenderer spriteRenderer)
    {
        _owner = owner;
        _spriteRenderer = spriteRenderer;
    }

    public void DoExpression(Sprite sprite, float animationDuration)
    {
        var prevPos = _spriteRenderer.transform.localPosition.y;
        var initPos = new Vector2(0, 0);
        _spriteRenderer.transform.localPosition = initPos;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.DOFade(1, animationDuration);
        _spriteRenderer.transform.DOLocalMoveY(prevPos, animationDuration).OnComplete(() =>
            _owner.StartCoroutine(UndoExpression(animationDuration))
        );
    }

    private IEnumerator UndoExpression(float animationDuration)
    {
        yield return new WaitForSeconds(animationDuration);
        _spriteRenderer.DOFade(0, animationDuration);
    }
}