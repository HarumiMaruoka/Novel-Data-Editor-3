// 日本語対応
using System;
using UnityEditor.Experimental.GraphView;

namespace NovelDataEditor
{
    public interface INodeGraphView
    {
        Port Input { get; }
        Port Output { get; }
        Node Node { get; }
        Action<INodeGraphElemtent> OnNodeSelected { get; set; }
    }
}