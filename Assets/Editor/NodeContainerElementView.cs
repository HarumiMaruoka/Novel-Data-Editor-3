// 日本語対応
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class NodeContainerElementView : VisualElement, INodeGraphViewElement
    {
        public NodeContainerElementView(NodeContainer nodeContainer, INodeGraphElemtent node)
        {
            _node = node;
            _nodeContainer = nodeContainer;
            this.viewDataKey = node.ViewData.GUID;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NodeContainerElementView.uxml");
            visualTree.CloneTree(this);

            _input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            _output = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _input.portName = "";
            _output.portName = "";

            _inputContainer = this.Q("input");
            _outputContainer = this.Q("output");

            _inputContainer.Add(_input);
            _outputContainer.Add(_output);

            _elements = this.Q("elements");
            _titleLabel = new Label();
            _elements.Add(_titleLabel);
            _titleLabel.text = node.ViewData.Title;

            _removeButton = this.Q<Button>("remove-button");
            _removeButton.clicked += () => OnRemoveButtonClicked(this);

            // クリックイベントのリスナーを追加
            this.RegisterCallback<ClickEvent>(OnClick);
        }

        private void OnClick(ClickEvent evt)
        {
            _onNodeSelected?.Invoke(this);
        }

        public void ApplyTitle(string str)
        {
            _node.ViewData.Title = str;
            _titleLabel.text = str;
        }

        private Label _titleLabel;
        private INodeGraphElemtent _node;
        private Port _input;
        private Port _output;
        private VisualElement _inputContainer;
        private VisualElement _outputContainer;
        private VisualElement _elements;
        private NodeContainer _nodeContainer;
        private Button _removeButton;
        private Action<INodeGraphViewElement> _onNodeSelected;

        public event Action<NodeContainerElementView> OnRemoveButtonClicked;
        public Port Input => _input;
        public Port Output => _output;
        public Action<INodeGraphViewElement> OnNodeSelected { get => _onNodeSelected; set => _onNodeSelected = value; }
        public NodeContainer NodeContainer => _nodeContainer;
        public INodeGraphElemtent Node => _node;
    }
}