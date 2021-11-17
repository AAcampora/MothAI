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

    private float littleLightValue = 0.0f;
    private float someLightValue = 0.0f;
    private float enoughLightValue = 0.0f;

    public enum agentState {STANDBY, MOVING, TURNING};

    public agentState AS = agentState.MOVING;

    public enum fuzzyStates { NO_LIGHT, NOT_ENOUGH_LIGHT, SOME_LIGHT, INLUMINATED}

    public fuzzyStates FS = fuzzyStates.NO_LIGHT;

    public LightTracker AT;

    public void Update()
    {
        switch (AS)
        {
            case agentState.STANDBY:
                break;
            case agentState.MOVING:
                StartCoroutine(MoveAgent());
                break;
            case agentState.TURNING:
                StartCoroutine(RotateAgent());
                break;
            default:
                break;
        }

        //switch (FS)
        //{   
        //    case fuzzyStates.NO_LIGHT:
        //        break;
        //    case fuzzyStates.NOT_ENOUGH_LIGHT:
        //        break;
        //    case fuzzyStates.SOME_LIGHT:
        //        break;
        //    case fuzzyStates.INLUMINATED:
        //        break;
        //    default:
        //        break;
        //}

        littleLightValue = littleLight.Evaluate(AT.light);
        someLightValue = someLight.Evaluate(AT.light);
        enoughLightValue = enoughLight.Evaluate(AT.light);

        Debug.Log("little Light: " + littleLightValue + " some Light: " + someLightValue + " enough light: " + enoughLightValue);
    }

    private IEnumerator MoveAgent()
    {
        AC.Move(new Vector3(10, AC.transform.position.y, transform.position.z) * Time.deltaTime);
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator RotateAgent()
    {
        AC.transform.Rotate(Vector2.up * 1 * (100f * Time.deltaTime));
        yield return new WaitForEndOfFrame();
    }
}
