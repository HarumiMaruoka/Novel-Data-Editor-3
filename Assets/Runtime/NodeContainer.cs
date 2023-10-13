// 日本語対応
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NovelDataEditor
{
    [Serializable]
    public class NodeContainer : INodeGraphElemtent
    {
        [SerializeReference]
        private List<Node> _nodes;
        [SerializeField]
        private ViewData _viewData;

        public IReadOnlyList<Node> Nodes => _nodes;
        public ViewData ViewData => _viewData ??= new ViewData();
        public Node Child => null;

        public Node CreateNode()
        {
            throw new NotImplementedException();
        }

        public Node AddNode(Node node)
        {
            _nodes.Add(node);
            return node;
        }

        public bool RemoveNode(Node node)
        {
            var result = _nodes.Remove(node);
            return result;
        }
    }
}