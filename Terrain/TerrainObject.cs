using EVEManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Utils;
using static FinePrint.ContractDefs;

namespace Terrain
{
    
    public class TerrainMaterial : MaterialManager
    {
#pragma warning disable 0169
#pragma warning disable 0414
        [ConfigItem] 
        Color _Color = 256 * Color.white;
        [ConfigItem]
        TextureWrapper _MainTex;
		[ConfigItem]
        TextureWrapper _midTex;
		[ConfigItem]
        TextureWrapper _steepTex;
        [ConfigItem]
        TextureWrapper _BumpMap;
        [ConfigItem]
        float _DetailScale = 4000f;
        [ConfigItem]
        float _DetailVertScale = 200f;
        [ConfigItem]
        float _DetailDist = 0.00875f;
        [ConfigItem]
        float _MinLight = 0f;
        [ConfigItem]
        Color _SpecularColor = Color.grey;
        [ConfigItem]
        float _SpecularPower = 51.2f;
        [ConfigItem]
        float _Albedo = .02f;
        [ConfigItem]
        Color _OceanColor = Color.blue;
        [ConfigItem]
        float _OceanDepthFactor = .002f;
    }

    public class OceanMaterial : MaterialManager
    {
        [ConfigItem]
        Color _SurfaceColor = 256 * Color.white;
        [ConfigItem]
        TextureWrapper _DetailTex;
        [ConfigItem]
        float _DetailScale = 4000f;
        [ConfigItem]
        float _DetailDist = 0.00875f;
        [ConfigItem]
        float _MinLight = 0f;
        [ConfigItem]
        float _Clarity = .005f;
        [ConfigItem]
        float _LightPower = 1.75f;
        [ConfigItem]
        float _Reflectivity = .08f;
    }

    public class WaterMaterial : MaterialManager
    {
        [ConfigItem]
        float _UseDisplacement = 0;
        [ConfigItem]
        float _UseMeanSky = 0;
        [ConfigItem]
        float _UseFiltering = 0;
        [ConfigItem]
        float _UseFoam = 0;
        [ConfigItem]
        float _UsePhong = 0;
        [ConfigItem, Tooltip("Range(0, 1)")]
        float _AmbientDensity = 0.15f;
        [ConfigItem, Tooltip("Range(0, 1)")]
        float _DiffuseDensity = 0.1f;
        [ConfigItem]
        Color _SurfaceColor = new Color(0.0078f, 0.5176f, 0.7f, 1);
        [ConfigItem]
        Color _ShoreColor = new Color(0.0078f, 0.5176f, 0.7f, 1);
        [ConfigItem]
        Color _DepthColor = new Color(0.0039f, 0.00196f, 0.145f, 1);
        [ConfigItem, Optional]
        TextureWrapper _SkyTexture;
        [ConfigItem, Optional]
        TextureWrapper _NormalTexture;
        [ConfigItem]
        float _NormalIntensity = 0.5f;
        [ConfigItem]
        float _TextureTiling = 1f;
        [ConfigItem]
        Vector3 _WindDirection = new Vector3(3, 5, 0);
        [ConfigItem, Optional]
        TextureWrapper _HeightTexture;
        [ConfigItem]
        float _HeightIntensity = 0.5f;
        [ConfigItem]
        float _WaveTiling = 1f;
        [ConfigItem]
        float _WaveAmplitudeFactor = 1.0f;
        [ConfigItem]
        float _WaveSteepness = 0.5f;
        [ConfigItem]
        Vector4 _WaveAmplitude = new Vector4(0.05f, 0.1f, 0.2f, 0.3f);
        [ConfigItem]
        Vector4 _WavesIntensity = new Vector4(3, 2, 2, 10);
        [ConfigItem]
        Vector4 _WavesNoise = new Vector4(0.05f, 0.15f, 0.03f, 0.05f);

        [ConfigItem]
        float _WaterClarity = 0.75f;
        [ConfigItem]
        float _WaterTransparency = 10.0f;
        [ConfigItem]
        Vector3 _HorizontalExtinction = new Vector3(3.0f, 10.0f, 12.0f);
        [ConfigItem]
        Vector3 _RefractionValues = new Vector3(0.3f, 0.01f, 1.0f);
        [ConfigItem]
        float _RefractionScale = 0.005f;

        [ConfigItem]
        float _Shininess = 0.5f;
        [ConfigItem]
        Vector3 _SpecularValues = new Vector3(12, 768, 0.15f);
        [ConfigItem]
        float _Distortion = 0.05f;
        [ConfigItem]
        float _RadianceFactor = 1.0f;
        [ConfigItem, Optional]
        TextureWrapper _ReflectionTexture;

        [ConfigItem, Optional]
        TextureWrapper _FoamTexture;
        [ConfigItem, Optional]
        TextureWrapper _ShoreTexture;
        [ConfigItem]
        Vector3 _FoamTiling = new Vector3(2.0f, 0.5f, 0.0f);
        [ConfigItem]
        Vector3 _FoamRanges = new Vector3(2.0f, 3.0f, 100.0f);
        [ConfigItem]
        Vector4 _FoamNoise = new Vector4(0.1f, 0.3f, 0.1f, 0.3f);
        [ConfigItem]
        float _FoamSpeed = 10f;
        [ConfigItem]
        float _FoamIntensity = 0.5f;
        [ConfigItem]
        float _ShoreFade = 0.3f;
    }

    public class TerrainObject : MonoBehaviour, IEVEObject
    {
#pragma warning disable 0649
        public override String ToString() { return body; }
        [ConfigItem, GUIHidden]
        String body;
        [ConfigItem, Optional] 
        TerrainMaterial terrainMaterial = null;
        [ConfigItem, Optional]
        OceanMaterial oceanMaterial = null;
        [ConfigItem, Optional]
        WaterMaterial waterMaterial = null;

        TerrainPQS terrainPQS;

        public void LoadConfigNode(ConfigNode node)
        {
            ConfigHelper.LoadObjectFromConfig(this, node);
        }

        public void Apply()
        {
            GameObject go = new GameObject();
            go.name = "EVE Terrain";
            terrainPQS = go.AddComponent<TerrainPQS>();
            terrainPQS.Apply( body, terrainMaterial, oceanMaterial, waterMaterial);
        }

        public void Remove()
        {
            terrainPQS.Remove();
            GameObject go = terrainPQS.gameObject;
            GameObject.DestroyImmediate(terrainPQS);
            GameObject.DestroyImmediate(go);
        }
    }
}
