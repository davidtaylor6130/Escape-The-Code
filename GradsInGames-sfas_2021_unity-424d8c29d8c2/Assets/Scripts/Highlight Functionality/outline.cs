using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outline : MonoBehaviour
{
	[Header("Settings")]
	public Color OutlineColour;
	[Range(0.0f, 0.005f)]	public float OutputStrength;

	[Header("Shaders")]
	public Shader DrawAsSolidColor;
	public Shader Outline;
	Material _outlineMaterial;
	Camera TempCam;

	void Start()
	{
		_outlineMaterial = new Material(Outline);
		TempCam = new GameObject().AddComponent<Camera>();

		_outlineMaterial.SetColor("_Color", OutlineColour);
	}

    private void Update()
    {
		_outlineMaterial.SetFloat("_Strength", OutputStrength);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		TempCam.CopyFrom(Camera.current);
		TempCam.backgroundColor = Color.black;
		TempCam.clearFlags = CameraClearFlags.Color;

		TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

		var rt = RenderTexture.GetTemporary(src.width, src.height, 0, RenderTextureFormat.R8);
		TempCam.targetTexture = rt;

		TempCam.RenderWithShader(DrawAsSolidColor, "");

		_outlineMaterial.SetTexture("_SceneTex", src);
		Graphics.Blit(rt, dst, _outlineMaterial);

		RenderTexture.ReleaseTemporary(rt);
	}
}

