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
        private Label _statusLabel;

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
            root.styleSheets.Add(styleSheet); // Style Sheetを追加する。

            _graphView = root.Q<NovelDataGraphView>();
            _inspectorView = root.Q<InspectorView>();
            _statusLabel = root.Q<Label>("status-label");

            _graphView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }


        private void OnSelectionChange()
        {
            // 
            var selectGameObject = Selection.activeObject as GameObject;

            if (selectGameObject)
            {
                _focusedDataTree = selectGameObject.GetComponent<DataTree>();

                if (_focusedDataTree)
                {
                    _graphView.PopulateView(_focusedDataTree);
                    return;
                }
            }

            // DataTreeを持たないオブジェクトを選択したとき。
            _focusedDataTree = null;
            _graphView.ClearView();
        }

        private void OnInspectorUpdate()
        {
            if (_focusedDataTree != null)
            {
                _statusLabel.text = _focusedDataTree.Title;
            }
            else
            {
                _statusLabel.text = "Data Tree is not selected.";
            }
        }

        private void OnNodeSelectionChanged(INodeGraphViewElement node)
        {
            _inspectorView.UpdateSelection(_focusedDataTree, node);
        }
    }
}