// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NovelDataEditor
{
    public class DataTree : MonoBehaviour
    {
        [SerializeField]
        private string _title="Data Tree";
        [SerializeReference]
        private INodeGraphElemtent _rootNode;
        [SerializeReference]
        private List<INodeGraphElemtent> _graphElemtents;

        public string Title => _title;
        public INodeGraphElemtent RootNode { get => _rootNode; set => _rootNode = value; }
        public List<INodeGraphElemtent> GraphElemtents => _graphElemtents ??= new List<INodeGraphElemtent>();

        public INodeGraphElemtent CreateNode(Type type)
        {
            if (type != typeof(RootNode) && type != typeof(Node) && type != typeof(NodeContainer))
            {
                throw new ArgumentException(nameof(type)); // 非対応の型が渡されたとき。
            }

            var instance = Activator.CreateInstance(type) as INodeGraphElemtent;
            if (instance != null)
            {
                _graphElemtents ??= new List<INodeGraphElemtent>();
                _graphElemtents.Add(instance);
                return instance;
            }
            else
            {
                throw new ArgumentException(nameof(instance)); // INodeGraphElemtentにキャストできなかったとき。
            }
        }

        public void DeleteNode(INodeGraphElemtent elemtent)
        {
            if (elemtent == _rootNode) _rootNode = null;
            _graphElemtents.Remove(elemtent);
        }

        public void ApplyChild(INodeGraphElemtent parent, INodeGraphElemtent child)
        {
            parent.Child = child;
        }

        public void ClearChild(INodeGraphElemtent parent)
        {
            parent.Child = null;
        }
    }
}