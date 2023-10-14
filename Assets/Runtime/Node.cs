// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NovelDataEditor
{
    [Serializable]
    public class Node : INodeGraphElemtent
    {
        [SerializeReference]
        private Node _child;
        [SerializeField]
        private Animator[] _animators;
        [SerializeField]
        private ViewData _viewData;

        public Node Child { get => _child; set => _child = value; }
        public IReadOnlyCollection<Animator> Animators => _animators;
        public ViewData ViewData => _viewData ??= new ViewData();
    }
}