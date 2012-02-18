using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities {
    public class Tree<T> : IEnumerable<Tree<T>> {
        private List<Tree<T>> _childNodes;

        public T Value;
        public List<Tree<T>> Children {
            get {
                return _childNodes;
            }
        }

        public Tree(T value) {
            _childNodes = new List<Tree<T>>();
            Value = value;
        }

        public void AddChild(T childValue) {
            _childNodes.Add(new Tree<T>(childValue));
        }

        public Tree<T> GetChild(int index) {
            return _childNodes[index];
        }

        public Tree<T> GetFirstChild(T value) {
            foreach(var node in _childNodes) {
                if(node.Value.Equals(value)) {
                    return node;
                }
            }

            foreach(var node in _childNodes) {
                var returnedValue = node.GetFirstChild(value);
                if(returnedValue != null) {
                    return returnedValue;
                }
            }

            return null;
        }

        public bool RemoveChild(T childValue) {
            Tree<T> toDelete = null;
            foreach(var tree in _childNodes) {
                if(tree.Value.Equals(childValue)) {
                    toDelete = tree;
                }
            }

            if(toDelete != null) {
                _childNodes.Remove(toDelete);
                return true;
            }

            return false;
        }

        public IEnumerator<Tree<T>> GetEnumerator() {
            foreach(var child in _childNodes) {
                yield return child;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            foreach(var child in _childNodes) {
                yield return child;
            }
        }
    }
}
