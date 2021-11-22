using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public CharacterController AC;

    public AnimationCurve littleLight;

    public AnimationCurve someLight;

    public AnimationCurve enoughLight;

    public float moveSpeed = 10;

    public float moveDuration = 1.0f;

    float elaspedTime = 0.0f;
    private float littleLightValue = 0.0f;
    private float someLightValue = 0.0f;
    private float enoughLightValue = 0.0f;
    public enum fuzzyStates { NO_LIGHT, NOT_ENOUGH_LIGHT, SOME_LIGHT, INLUMINATED }

    public fuzzyStates FS = fuzzyStates.NO_LIGHT;

    public LightTracker AT;

    public float checkPointDir;

    public bool isTuringRight = false;

    public bool isMoving = false;
    FuzzyLight fuzzyLight;

    private void Start()
    {
        //set the agent origin point direction, inverted to take the direction if the player turned back
        checkPointDir = AC.transform.localEulerAngles.y - 180f;

        littleLightValue = 0.0f;
        someLightValue = 0.0f;
        enoughLightValue = 0.0f;
    }

    public void Update()
    {
        switch (FS)
        {
            case fuzzyStates.NO_LIGHT:

                if (isMoving)
                {
                    StartCoroutine(LightFinding());
                }

                break;
            case fuzzyStates.NOT_ENOUGH_LIGHT:
                break;
            case fuzzyStates.SOME_LIGHT:
                break;
            case fuzzyStates.INLUMINATED:
                break;
            default:
                break;
        }

        EvalutateLightIntensity();

        Debug.Log("little Light: " + littleLightValue + " some Light: " + someLightValue + " enough light: " + enoughLightValue);
    }

    //private IEnumerator MoveAgent()
    //{
    //    var moveDirection = transform.TransformDirection(Vector3.forward) * moveSpeed;
    //    AC.Move(moveDirection * Time.deltaTime);

    //    if (littleLightValue >= 0.02 && someLightValue >= 0.4)
    //    {
    //        FS = fuzzyStates.SOME_LIGHT;
    //        Debug.Log("Light Found");
    //    }
    //    yield return new WaitForEndOfFrame();
    //}

    private IEnumerator RotateAgent()
    {
        var clampedDir = checkPointDir - 90.0f;
        clampedDir = -clampedDir;

        if (AC.transform.localEulerAngles.y <= clampedDir && !isTuringRight)
        {
            ClampedRotation(checkPointDir);
            if (AC.transform.localEulerAngles.y >= clampedDir && !isTuringRight)
            {
                SwitchDirection(clampedDir, 175, true);
            }
        }
        if (AC.transform.localEulerAngles.y <= clampedDir && isTuringRight)
        {
            ClampedRotation(-checkPointDir);
            if (AC.transform.localEulerAngles.y >= clampedDir && isTuringRight)
            {
                SwitchDirection(clampedDir, 5, false);
            }
        }

        yield return new WaitForEndOfFrame();
    }

    void ClampedRotation(float dir)
    {
        AC.transform.Rotate(0, dir * Time.deltaTime, 0);
    }

    void SwitchDirection(float clampedDir, float offset, bool isRotatingRight)
    {
        AC.transform.localEulerAngles = new Vector3(AC.transform.localEulerAngles.x, clampedDir - offset,
               AC.transform.localEulerAngles.z);
        isTuringRight = isRotatingRight;
    }

    FuzzyLight EvalutateLightIntensity()
    {
        littleLightValue = littleLight.Evaluate(AT.light);
        someLightValue = someLight.Evaluate(AT.light);
        enoughLightValue = enoughLight.Evaluate(AT.light);
        fuzzyLight = new FuzzyLight(littleLightValue, someLightValue, enoughLightValue);
        return fuzzyLight;
    }

    private IEnumerator LightFinding()
    {
        var moveDirection = transform.TransformDirection(Vector3.forward) * moveSpeed;
        AC.Move(moveDirection * Time.deltaTime);
        elaspedTime += Time.deltaTime;
        if (moveDuration < elaspedTime)
        {
            isMoving = false;
            StopAllCoroutines();
        }
        yield return null;
    }
}
