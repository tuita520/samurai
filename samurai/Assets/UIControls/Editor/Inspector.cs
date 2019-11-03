using UnityEditor;
using Phenix.Unity.UI;

namespace Phenix.Unity.Editor
{
    [CustomEditor(typeof(NumFont)), CanEditMultipleObjects]
    public class InspectorNumFont : UnityEditor.Editor
    {
        ulong _oriValue;
        private void Awake()
        {
            _oriValue = (target as NumFont).Value;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ulong val = (target as NumFont).Value;
            if (_oriValue != val)
            {
                (target as NumFont).Value = val;
                _oriValue = val;
            }
        }
    }
}
