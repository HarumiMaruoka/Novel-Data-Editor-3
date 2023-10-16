// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NovelDataEditor
{
    [Serializable]
    public class NodeContainer : INodeGraphElemtent
    {
        [SerializeReference]
        private List<INodeGraphElemtent> _nodes;
        [SerializeField]
        private ViewData _viewData;

        public IReadOnlyList<INodeGraphElemtent> Nodes => _nodes ??= new List<INodeGraphElemtent>();
        public ViewData ViewData => _viewData ??= new ViewData();
        public INodeGraphElemtent Child { get => null; set { } }

        public Node CreateNode()
        {
            var elemtent = new Node();
            AddNode(elemtent);
            return elemtent;
        }

        public INodeGraphElemtent AddNode(INodeGraphElemtent node)
        {
            _nodes.Add(node);
            return node;
        }

        public bool RemoveNode(INodeGraphElemtent node)
        {
            var result = _nodes.Remove(node);
            return result;
        }
    }
}