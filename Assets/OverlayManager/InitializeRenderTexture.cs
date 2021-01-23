using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class InitializeRenderTexture : MonoBehaviour
{
    private RenderTexture renderBuffer;
    private Material material;

    // Start is called before the first frame update
    void Awake()
    {
        renderBuffer = new RenderTexture(128, 128, 16, RenderTextureFormat.ARGB32);
        GetComponent<EasyOpenVROverlayForUnity>().renderTexture = renderBuffer;

        SetRenderSize(1200, 1000);
    }

    public void SetRenderSize(int width, int height) {
        renderBuffer.width = width;
        renderBuffer.height = height;

        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        if (material == null)  material = GetComponent<RawImage>().material;
        Graphics.Blit(material.mainTexture, renderBuffer);
    }
}
