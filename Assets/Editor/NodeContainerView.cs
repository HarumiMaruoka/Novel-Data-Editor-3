// 日本語対応
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class NodeContainerView : UnityEditor.Experimental.GraphView.Node, INodeGraphViewElement
    {
        private NodeContainer _nodeContainer = null;
        private NovelDataGraphView _graphView = null;
        private VisualElement _elementsViewContainer = null;

        public Port Input => null;
        public Port Output => null;
        public INodeGraphElemtent Node => _nodeContainer;
        public NodeContainer NodeContainer => _nodeContainer;
        public Action<INodeGraphViewElement> OnNodeSelected { get => null; set { } }

        public NodeContainerView(NovelDataGraphView graphView, NodeContainer nodeContainer) : base("Assets/Editor/NodeContainerView.uxml")
        {
            _nodeContainer = nodeContainer;
            _graphView = graphView;

            this.viewDataKey = nodeContainer.ViewData.GUID;
            this.title = "Node Container";

            style.left = _nodeContainer.ViewData.Position.x;
            style.top = _nodeContainer.ViewData.Position.y;

            _elementsViewContainer = this.Q("elements");

            // Create Elements
            for (int i = 0; i < nodeContainer.Nodes.Count; i++)
            {
                var cellView = CreateNodeView(nodeContainer.Nodes[i]);
                _elementsViewContainer.Add(cellView);
            }

            _graphView.OnCreatedNodeView += CreateEdges;
            // Add Node Button の処理をここ記述する。
            var button = _elementsViewContainer.Q<Button>();
            button.clicked += OnAddElementButtonClicked;
        }

        private void CreateEdges()
        {
            foreach (var node in _nodeContainer.Nodes)
            {
                // 子と親を繋げる。
                if (node == null) continue;
                var child = node.Child;
                if (child == null) continue; // 子がいなければ何もしない。

                INodeGraphViewElement parentView = FindNodeView(node);
                INodeGraphViewElement childView = FindNodeView(child);

                var edge = parentView.Output.ConnectTo(childView.Input);
                _graphView.AddElement(edge);
            }

            _graphView.OnCreatedNodeView -= CreateEdges;
        }

        private INodeGraphViewElement FindNodeView(INodeGraphElemtent n)
        {
            // 深さ2まで探索する。
            foreach (var elem in _graphView.Query<VisualElement>().ToList())
            {
                if (elem.viewDataKey == n.ViewData.GUID && elem is INodeGraphViewElement)
                {
                    return elem as INodeGraphViewElement;
                }

                foreach (var item in elem.Query<VisualElement>().ToList())
                {
                    if (item.viewDataKey == n.ViewData.GUID && item is INodeGraphViewElement)
                    {
                        return item as INodeGraphViewElement;
                    }
                }
            }

            return null;
        }

        private void OnAddElementButtonClicked() // ノード追加ボタンを押された時の処理
        {
            // ノードを生成し、ビューを生成する。
            Node node = _nodeContainer.CreateNode();
            var nodeView = CreateNodeView(node);
            _elementsViewContainer.Add(nodeView);
        }

        public void OnRemoveElementButtonClicked(NodeContainerElementView removeObj) // （このContainerが所有する）ノード削除ボタンを押された時の処理
        {
            // ノードとビューを削除する。
            // エッジがあればエッジを削除する。
            var inputConnections = removeObj.Input.connections;
            var removeEdges = new HashSet<Edge>();
            foreach (var edge in inputConnections)
            {
                removeEdges.Add(edge);
            }
            var outputConnections = removeObj.Output.connections;
            foreach (var edge in outputConnections)
            {
                removeEdges.Add(edge);
            }
            foreach (var edge in removeEdges)
            {
                _graphView.OnDeleteEdge(edge);
                edge.parent.Remove(edge);
            }

            _nodeContainer.RemoveNode(removeObj.Node); // ノードの削除。
            DeleteElementView(removeObj); // ビューの削除。
        }

        private NodeContainerElementView CreateNodeView(INodeGraphElemtent node)
        {
            var nodeView = new NodeContainerElementView(_nodeContainer, node);
            nodeView.OnRemoveButtonClicked += OnRemoveElementButtonClicked;
            nodeView.OnNodeSelected = _graphView.OnNodeSelected;
            return nodeView;
        }

        private void DeleteElementView(NodeContainerElementView nodeView)
        {
            _elementsViewContainer.Remove(nodeView);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _nodeContainer.ViewData.Position = new Vector2(newPos.x, newPos.y);
        }

        public void OnDeleteThisView()
        {
            this.Query<NodeContainerElementView>().ForEach(n =>
            {
                OnRemoveElementButtonClicked(n);
            });
        }

        public void ApplyTitle(string str)
        {
            _nodeContainer.ViewData.Title = str;
        }
    }
}