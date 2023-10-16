// 日本語対応
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private Editor _editor;

        public void UpdateSelection(DataTree dataTree, INodeGraphViewElement selectedView)
        {
            if (!dataTree || selectedView == null || selectedView.Node == null)
            {
                throw new ArgumentNullException();
            }

            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = Editor.CreateEditor(dataTree);

            SerializedObject serializedObject = _editor.serializedObject;
            SerializedProperty nodesProperty = serializedObject.FindProperty("_graphElemtents");
            SerializedProperty selectNodeProperty = null;

            var nodes = dataTree.GraphElemtents;
            for (int i = 0; i < nodes.Count; i++)
            {
                var view = selectedView as NodeContainerElementView;
                if (view != null) // コンテナを
                {
                    if (view.NodeContainer == nodes[i]) // コンテナの特定。
                    {
                        SerializedProperty containerNodesProperty = nodesProperty.GetArrayElementAtIndex(i);
                        SerializedProperty containerElementsProperty = containerNodesProperty.FindPropertyRelative("_nodes");

                        var containerElements = view.NodeContainer.Nodes;

                        for (int j = 0; j < containerElements.Count; j++) // コンテナの中の要素を特定。
                        {
                            if (selectedView.Node == containerElements[j])
                            {
                                selectNodeProperty = containerElementsProperty.GetArrayElementAtIndex(j);
                                break;
                            }
                        }
                    }
                }

                if (selectedView.Node == nodes[i]) // 通常ノードのとき。
                {
                    selectNodeProperty = nodesProperty.GetArrayElementAtIndex(i);
                }
            }

            if (selectNodeProperty == null) return;

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                var oldTitle = selectedView.Node.ViewData.Title;
                _editor.serializedObject.Update();
                EditorGUILayout.PropertyField(selectNodeProperty, true);
                _editor.serializedObject.ApplyModifiedProperties();
                if (oldTitle != selectedView.Node.ViewData.Title)
                {
                    selectedView.ApplyTitle(selectedView.Node.ViewData.Title);
                }
            });
            Add(container);
        }
    }
}