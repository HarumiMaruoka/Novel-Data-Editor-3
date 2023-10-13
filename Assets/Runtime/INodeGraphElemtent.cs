// 日本語対応
using System;
using UnityEngine;

namespace NovelDataEditor
{
    public interface INodeGraphElemtent // 継承したクラスがノードグラフの要素であることを表現する
    {
        Node Child { get; }
        ViewData ViewData { get; }
    }

    [Serializable]
    public class ViewData
    {
        [SerializeField]
        private string _guid;
        [SerializeField]
        private Vector2 _position;

        public string GUID { get => _guid ??= UnityEditor.GUID.Generate().ToString(); }
        public Vector2 Position { get => _position; set => _position = value; }
    }
}