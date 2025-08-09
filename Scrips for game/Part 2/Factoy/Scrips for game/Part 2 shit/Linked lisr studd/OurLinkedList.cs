using UnityEngine;
using System;

public class OurLinkedList<T>
{
   Node<T> head;
    Node<T> tail;

    public OurLinkedList()
    {
        Empty();
    }

    public T this[int index]
    {
        get { return FindAt(index); }
        set { Set(index, value); }
    }

    public int Size { get; protected set; }
    public bool IsEmpty { get { return Size == 0; } }

    public T FindAt(int index)
    {
        return FindNodeAt(index).value;
    }

    public void Insert(T value)
    {
        InsertNodeBefore(tail, value);
    }

    public void InsertAt(T value, int index)
    {
        InsertNodeBefore(FindNodeAt(index), value);
    }

    public T Set(int index, T value)
    {
        FindNodeAt(index).value = value;
        return value;
    }

    public bool Remove(T value)
    {
        Node<T> node;
        int index = FindNode(value, out node);

        if (index == -1)
            return false;

        node.previous.next = node.next;
        node.next.previous = node.previous;
        Size--;

        return true;
    }

    public T RemoveAt(int index)
    {
        Node<T> node = FindNodeAt(index);
        node.previous.next = node.next;
        node.next.previous = node.previous;

        Size--;
        return node.value;
    }

    public void Empty()
    {
        head = new Node<T>(default(T), null, tail);
        tail = new Node<T>(default(T), head, null);
        Size = 0;
    }

    private void InsertNodeBefore(Node<T> node, T value)
    {
        Node<T> newNode = new Node<T>(value, node.previous, node);
        node.previous.next = newNode;
        node.previous = newNode;
        Size++;
    }

    private int FindNode(T value, out Node<T> node)
    {
        node = head;
        for (int i = 0; i < Size; i++)
        {
            node = node.next;
            if (node.value.Equals(value))
            {
                return i;
            }
        }
        node = null;
        return -1;
    }

    private Node<T> FindNodeAt(int index)
    {
        if (index < 0 || index >= Size)
        {
            throw new IndexOutOfRangeException("Invalid index");
        }
        Node<T> node;
        if (index < Size / 2)
        {
            node = head.next;
            for (int i = 0; i < index; i++)
            {
                node = node.next;
            }
        }
        else
        {
            node = tail;
            for (int i = Size - 1; i >= index; i--)
            {
                node = node.previous;
            }
        }
        return node;
    }

    private class Node<X>
    {
        public X value;
        public Node<X> next;
        public Node<X> previous;

        public Node(X value, Node<X> previous, Node<X> next)
        {
            this.value = value;
            this.previous = previous;
            this.next = next;
        }
    } 
}
