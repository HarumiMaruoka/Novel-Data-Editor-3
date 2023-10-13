// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NovelDataEditor
{
    public class DataTree : MonoBehaviour
    {
        [SerializeReference]
        private INodeGraphElemtent _rootNode;
        [SerializeReference]
        private List<INodeGraphElemtent> _graphElemtents;

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
            _graphElemtents.Remove(elemtent);
        }

        public void ApplyChild(Node parent, Node child)
        {
            parent.Child = child;
        }

        public void ClearChild(Node parent)
        {
            parent.Child = null;
        }
    }
}