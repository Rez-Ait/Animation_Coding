using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AnimationBotton : MonoBehaviour
{
    [field: SerializeField]
    public List<BtnAction> lsBtnAction = new List<BtnAction>();
    [field: SerializeField]
    public bool isRepit = true;


    private RectTransform myRectTransform;
    //private Rect myRect;

    //public bool isStartDelay;
    //[Range(1, 10)]
    //public float startDelay = 1;
    //public bool isDelayForAllStep;
    //[Range(1, 10)]
    //public float delayForAllStep = 1;
    //[field: SerializeField]
    //public bool isRepit = true;

    void Start()
    {
        //myRect.GetComponent<Rect>();
        myRectTransform = GetComponent<RectTransform>();
        StartCoroutine(Animat());

    }

    //float Y = 0;

    IEnumerator Animat()
    {
        int step = 0;
        int maxStep = lsBtnAction.Count;
        while (true)
        {
            yield return new WaitForSeconds(1f);

            BtnAction btnAction = lsBtnAction[step];
            float defaltTime = Time.realtimeSinceStartup + btnAction.timeAnimation;
            switch (btnAction.actionType)
            {
                case ActionType.Rotation:

                    while (true)
                    {
                        Rotation(btnAction.speed, false);


                        yield return new WaitForSeconds(0.01f);

                        if (Time.realtimeSinceStartup >= defaltTime && !btnAction.isBackStep)
                            break;
                        else
                        {
                            defaltTime = Time.realtimeSinceStartup + btnAction.timeAnimation;
                            while (true)
                            {
                                Rotation(btnAction.speed, true);
                                yield return new WaitForSeconds(0.01f);

                                if (Time.realtimeSinceStartup >= defaltTime)
                                {

                                }
                                break;

                            }
                        }
                    }
                    break;
                case ActionType.Scale:

                    while (true)
                    {
                        Scale(btnAction.speed);

                        yield return new WaitForSeconds(0.01f);

                        if (Time.realtimeSinceStartup >= defaltTime)
                            break;
                    }

                    break;
                case ActionType.Position:

                    //while (true)
                    //{
                    //    Scale(btnAction.speed);

                    //    yield return new WaitForSeconds(0.01f);

                    //    if (Time.realtimeSinceStartup >= defaltTime)
                    //        break;
                    //}

                    break;
                default:
                    break;
            }

            if (!isRepit)
                break;

            step++;
            if (step >= maxStep)
                step = 0;

        }

    }

    public void Rotation(float speed, bool isBack)
    {
        if (isBack)
        {

            myRectTransform.transform.Rotate(0, 0, -(speed * Time.deltaTime));
        }
        else
        {
            myRectTransform.transform.Rotate(0, 0, speed * Time.deltaTime);

        }
        //myRectTransform.transform.Rotate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
        //this.transform.rotation = new Quaternion(0, 1, 0, 0);
    }

    //public void Position(ref float step)
    //{
    //    myRectTransform.rect.Set(0, step += Time.deltaTime, 0, 0);
    //    //this.transform.position = new Vector3(0, 0, 1);
    //}

    public void Scale(float speed)
    {
        myRectTransform.localScale += new Vector3(1f, 1f) * speed * Time.deltaTime;
        //this.transform.localScale = new Vector3(1, 1);
    }
}


[Serializable]
public class BtnAction
{
    public ActionType actionType;
    [Range(1, 500)]
    public float speed;
    public float timeAnimation;
    public bool isBackStep = true;
}

//[Serializable]
//public class ButtonActions : ButtonAction
//{


//    [field: SerializeField, Range(0.1f, 50)]
//    private float timeAnimation = 1;

//    public ButtonActions() { }
//    public ButtonActions(AnimationType _animationType, float _speed, bool _isDelay, float _delay, float _timeAnimation, bool _isRepit)
//    {
//        animationType = _animationType;
//        speed = _speed;
//        isDelay = _isDelay;
//        delay = _delay;
//        timeAnimation = _timeAnimation;
//    }
//}

//public class ButtonAction
//{
//    [field: SerializeField]
//    private ActionType actionType = ActionType.Scale;
//    [field: SerializeField, Range(1, 100)]
//    private float speed = 10;
//    [field: SerializeField]
//    private bool isDelay = false;
//    [field: SerializeField, Range(1, 10)]
//    private float delay = 1;
//    [field: SerializeField]
//    private bool isBack = true;
//}


public enum ActionType
{
    Rotation,
    Scale,
    Position
}