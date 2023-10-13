// 日本語対応
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class NodeContainerView : UnityEditor.Experimental.GraphView.Node
    {
        private NodeContainer _nodeContainer = null;
        private NovelDataGraphView _graphView = null;
        private VisualElement _elementsViewContainer = null;

        public NodeContainerView(NovelDataGraphView graphView, NodeContainer nodeContainer) : base("Assets/Editor/NodeContainerView.uxml")
        {
            _nodeContainer = nodeContainer;
            _graphView = graphView;

            this.viewDataKey = nodeContainer.ViewData.GUID;
            this.title = "Node Container";

            style.left = _nodeContainer.ViewData.Position.x;
            style.top = _nodeContainer.ViewData.Position.y;

            _elementsViewContainer = this.Q("elements");

            for (int i = 0; i < nodeContainer.Nodes.Count; i++)
            {
                var cellView = CreateNodeView(nodeContainer.Nodes[i]);
                _elementsViewContainer.Add(cellView);
            }

            // Add Node Button の処理をここ記述する。
            var button = _elementsViewContainer.Q<Button>();
            button.clicked += OnAddElementButtonClicked;
        }

        private void OnAddElementButtonClicked() // ノード追加ボタンを押された時の処理
        {
            // ノードを生成し、ビューを生成する。
            Node node = _graphView.CreateNode(typeof(Node)) as Node;
            if (node == null) throw new ArgumentException(nameof(Node));

            var nodeView = CreateNodeView(node);
            _elementsViewContainer.Add(nodeView);
        }

        public void OnRemoveElementButtonClicked(NodeContainerCellView removeObj) // （このContainerが所有する）ノード削除ボタンを押された時の処理
        {
            // ノードとビューを削除する。
            _nodeContainer.RemoveNode(removeObj.Node); // ノードの削除。
            DeleteNodeView(removeObj); // ビューの削除。
        }

        private NodeContainerCellView CreateNodeView(Node node)
        {
            var nodeView = new NodeContainerCellView(node);
            nodeView.OnRemoveButtonClicked += OnRemoveElementButtonClicked;
            return nodeView;
        }

        private void DeleteNodeView(NodeContainerCellView nodeView)
        {
            _elementsViewContainer.Remove(nodeView);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _nodeContainer.ViewData.Position = new Vector2(newPos.x, newPos.y);
        }
    }
}