Shader "Custom/customShader"
{
   Properties
   {
     _HeightMin ("Height Min", Float) = -1
     _HeightMax ("Height Max", Float) = 1
     _ColorTop ("Top Color", Color) = (0,0,0,1)
     _ColorBottom ("Bottom Color", Color) = (1,1,1,1)
   }
 
   SubShader
   {
     CGPROGRAM
     #pragma surface surf Lambert
 
     fixed4 _ColorTop;
     fixed4 _ColorBottom;
     float _HeightMin;
     float _HeightMax;
 
     struct Input
     {
       float3 worldPos;
     };
 
     void surf (Input IN, inout SurfaceOutput o)
     {
       float h = (_HeightMax-IN.worldPos.z) / (_HeightMax-_HeightMin);
       fixed4 tintColor = lerp(_ColorBottom.rgba, _ColorTop.rgba, h);
 
       o.Albedo = tintColor.rgb;
       o.Alpha = tintColor.a;
     }
     ENDCG
   }
   Fallback "Diffuse"
}
