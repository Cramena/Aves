using UnityEngine;

[ExecuteInEditMode]
public class QuickGlow : MonoBehaviour
{
    public Material BlurMaterial;
    public Material AddMaterial;
    [Range(0, 10)]
    public int Iterations;
    [Range(0, 4)]
    public int DownRes;
    [Range(0, 10)]
    public float Size;
    [Range(0, 3)]
    public float Intensity;


    [Header("Flash variables")]
    [Range(0, 3)]
    public float initialIntensity = 3;
    [Range(0, 3)]
    public float finalIntensity = 0;
    public float flashSpeed;
    private bool flashActivated;

    public void Activated()
    {
        flashActivated = true;
        Intensity = initialIntensity;
    }

    void Update ()
    {
        if (flashActivated == true && Intensity >= finalIntensity)
        {
            Intensity -= Time.deltaTime * flashSpeed;
            CustomValidate();
        }
        
        if (Intensity <= finalIntensity)
        {
            flashActivated = false;
            this.enabled = false;
        }
    }

    // Unity function called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    void OnValidate()
    {
        if (BlurMaterial != null)
            BlurMaterial.SetFloat("_Size", Size);
        if (AddMaterial != null)
            AddMaterial.SetFloat("_Intensity", Intensity);
    }

    // Simulating the same effects as OnValidate to make the render image in real time
    void CustomValidate()
    {
        if (BlurMaterial != null)
            BlurMaterial.SetFloat("_Size", Size);
        if (AddMaterial != null)
            AddMaterial.SetFloat("_Intensity", Intensity);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        RenderTexture composite = RenderTexture.GetTemporary(src.width, src.height);
        Graphics.Blit(src, composite);

        int width = src.width >> DownRes;
        int height = src.height >> DownRes;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(src, rt);

        for (int i = 0; i < Iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, BlurMaterial);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        AddMaterial.SetTexture("_BlendTex", rt);
        Graphics.Blit(composite, dst, AddMaterial);

        RenderTexture.ReleaseTemporary(rt);
        RenderTexture.ReleaseTemporary(composite);
    }
}