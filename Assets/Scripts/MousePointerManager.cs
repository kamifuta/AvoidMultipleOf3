using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Threading;
using Cysharp.Threading.Tasks;

public class MousePointerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CircleCombiner circleCombiner;

    private Camera mainCamera;
    private Vector3 clickedPos;
    private Vector3 MousePos => Input.mousePosition;
    private bool ClickedMouse => Input.GetMouseButtonDown(0);
    private bool ClickingMouse => Input.GetMouseButton(0);
    private bool UppedMouse => Input.GetMouseButtonUp(0);
    private Vector3 clickedObjPos;
    Subject<Vector3> dragVecSubject;

    private const float MinDragDistance = 0.5f;

    private void Start()
    {
        mainCamera = Camera.main;
        circleCombiner.SetCombineCollback(CompleteDragSubject);

        this.ObserveEveryValueChanged(x => x.ClickedMouse)
            .Where(x => x && gameManager.gameState==GameState.Playing)
            .Subscribe(_ =>
            {
                clickedPos = MousePos;

                Ray mouseRay = mainCamera.ScreenPointToRay(MousePos);
                if (circleCombiner.CheckClickedCircle(mouseRay))
                {
                    clickedObjPos = circleCombiner.clickedObj.transform.position;
                }
                else
                {
                    return;
                }

                dragVecSubject = new Subject<Vector3>();
                dragVecSubject
                    .Where(vec => Vector3.Magnitude(vec) >= MinDragDistance)
                    .Subscribe(x =>
                    {
                        Ray dragRay = new Ray(clickedObjPos, x);
                        if (circleCombiner.SetTargetCircle(dragRay)) dragVecSubject.OnCompleted();
                    })
                    .AddTo(this);
            })
            .AddTo(this);

        this.UpdateAsObservable()
            .Where(_ => ClickingMouse && gameManager.gameState == GameState.Playing)
            .Subscribe(_ =>
            {
                var mouseWorldPos = mainCamera.ScreenToWorldPoint(MousePos);
                mouseWorldPos.z = 0;
                dragVecSubject?.OnNext(mouseWorldPos - clickedObjPos);
            })
            .AddTo(this);

        this.ObserveEveryValueChanged(x => x.UppedMouse)
            .Where(x => x && gameManager.gameState == GameState.Playing)
            .Subscribe(_ =>
            {
                if (circleCombiner.clickedObj != null)
                {
                    circleCombiner.clickedObj.layer = (int)LayerInfo.circle;
                }
                dragVecSubject?.OnCompleted();
            })
            .AddTo(this);
    }

    private void CompleteDragSubject()
    {
        dragVecSubject?.OnCompleted();
    }
}
