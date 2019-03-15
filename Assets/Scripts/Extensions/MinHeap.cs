using System;

// Self expanding native array implementation of a MinHeap (binary tree where the parent is always "smaller" than it's children)
// Based on this tutorial for nodal pathfinding http://www.jgallant.com/nodal-pathfinding-in-unity-2d-with-a-in-non-grid-based-games/
public class MinHeap<T> where T : IComparable<T> {
    private int _count;
    private int _capacity;
    private T _temp;
    private T _mintemp; // temp for storing the smallest T when minifing
    private T[] _array;
    private T[] _temparray; // needed for self expansion

    public int Count
    {
        get { return _count; }
    }

    // default tiny constructor
    public MinHeap() : this(16) { }

    public MinHeap(int capacity)
    {
        _count = 0;
        _capacity = capacity;
        _array = new T[capacity];
    }

    // Add an item to the heap.
    // expands the heap if more space is needed.
    // minifies as it adds.
    public void Add(T item)
    {
        _count++;
        if (_count > _capacity)
            ExpandArray();

        int pos = _count - 1;
        _array[pos] = item; // new item to the last position of the array.

        int parentPos = GetParentPos(pos);

        // work your way up the parents and swap if the child is smaller.
        while (pos > 0 && _array[parentPos].CompareTo(_array[pos]) > 0) {
            // swap
            _temp = _array[pos];
            _array[pos] = _array[parentPos];
            _array[parentPos] = _temp;

            // move to parent
            pos = parentPos;
            parentPos = GetParentPos(pos);
        }
    }

    // Extracts the first element from the heap (which is the smallest).
    // The last element becomes the first and everything is minified.
    public T ExtractMin()
    {
        if (_count == 0)
            throw new InvalidOperationException("Heap is empty");

        _temp = _array[0];
        _array[0] = _array[_count - 1];
        _count--;

        MinHeapify(0);

        return _temp;
    }

    // Helpers

    private void MinHeapify(int pos)
    {
        while (true) {
            int left = GetLeftPos(pos);
            int right = left + 1;

            // assume the parent is the min.
            int minPos = pos;

            // update to the left if it's smaller.
            if (left < _count && _array[left].CompareTo(_array[minPos]) < 0)
                minPos = left;

            // update to the right if it's smaller.
            if (right < _count && _array[right].CompareTo(_array[minPos]) < 0)
                minPos = right;

            // swap the parent with the min if needed.
            if (minPos != pos) {
                _mintemp = _array[pos];

                // swap
                _array[pos] = _array[minPos];
                _array[minPos] = _mintemp;
                pos = minPos;
            } else {
                return;
            }
        }
    }

    // copies the array into a new array, double the size.
    private void ExpandArray()
    {
        _capacity <<= 1;
        _temparray = new T[_capacity];

        for (int i = 0; i < _array.Length; i++) {
            _temparray[i] = _array[i];
        }

        _array = _temparray;
    }

    // bit shift to get the parent array position without division: floor((n -1) / 2)
    private int GetParentPos(int pos)
    {
        return ((pos - 1) >> 1);
    }

    // bit shift to get the left child array position without division: (2*n) + 1
    private int GetLeftPos(int pos)
    {
        return ((pos << 1) + 1);
    }
}
