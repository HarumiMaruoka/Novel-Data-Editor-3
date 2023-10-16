// 日本語対応
using System;
using UnityEngine;

namespace NovelDataEditor
{
    public interface INodeGraphElemtent // 継承したクラスがノードグラフの要素であることを表現する
    {
        INodeGraphElemtent Child { get; set; }
        ViewData ViewData { get; }
    }

    [Serializable]
    public class ViewData
    {
        [SerializeField]
        private string _guid;
        [SerializeField]
        private Vector2 _position;
        [SerializeField]
        private string _title = "title";

        public string GUID { get => _guid ??= UnityEditor.GUID.Generate().ToString(); }
        public Vector2 Position { get => _position; set => _position = value; }
        public string Title { get => _title; set => _title = value; }
    }
}