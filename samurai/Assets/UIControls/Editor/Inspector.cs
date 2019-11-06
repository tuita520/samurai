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

            if (EditorApplication.isPlaying)
            {
                return;
            }

            ulong val = (target as NumFont).Value;
            if (_oriValue != val)
            {
                (target as NumFont).Value = val;
                _oriValue = val;
            }
        }
    }

    [CustomEditor(typeof(ArcView)), CanEditMultipleObjects]
    public class InspectorArcView : UnityEditor.Editor
    {
        ArcView _arcView;
        int _oriCellCount;
        float _oriAxisRadius;
        float _oriAxisOffsetDegree;
        float _oriCellSpaceDegree;

        private void Awake()
        {
            _arcView = target as ArcView;
            _oriCellCount = _arcView.cells.Count;
            float _oriAxisRadius = serializedObject.FindProperty("_axisRadius").floatValue;
            float _oriAxisOffsetDegree = serializedObject.FindProperty("_axisOffsetDegree").floatValue;
            float _oriCellSpaceDegree = serializedObject.FindProperty("_cellSpaceDegree").floatValue;            
        }

        bool IsDirty()
        {
            return _oriCellCount != _arcView.cells.Count ||
                _oriAxisRadius != serializedObject.FindProperty("_axisRadius").floatValue ||
                _oriAxisOffsetDegree != serializedObject.FindProperty("_axisOffsetDegree").floatValue ||
                _oriCellSpaceDegree != serializedObject.FindProperty("_cellSpaceDegree").floatValue;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (EditorApplication.isPlaying)
            {
                return;
            }

            if (IsDirty())
            {
                _arcView.Reset();
            }
        }
    }
}
