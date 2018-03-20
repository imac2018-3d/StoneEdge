using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {
	// https://unity3d.com/fr/learn/tutorials/topics/graphics/realtime-global-illumination-daynight-cycle
	public class DayNightCycle : MonoBehaviour {
		public Gradient NightToNoonGradient;

		public float MaxIntensity = 3f;
		public float MinIntensity = 0f;
		public float MinPoint = -0.2f;

		public float MaxAmbient = 1f;
		public float MinAmbient = 0f;
		public float MinAmbientPoint = -0.2f;

		public Gradient NightDayFogColor;
		public AnimationCurve FogDensityCurve;
		public float FogScale = 1f;

		public float DayAtmosphereThickness = 0.4f;
		public float NightAtmosphereThickness = 0.87f;

		public Vector3 DayRotateSpeed;
		public Vector3 NightRotateSpeed;

		float skySpeed = 1;

		Light mainLight;
		Skybox sky;
		Material skyMat;

		public Renderer Lightwall;
		public Renderer Water;
		public Transform Stars;
		public Transform WorldProbe;

		bool lighton = false;

		void Start() {
			mainLight = GetComponent<Light>();
			skyMat = RenderSettings.skybox;
		}

		void Update() {
			float tRange = 1 - MinPoint;
			float dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - MinPoint) / tRange);
			float i = ((MaxIntensity - MinIntensity) * dot) + MinIntensity;

			mainLight.intensity = i;

			tRange = 1 - MinAmbientPoint;
			dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - MinAmbientPoint) / tRange);
			i = ((MaxAmbient - MinAmbient) * dot) + MinAmbient;
			RenderSettings.ambientIntensity = i;

			mainLight.color = NightToNoonGradient.Evaluate(dot);
			RenderSettings.ambientLight = mainLight.color;

			RenderSettings.fogColor = NightDayFogColor.Evaluate(dot);
			RenderSettings.fogDensity = FogDensityCurve.Evaluate(dot) * FogScale;

			i = ((DayAtmosphereThickness - NightAtmosphereThickness) * dot) + NightAtmosphereThickness;
			skyMat.SetFloat ("_AtmosphereThickness", i);

			if (dot > 0) 
				transform.Rotate (DayRotateSpeed * Time.deltaTime * skySpeed);
			else
				transform.Rotate (NightRotateSpeed * Time.deltaTime * skySpeed);

			if (Stars != null) {
				Stars.transform.rotation = transform.rotation;
			}

			if (Lightwall != null) {
				Color final = Color.white * Mathf.LinearToGammaSpace (lighton ? 5 : 0);
				Lightwall.material.SetColor ("_EmissionColor", final);
				DynamicGI.SetEmissive (Lightwall, final);
			}

			if (WorldProbe != null) {
				Vector3 tvec = Camera.main.transform.position;
				WorldProbe.transform.position = tvec;
			}

			if (Water != null) {
				Water.material.mainTextureOffset = new Vector2 (Time.time / 100, 0);
				Water.material.SetTextureOffset ("_DetailAlbedoMap", new Vector2 (0, Time.time / 80));
			}
		}
	}

} // namespace Se