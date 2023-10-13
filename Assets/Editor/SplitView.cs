// 日本語対応
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}