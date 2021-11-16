using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Credit to the light detection technique to Tobias Filthaus
/// </summary>
public class Agent : MonoBehaviour
{

    public Camera camLightScan;
    public bool isLightActive = false;
    public float updateTime = 0.1f;

    public static float lightValue;

    private const int textureSize = 1;

    private Texture2D textLight;
    private RenderTexture texTemp;
    private Rect rectLight;
    private Color lightPixel;

    // Start is called before the first frame update
    void Start()
    {
        textLight = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);
        texTemp = new RenderTexture(textureSize, textureSize, 24);
        rectLight = new Rect(0f, 0f, textureSize, textureSize);

        StartCoroutine(LightDetectionUpdate(updateTime));
    }

    private IEnumerator LightDetectionUpdate(float updateTime)
    {
        while(true)
        {
            //set target texture of the camera
            camLightScan.targetTexture = texTemp;
            //read into the set target texture
            camLightScan.Render();

            //set the target texture as the active rendered texture
            RenderTexture.active = texTemp;

            //read the current rendered texture
            textLight.ReadPixels(rectLight, 0, 0);

            //reset the active rendered texture
            RenderTexture.active = null;

            //reset target texture of the camera
            camLightScan.targetTexture = null;

            //read the pixel in the middle texture
            lightPixel = textLight.GetPixel(textureSize / 2, textureSize / 2);

            lightValue = (lightPixel.r + lightPixel.g + lightPixel.b) / 3f;

            if(isLightActive)
            {
                Debug.Log("Light value: " + lightValue);
            }

            yield return new WaitForSeconds(updateTime);
        }
    }
}
