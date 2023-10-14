// 日本語対応
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NovelDataEditor
{
    public class NovelDataGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NovelDataGraphView, GraphView.UxmlTraits> { }

        private DataTree _dataTree;

        public event Action OnCreatedNodeView;

        public DataTree DataTree => _dataTree;

        public Action<INodeGraphElemtent> OnNodeSelected { get; set; }

        public NovelDataGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NovelDataEditorWindow.uss");
            this.styleSheets.Add(styleSheet);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<INodeGraphElemtent>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", _ =>
                {
                    var graphElement = CreateNode(type);
                    CreateNodeView(graphElement);
                });
            }
        }

        public void PopulateView(DataTree novelDataController)
        {
            _dataTree = novelDataController;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements); // 古いものを全て破棄。
            graphViewChanged += OnGraphViewChanged;

            if (novelDataController.RootNode == null) // ルートノードが無ければ作成する。
            {
                novelDataController.RootNode = CreateNode(typeof(RootNode));
                EditorUtility.SetDirty(novelDataController);
                AssetDatabase.SaveAssets();
            }

            // Create Node view
            novelDataController.GraphElemtents.ForEach(n => CreateNodeView(n));

            OnCreatedNodeView?.Invoke();

            // Create edges
            novelDataController.GraphElemtents.ForEach(n => CreateEdge(n));
        }

        public INodeGraphElemtent CreateNode(Type type)
        {
            return _dataTree.CreateNode(type);
        }

        private void CreateNodeView(INodeGraphElemtent nodeGraphElemtent)
        {
            var rootNode = nodeGraphElemtent as RootNode;
            if (rootNode != null)
            {
                var nodeView = new NodeView(rootNode);
                this.AddElement(nodeView);
                return;
            }
            var nodeContainer = nodeGraphElemtent as NodeContainer;
            if (nodeContainer != null)
            {
                var nodeContainerView = new NodeContainerView(this, nodeContainer);
                this.AddElement(nodeContainerView);
                return;
            }
            var node = nodeGraphElemtent as Node;
            if (node != null)
            {
                var nodeView = new NodeView(node);
                this.AddElement(nodeView);
                return;
            }
        }

        private void CreateEdge(INodeGraphElemtent parent)
        {
            // 子と親を繋げる。
            var child = parent.Child;
            if (child == null) return; // 子がいなければ何もしない。

            INodeGraphViewElement parentView = FindNodeView(parent);
            INodeGraphViewElement childView = FindNodeView(child);

            var edge = parentView.Output.ConnectTo(childView.Input);
            AddElement(edge);
        }

        private INodeGraphViewElement FindNodeView(INodeGraphElemtent n)
        {
            // 深さ2まで探索する。
            foreach (var elem in this.Query<VisualElement>().ToList())
            {
                if (elem.viewDataKey == n.ViewData.GUID && elem is INodeGraphViewElement)
                {
                    return elem as INodeGraphViewElement;
                }

                foreach (var item in elem.Query<VisualElement>().ToList())
                {
                    if (item.viewDataKey == n.ViewData.GUID && item is INodeGraphViewElement)
                    {
                        return item as INodeGraphViewElement;
                    }
                }
            }

            return null;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                OnRemoveElements(graphViewChange);
            }

            if (graphViewChange.edgesToCreate != null)
            {
                OnCreateEdge(graphViewChange);
            }

            EditorUtility.SetDirty(_dataTree);

            return graphViewChange;
        }

        private void OnCreateEdge(GraphViewChange graphViewChange)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                INodeGraphViewElement parentView = edge.output.GetFirstAncestorOfType<INodeGraphViewElement>();
                INodeGraphViewElement childView = edge.input.GetFirstAncestorOfType<INodeGraphViewElement>();

                _dataTree.ApplyChild(parentView.Node, childView.Node);
            });
        }

        private void OnRemoveElements(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                INodeGraphViewElement nodeView = elem as INodeGraphViewElement;
                if (nodeView != null)
                {
                    _dataTree.DeleteNode(nodeView.Node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    INodeGraphViewElement parentView = edge.output.node as INodeGraphViewElement;

                    _dataTree.ClearChild(parentView.Node);
                }
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }
    }
}