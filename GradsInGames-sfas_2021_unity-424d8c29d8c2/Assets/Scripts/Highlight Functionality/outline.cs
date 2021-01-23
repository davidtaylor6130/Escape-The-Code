using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outline : MonoBehaviour
{
	[Header("Active Object Settings")]
	public Color OutlineColour;
	public Color DimOutlineColour;
	[Range(0.0f, 0.005f)]	public float OutputStrength;

	[Header("Shaders")]
	public Shader DrawAsSolidColor;
	public Shader Outline;
	Material _outlineMaterial;
	Camera TempCam;

	public bool IsActiveComputer = true;


	void Start()
	{
		//- Create Temp Camera and Mat -//
		_outlineMaterial = new Material(Outline);
		TempCam = new GameObject().AddComponent<Camera>();
	}

    private void Update()
    {
		//- Update Its Output Color  -//
		if (IsActiveComputer)
			_outlineMaterial.SetColor("_Color", OutlineColour);
		else if (!IsActiveComputer)
			_outlineMaterial.SetColor("_Color", DimOutlineColour);
		//- Set Its Strength -//
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

