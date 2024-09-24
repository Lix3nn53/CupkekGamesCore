using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Codice.Client.Common.TreeGrouper;
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core
{
    [CreateAssetMenu(fileName = "BehaviourTree", menuName = "CupkekGames/BehaviourTree/BehaviourTree")]
    public class BehaviourTree : ScriptableObject
    {
        private BTNodeRuntimeState _state = BTNodeRuntimeState.Running;
        [SerializeField] private List<BTNode> _nodes = new();
        public ReadOnlyCollection<BTNode> Nodes => _nodes.AsReadOnly();
        [SerializeField] private BTNodeRoot _rootNode = null;
        public BTNodeRoot RootNode => _rootNode;

        public void SetRootNode(BTNodeRoot root)
        {
            _rootNode = root;
        }

        public void InitializeEditor()
        {
            if (_rootNode == null)
            {
                _rootNode = CreateNode(typeof(BTNodeRoot)) as BTNodeRoot;
            }
        }

        public void Update()
        {
            if (_state != BTNodeRuntimeState.Running)
            {
                return;
            }

            _state = _rootNode.Update();
        }

        public BTNode CreateNode(Type type)
        {
            if (Application.isPlaying)
            {
                Debug.LogError($"Can't add new node in play mode");
                return null;
            }

            if (!type.IsSubclassOf(typeof(BTNode)))
            {
                Debug.LogError($"Type {type.Name} is not a subclass of BTNode.");
                return null;
            }

            BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
            if (node == null)
            {
                Debug.LogError($"Failed to create an instance of {type.Name}.");
                return null;
            }

            node.name = type.Name;

            _nodes.Add(node);

            EditorUtility.SetDirty(this);
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Guid guid)
        {
            int indexToDelete = -1;
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].Guid.Value == guid)
                {
                    indexToDelete = i;
                }
            }

            if (indexToDelete != -1)
            {
                BTNode nodeToDelete = _nodes[indexToDelete];

                _nodes.RemoveAt(indexToDelete);

                AssetDatabase.RemoveObjectFromAsset(nodeToDelete);
                AssetDatabase.SaveAssets();
            }
        }

        public void AddChild(BTNode parent, BTNode child, int index)
        {
            if (parent is BTNodeDecorator decorator)
            {
                decorator.Child = child;
            }
            else if (parent is BTNodeComposite composite)
            {
                composite.Children[index] = child;
            }
        }

        public void RemoveChild(BTNode parent, BTNode child, int index)
        {
            if (parent is BTNodeDecorator decorator)
            {
                decorator.Child = null;
            }
            else if (parent is BTNodeComposite composite)
            {
                composite.Children[index] = null;
            }
        }

        public List<BTNode> GetChildren(BTNode parent)
        {
            if (parent is BTNodeDecorator decorator)
            {
                if (decorator.Child != null)
                {
                    return new List<BTNode>() { decorator.Child };
                }
            }
            else if (parent is BTNodeComposite composite)
            {
                return composite.Children;
            }

            return new List<BTNode>();
        }

        public void Traverse(BTNode node, Action<BTNode> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);

                var children = GetChildren(node);

                children.ForEach(n =>
                {
                    Traverse(n, visiter);
                });
            }
        }

        public BehaviourTree Clone()
        {
            BehaviourTree clone = Instantiate(this);

            clone.SetRootNode(clone.RootNode.Clone() as BTNodeRoot);

            clone._nodes = new();

            Traverse(clone.RootNode, (n) =>
            {
                clone._nodes.Add(n);
            });

            return clone;
        }
    }
}