using System;
using System.Collections.Generic;
using UnityEngine;

public class HashMap<TKey, TValue> where TKey : IComparable<TKey>
{
     private Pair<TKey, TValue>[] items;
    private const int InitialCapacity = 20;
    private float loadFactor = 0.5f;

    public int Size { get; private set; }

    public TValue this[TKey key]
    {
        get { return Find(key); }
        set { Set(key, value); }
    }

    public HashMap()
    {
        Clear();
    }

    public void Clear()
    {
        items = new Pair<TKey, TValue>[InitialCapacity];
        Size = 0;
    }

    public void Add(TKey key, TValue value)
    {
        int index = Hash(key); // getting the index

        if (items[index] == null)
        {
            items[index] = new Pair<TKey, TValue>(key, value); // inserts in that index
        }
        else
        {
            int resolvedIndex = ResolveCollision(index, key); // if it's not empty, then resolve collision
            items[resolvedIndex] = new Pair<TKey, TValue>(key, value);
        }

        Size++;

        if ((float)Size / items.Length > loadFactor)
        {
            IncreaseCapacity();
        }
    }

    private void IncreaseCapacity()
    {
        Pair<TKey, TValue>[] oldItems = items;
        items = new Pair<TKey, TValue>[oldItems.Length * 2];
        Size = 0;

        foreach (Pair<TKey, TValue> pair in oldItems)
        {
            if (pair == null) continue;
            Add(pair.Key, pair.Value);
        }
    }

    // Linear probing
   protected virtual int ResolveCollision(int index, TKey key)
{
    int startIndex = index;

    while (true)
    {
        // Move to next slot (wrap around if needed)
        index = (index + 1) % items.Length;

        // If the slot is empty, we're good to insert here
        if (items[index] == null) // Check for null explicitly
        {
            return index;
        }

        // If the key already exists, replace it (optional depending on your logic)
        if (items[index].Key.CompareTo(key) == 0)
        {
            return index;
        }

        // If we wrapped all the way around, the table is full
        if (index == startIndex)
        {
            throw new InvalidOperationException("Hash table is full.");
        }
    }
}

    protected virtual int FindIndexByHashValue(int hashValue, TKey key)
    {
        for (int i = hashValue; i < items.Length; i++)
        {
            if (items[i] != null && items[i].Key.CompareTo(key) == 0)
            {
                return i;
            }
        }

        for (int i = 0; i < hashValue; i++)
        {
            if (items[i] != null && items[i].Key.CompareTo(key) == 0)
            {
                return i;
            }
        }

        return -1;
    }

    protected virtual int Hash(TKey key)
    {
        return Math.Abs(key.GetHashCode() % items.Length); // this helps us get the index
    }

    public TValue Find(TKey key)
    {
        int hashValue = Hash(key);
        int index = FindIndexByHashValue(hashValue, key);
        if (index < 0)
        {
            throw new KeyNotFoundException("Key does not exist");
        }
        return items[index].Value;
    }

    public void Set(TKey key, TValue value)
    {
        int hashValue = Hash(key);
        int index = FindIndexByHashValue(hashValue, key);
        if (index < 0)
        {
            throw new KeyNotFoundException("Key does not exist");
        }

        items[index].Value = value;
    }

    public TValue FindAndRemove(TKey key)
    {
        int hashValue = Hash(key);
        int index = FindIndexByHashValue(hashValue, key);
        if (index < 0)
        {
            throw new KeyNotFoundException("Key does not exist");
        }

        TValue value = items[index].Value;
        items[index] = null;
        Size--;
        return value;
    }

    public bool ContainsKey(TKey key)
    {
        int hashValue = Hash(key);
        int index = FindIndexByHashValue(hashValue, key);
        return index >= 0;
    }

    private class Pair<XKey, XValue>
    {public XKey Key { get; private set; }
        public XValue Value { get; set; }

        public Pair(XKey key, XValue value)
        {
            Key = key;
            Value = value;
        }
    }

  
}
