using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class NovelDataEditorWindow : EditorWindow
    {
        private NovelDataGraphView _graphView;
        private InspectorView _inspectorView;
        private DataTree _focusedDataTree;

        [MenuItem("Window/Novel Data Editor Window")]
        public static void OpenWindow()
        {
            NovelDataEditorWindow wnd = GetWindow<NovelDataEditorWindow>();
            wnd.titleContent = new GUIContent("NovelDataEditorWindow");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NovelDataEditorWindow.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NovelDataEditorWindow.uss");
            root.styleSheets.Add(styleSheet); // Style SheetÇí«â¡Ç∑ÇÈÅB

            _graphView = root.Q<NovelDataGraphView>();
            _inspectorView = root.Q<InspectorView>();

            _graphView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }


        private void OnSelectionChange()
        {
            var selectGameObject = Selection.activeObject as GameObject;

            if (selectGameObject)
            {
                _focusedDataTree = selectGameObject.GetComponent<DataTree>();

                if (_focusedDataTree)
                {
                    _graphView.PopulateView(_focusedDataTree);
                }
            }
        }

        private void OnInspectorUpdate()
        {

        }

        private void OnNodeSelectionChanged(INodeGraphElemtent node)
        {
            _inspectorView.UpdateSelection(node);
        }
    }
}