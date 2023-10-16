// 日本語対応
using System;
using UnityEditor.Experimental.GraphView;

namespace NovelDataEditor
{
    public interface INodeGraphViewElement
    {
        Port Input { get; }
        Port Output { get; }
        INodeGraphElemtent Node { get; }
        Action<INodeGraphViewElement> OnNodeSelected { get; set; }
        void ApplyTitle(string str);
    }
}