// Lowpoly Shader <https://u3d.as/2A5c>
// Copyright (c) Amazing Assets <https://amazingassets.world>

using UnityEngine;
using UnityEditor;


namespace AmazingAssets.LowpolyShader.Editor
{
    class LowpolyShaderLabelDrawer : MaterialPropertyDrawer
    {
        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
        }
    }
}