using System.Collections.Generic;
using System;
using System.Collections;

namespace BinaryTrees;
public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    public BinaryTree<T> Parent;
    public BinaryTree<T> Left;
    public BinaryTree<T> Right;
    public T Value;
    public int HasLeftChildren;
    public int Count;
    public int Index;

    public BinaryTree()
    {

    }

    public BinaryTree(T key)
    {
        Value = key; 
        Count = 1;
    }

    
    public void Add(T key)
    {
        
        var currentTree = this;
        if (currentTree.Count == 0)
        {
            currentTree.Value = key;
            currentTree.Count++;
            // currentTree.UpdateIndex();
            return;
        }
        var treeToAdd = new BinaryTree<T>(key);
        while (true)
        {

            currentTree.Count++;
            if (key.CompareTo(currentTree.Value) < 0)
            {
                if (currentTree.Left == null)
                {
                    currentTree.Left = treeToAdd;
                    currentTree.HasLeftChildren++;
                    treeToAdd.Parent = currentTree;
                    //currentTree.Count++;
                    
                    break;
                }
                currentTree.HasLeftChildren++;
                //currentTree.Count++;
                currentTree = currentTree.Left;
                
            }
            else
            {
                if (currentTree.Right == null)
                {
                    currentTree.Right = treeToAdd;
                    treeToAdd.Parent = currentTree;
                    //currentTree.Count++;
                    break;
                }
                //currentTree.Count++;
                currentTree = currentTree.Right;
            }
        }
        //treeToAdd.Parent = this;
    }

    public int GetIndex()
    {
        var current = this;
        int index = int.MinValue;
        if (current.Parent == null)
        {
            index = current.HasLeftChildren;
            return index;
        }   
        if(current.Parent.Right == current)
        {
            index = current.Parent.GetIndex() + current.HasLeftChildren + 1;
        }
        else if(current.Parent.Left == current)
        {
            index = current.Parent.GetIndex() - current.Count  + current.HasLeftChildren;
        }
        return index;
    }


    public IEnumerator<T> GetEnumerator()
    {
     
        return this?.GetValues().GetEnumerator();

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Contains(T query)
    {
        if(Count == 0)
            return false;
        var stack = new Stack<BinaryTree<T>>();
        stack.Push(this);
        while(stack.Count > 0)
        {
            var current = stack.Pop();
            if(query.CompareTo(current.Value) == 0)
                return true;
            if(query.CompareTo(current.Value) < 0)
            {
                if(current.Left != null)
                    stack.Push(current.Left);
            }
            else if(query.CompareTo(current.Value) > 0)
            {
                if(current.Right != null)
                    stack.Push(current.Right);
            }
        }
        return false;

    }

    public T this[int index]
    {
        get
        {

            if (index < 0 || index > Count - 1)
                throw new IndexOutOfRangeException();
            
            
            var current = this;
            
            while (true)
            {
                var currentIndex = current.GetIndex();
                if (index == currentIndex)
                {
                    return current.Value;
                }
                if (index < currentIndex)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
            
            
            
        }
    }
    public IEnumerable<T> GetValues()
    {
        if(Count == 0)
            yield break;
        if(Left != null)
        foreach (var child in Left)
        {
            yield return child;
        }
        yield return Value;
        if(Right != null)
        foreach (var child in Right)
        {
            yield return child;
        }
            
    }
    
}
