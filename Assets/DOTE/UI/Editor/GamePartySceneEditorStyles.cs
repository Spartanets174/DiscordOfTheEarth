using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public static class GamePartySceneEditorStyles
    {
        public const float DefaultSpace = 12;
        public const float SmallSpace = 6;

        public static GUIStyle BoxPaddingStyle { get; private set; }
        public static GUIStyle FirstLevelHeader { get; private set; }
        public static GUIStyle SecondLevelHeader { get; private set; }

        public static void Init()
        {
            GUI.backgroundColor = Color.white;

            FirstLevelHeader = new(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };

            SecondLevelHeader = new(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14
            };

            BoxPaddingStyle = new GUIStyle
            {
                padding = new RectOffset(5, 5, 5, 5)
            };
        }
    }
}