using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBotton : MonoBehaviour
{
    [field: SerializeField, Range(0, 10)]
    private float delayStart;
    [field: SerializeField]
    private List<BtnAction> btnAction = new List<BtnAction>(0);

    Vector3? startScale, startPosition, startRotation;

    [field: SerializeField]
    private bool isAutoStart = true;
    [field: SerializeField]
    private bool isRepet = true;

    //private Coroutine _coroutine;


    public void AddAction(BtnAction action)
    {
        btnAction.Add(action);
    }
    public void AddAction(List<BtnAction> actions)
    {
        btnAction.AddRange(actions);
    }

    private void OnEnable()
    {
        Debug.Log("Enable Object", this);
        if (!isAutoStart)
            return;

        startPosition ??= transform.localPosition;
        startRotation ??= transform.localEulerAngles;
        startScale ??= transform.localScale;

        if (!Vector3.Equals(transform.localPosition, startPosition))
            this.transform.localPosition = (Vector3)startPosition;

        if (!Vector3.Equals(transform.localRotation, startRotation))
            this.transform.localRotation = Quaternion.Euler((Vector3)startRotation);

        if (!Vector3.Equals(transform.localScale, startScale))
            this.transform.localScale = (Vector3)startScale;

        StartCoroutine(Animat());

        //_coroutine = StartCoroutine(Animat());
    }

    void OnDisable()
    {
        Debug.Log("Disable Object", this);
        if (!isAutoStart)
            return;

        //if (_coroutine != null) 
        //  StopCoroutine(_coroutine);
    }


    IEnumerator Animat()
    {
        bool tmpIsRepit = true;
        int stepAction = 0;
        float currentTime = 0, endTime;
        ActionType currentAction;
        BtnAction myAction;
        Vector3 originalScale;
        Vector3 originalPosition;
        Vector3 endState;
        Quaternion originalRotation;
        Quaternion endStateRotation;
        Action<bool> func;
        yield return new WaitForSeconds(delayStart);

        if (btnAction.Count != 0)
            while (tmpIsRepit)
            {

                for (int i = 0; i < btnAction.Count; i++)
                {

                    originalPosition = transform.localPosition;
                    originalRotation = transform.localRotation;
                    originalScale = transform.localScale;
                    myAction = btnAction[stepAction];
                    endStateRotation = Quaternion.Euler((Vector3)(myAction.endState + startRotation));
                    endTime = myAction.timeAnimation;
                    currentAction = myAction.actionType;
                    endState = myAction.endState;
                    func = null;
                    //Debug.Log($"Start Action : {currentAction}");

                    switch (currentAction)
                    {
                        case ActionType.Position:
                            func = (bool isBack) =>
                            {
                                if (!isBack)
                                    transform.localPosition = Vector2.Lerp(originalPosition, (Vector3)(startPosition + endState), currentTime / endTime);
                                else
                                    transform.localPosition = Vector2.Lerp((Vector3)(startPosition + endState), originalPosition, currentTime / endTime);
                            };
                            break;

                        case ActionType.Rotation:
                            func = (bool isBack) =>
                            {
                                if (!isBack)
                                    transform.localRotation = Quaternion.Lerp(originalRotation, endStateRotation, currentTime / endTime);
                                else
                                    transform.localRotation = Quaternion.Lerp(endStateRotation, originalRotation, currentTime / endTime);
                            };
                            break;

                        case ActionType.Scale:
                            func = (bool isBack) =>
                            {
                                if (!isBack)
                                    transform.localScale = Vector2.Lerp(originalScale, (Vector3)(startScale + myAction.endState), currentTime / endTime);
                                else
                                    transform.localScale = Vector2.Lerp((Vector3)(startScale + myAction.endState), originalScale, currentTime / endTime);
                            };
                            break;

                        default:
                            break;
                    }
                    Transform tmp = transform;
                    if (currentAction != ActionType.none)
                    {
                        currentTime = 0;
                        do
                        {
                            func(false);
                            currentTime += Time.deltaTime;
                            yield return null;
                        } while (currentTime <= endTime);
                        currentTime = 0;
                        if (myAction.isBackStep)
                            do
                            {
                                func(true);
                                currentTime += Time.deltaTime;
                                yield return null;
                            } while (currentTime <= endTime);

                        transform.localPosition = (Vector3)startPosition;
                        transform.localRotation = Quaternion.Euler((Vector3)startRotation);
                        transform.localScale = (Vector3)startScale;
                    }
                    else
                        yield return new WaitForSeconds(endTime * 2);


                    stepAction++;
                    if (stepAction % btnAction.Count == 0)
                        stepAction = 0;
                }

                tmpIsRepit = isRepet;

            }

    }

}

//switch (currentAction)
//{
//    case ActionType.Position:
//        transform.localPosition = Vector2.Lerp(this.transform.position, myAction.endState, currentTime / endTime);
//        break;
//    case ActionType.Rotation:
//        transform.Rotate(myAction.endState * currentTime / endTime);
//        break;
//    case ActionType.Scale:
//        transform.localScale = Vector2.Lerp(this.transform.position, myAction.endState, currentTime / endTime);
//        break;
//    default:
//        break;
//}

[Serializable]
public class BtnAction
{
    public ActionType actionType;
    public float timeAnimation = 1;
    public bool isBackStep = false;
    public Vector3 endState;

    public BtnAction() { }
    public BtnAction(ActionType _actionType, float _timeAnimation, bool _isBackStep, Vector3 _endState)
    {
        timeAnimation = _timeAnimation;
        isBackStep = _isBackStep;
        actionType = _actionType;
        endState = _endState;
    }
}

public enum ActionType
{
    none,
    Position,
    Rotation,
    Scale
}