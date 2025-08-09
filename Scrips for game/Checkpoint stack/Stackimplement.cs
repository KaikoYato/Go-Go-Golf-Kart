using UnityEngine;
using System;

public class Stackimplement<T>
{
    Node<T> top = null; //Reference to the top. keeping it as null to avoid any null errors

    public bool isEmpty(){
        return top == null;
    }
    
    public void Push(T Item){ //Filling Array / Down the Can
        Node<T> newNode = new Node<T>(); // make new node and reference it to the top node
        newNode.value = Item; //Setting the value

        newNode.next = top; //.next references the one below the Top
        top = newNode;
        Debug.Log("Pushing Stack");
    }

    public T Pop(){ //Read top Value and Taking it out of the Array / out of the can
        Debug.Log("Popping  Stack");
        if (isEmpty())
        {
            throw new System.InvalidOperationException("THIS STACK EMPTY");
        }

        T tempNode = top.value;

        if (top.next == null) // TO CHECK IF THE NODE EVEN HAS A TOP VALUE IF NOT, WHEN POPPING IT, THE TOP VALUE WILL NOW RETURN NULL
        {
            top = null;  
        }
        else
        {
            top = top.next; // does the normal assignment
        }
        
        return tempNode;
    }

    public T Peek(){//Look at the top pringle / Read the the most recently added value in the array
        if(isEmpty()){
            return default(T);
            //throw new System.InvalidOperationException("THIS STACK EMPTY");
        }
        return top.value;
    }

    private class Node<X>{
        public X value;
        public Node<X> next;
    }
    
}
