// 日本語対応
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        public void UpdateSelection(INodeGraphElemtent node)
        {
            throw new NotImplementedException();
        }
    }
}