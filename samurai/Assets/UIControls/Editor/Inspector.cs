using UnityEditor;
using Phenix.Unity.UI;

namespace Phenix.Unity.Editor
{
    [CustomEditor(typeof(NumFont)), CanEditMultipleObjects]
    public class InspectorNumFont : UnityEditor.Editor
    {
        ulong _oriValue;
        
        private void OnEnable()
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
        //int _oriCellCount;
        //float _oriAxisRadius;
        //float _oriAxisOffsetDegree;
        //float _oriCellSpaceDegree;

        private void OnEnable()
        {
            _arcView = target as ArcView;
            //_oriCellCount = _arcView.cells.Count;
            //_oriAxisRadius = serializedObject.FindProperty("_axisRadius").floatValue;
            //_oriAxisOffsetDegree = serializedObject.FindProperty("_axisOffsetDegree").floatValue;
            //_oriCellSpaceDegree = serializedObject.FindProperty("_cellSpaceDegree").floatValue;            
        }
        
        /*bool IsDirty()
        {
            return _oriCellCount != _arcView.cells.Count ||
                _oriAxisRadius != serializedObject.FindProperty("_axisRadius").floatValue ||
                _oriAxisOffsetDegree != serializedObject.FindProperty("_axisOffsetDegree").floatValue ||
                _oriCellSpaceDegree != serializedObject.FindProperty("_cellSpaceDegree").floatValue;
        }*/

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (EditorApplication.isPlaying)
            {
                return;
            }

            //if (IsDirty())
            //{
                _arcView.Reset();
            //}
        }
    }
    
    [CustomEditor(typeof(TabView)), CanEditMultipleObjects]
    public class InspectorTabView : UnityEditor.Editor
    {
        TabView _tabView;        

        private void OnEnable()
        {
            _tabView = target as TabView;            
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                return;
            }

            _tabView.InitTabsOnInspetor();
        }
    }

    [CustomEditor(typeof(GridView)), CanEditMultipleObjects]
    public class InspectorGridView : UnityEditor.Editor
    {
        GridView _gridView;

        private void OnEnable()
        {
            _gridView = target as GridView;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                return;
            }

            _gridView.InitCellsOnInspector();
        }
    }


    [CustomEditor(typeof(SimpleScrollView)), CanEditMultipleObjects]
    public class InspectorSimpleScrollView : UnityEditor.Editor
    {
        SimpleScrollView _simpleScrollView;

        private void OnEnable()
        {
            _simpleScrollView = target as SimpleScrollView;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                return;
            }

            _simpleScrollView.InitCellsOnInspector();
        }
    }
}
