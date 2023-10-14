// 日本語対応
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class NodeContainerCellView : VisualElement, INodeGraphViewElement
    {
        public NodeContainerCellView(Node node)
        {
            _node = node;
            this.viewDataKey = node.ViewData.GUID;

            _input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            _output = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _input.portName = "";
            _output.portName = "";

            this.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

            this.Add(_input);
            this.Add(_output);

            // ボタンを取得するか生成する。
            // そのボタンクリック時にOnRemoveButtonClickedを発火する。
            // 引数はthisポインタ。
        }

        private Node _node;
        private Port _input;
        private Port _output;
        private Action<INodeGraphElemtent> _onNodeSelected;

        public Port Input => _input;
        public Port Output => _output;
        public Node Node => _node;
        public Action<INodeGraphElemtent> OnNodeSelected { get => _onNodeSelected; set => _onNodeSelected = value; }
        public event Action<NodeContainerCellView> OnRemoveButtonClicked;
    }
}