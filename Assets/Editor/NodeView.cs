// 日本語対応
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NovelDataEditor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node, INodeGraphViewElement
    {
        public NodeView(Node node)
        {
            _node = node;
            this.viewDataKey = node.ViewData.GUID;

            style.left = _node.ViewData.Position.x;
            style.top = _node.ViewData.Position.y;

            if (node is RootNode) this.title = "Root Node";
            else this.title = _node.ViewData.Title;

            if (node is not RootNode) // Root nodeに Input portは必要ない。
            {
                _input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                _input.portName = "";
                this.inputContainer.Add(_input);
            }

            _output = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _output.portName = "";
            this.outputContainer.Add(_output);
        }

        private Node _node;
        private Port _input;
        private Port _output;
        private Action<INodeGraphViewElement> _onNodeSelected;

        public Port Input => _input;
        public Port Output => _output;
        public Action<INodeGraphViewElement> OnNodeSelected { get => _onNodeSelected; set => _onNodeSelected = value; }
        public INodeGraphElemtent Node => _node;

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _node.ViewData.Position = new Vector2(newPos.x, newPos.y);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            _onNodeSelected?.Invoke(this);
        }

        public void ApplyTitle(string str)
        {
            _node.ViewData.Title = str;
            title = str;
        }
    }
}