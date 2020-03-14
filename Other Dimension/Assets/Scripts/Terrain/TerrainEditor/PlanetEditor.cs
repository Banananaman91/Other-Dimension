using System;
using UnityEditor;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
namespace Terrain.TerrainEditor
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : Editor
    {
        private Planet _planet;
        private Editor _shapeEditor;
        private Editor _colourEditor;

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (check.changed)
                {
                    _planet.GeneratePlanet();
                }
            }

            DrawSettingsEditor(_planet.ShapeSettings, _planet.OnShapeSettingsUpdated, ref _planet.shapeSettingsFoldout, ref _shapeEditor);
            DrawSettingsEditor(_planet.ColourSettings, _planet.OnColourSettingsUpdated, ref _planet.colourSettingsFoldout, ref _colourEditor);
        }

        void DrawSettingsEditor(Object settings, Action onSettingsUpdated, ref bool foldOut, ref Editor editor)
        {
            if (settings != null)
            {
                foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    if (foldOut)
                    {
                        CreateCachedEditor(settings, null, ref editor);
                        editor.OnInspectorGUI();

                        if (check.changed)
                        {
                            if (onSettingsUpdated != null)
                            {
                                onSettingsUpdated();
                            }
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            _planet = (Planet)target;
        }
    }
}
#endif
