using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CircleCombiner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public GameObject clickedObj { get; private set; }
    private GameObject targetObj;
    private Circle clickedCircle;
    private Action combineCallback;

    private const int CircleLayermask = 1 << 6;
    private const float DragRayDistance = 0.5f;

    public void SetCombineCollback(Action callback)
    {
        combineCallback = callback;
    }

    /// <summary>
    /// ‰~‚ðƒNƒŠƒbƒN‚µ‚½‚©‚Ç‚¤‚©
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    public bool CheckClickedCircle(Ray ray)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, CircleLayermask);
        if (hit.collider != null)
        {
            clickedObj = hit.collider.gameObject;
            clickedObj.layer = (int)LayerInfo.clickedCircle;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SetTargetCircle(Ray ray, float length)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, DragRayDistance, CircleLayermask);
        if (hit.collider != null)
        {
            targetObj = hit.collider.gameObject;

            var token1 = this.GetCancellationTokenOnDestroy();
            var token2 = targetObj.GetCancellationTokenOnDestroy();
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(token1, token2);
            CheckCombine(targetObj, linkedToken.Token).Forget();

            return true;
        }
        return false;
    }

    private async UniTask CheckCombine(GameObject firstTargetObj, CancellationToken token = default)
    {
        clickedCircle = clickedObj.GetComponent<Circle>();
        Color? clickedColor = clickedCircle?.color;
        var combinable = targetObj.GetComponent<ICombinable>();
        if (combinable != null && combinable.CheckCombinable(clickedColor))
        {
            await UniTask.DelayFrame(30, cancellationToken: token);
            if (firstTargetObj == targetObj)
            {
                var targetCircle = targetObj.GetComponent<Circle>();
                targetCircle.Combine(clickedCircle.num);

                //SEManager.Instance.Play(SEPath.BUBBLE);

                gameManager.CombinedCallback(targetCircle.color);
                combineCallback();
                Destroy(clickedObj);
            }
        }
    }
}
