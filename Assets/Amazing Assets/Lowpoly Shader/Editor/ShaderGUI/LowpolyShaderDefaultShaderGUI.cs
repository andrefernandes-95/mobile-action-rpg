// Lowpoly Shader <https://u3d.as/2A5c>
// Copyright (c) Amazing Assets <https://amazingassets.world>
 
using UnityEngine;
using UnityEditor;


namespace AmazingAssets.LowpolyShader.Editor
{
    public class LowpolyShaderDefaultShaderGUI : ShaderGUI
    {
        static MaterialProperty _CurvedWorldBendSettings = null;


        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            _CurvedWorldBendSettings = FindProperty("_CurvedWorldBendSettings", properties, false);
            if(_CurvedWorldBendSettings != null)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Curved World", EditorStyles.boldLabel);
                    materialEditor.ShaderProperty(_CurvedWorldBendSettings, string.Empty);

                    GUILayout.Space(5);
                }
                EditorGUILayout.EndVertical();
            }


            base.OnGUI(materialEditor, properties);
        }
    }
}
