using System;
using System.Collections.Generic;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.Utils{
    /// <summary>
    /// Performant Priority Queue, dequeuing smallest items first
    /// </summary>
    public struct BinaryMinHeap<T> where T : IComparable<T>{
        T[] heap;
        HashSet<T> lookup;
        public BinaryMinHeap(int capacity){
            heap = new T[capacity];
            lookup = new();
            Count = 0;
        }
        public int Count { get; private set; }

        /// <summary>
        /// Checks to see if parsed data is found inside heap
        /// </summary>
        /// <param name="data">The data we are comparing</param>
        /// <returns>True if found</returns>
        public readonly bool Contains(T data) => lookup.Contains(data);

        /// <summary>
        /// Resets the pointer, Resulting in the heap virtually resetting
        /// </summary>
        public void Clear()
        {
            Count = 0;
            lookup.Clear();
        }

        // ---

        /// <summary>
        /// Adds the item to the heap
        /// </summary>
        /// <param name="data">The item we wish to add to the heap</param>
        public void Enqueue(T data){
            if (Count >= heap.Length){ throw new InvalidOperationException("Reached max heap capacity"); }
            else if(!lookup.Add(data)){ return;  } // Stops dupes
            heap[Count] = data;
            Sort(Count, true);
            Count++;
        }

        /// <summary>
        /// Returns the minimum item from the heap
        /// </summary>
        public T Dequeue(){
            if (Count == 0) { return default; }

            T min = heap[0];
            lookup.Remove(min);
            heap[0] = heap[Count - 1];
            Count--;
            Sort(0, false);

            return min;
        }

        // ---

        /// <summary>
        /// Sorts the heap
        /// </summary>
        /// <param name="i">Starting index</param>
        /// <param name="up">If true, sorts from i -> index 0</param>
        void Sort(int i, bool up)
        {
            switch (up)
            {
                case true:
                    while (i > 0)
                    {
                        int parent = (i - 1) / 2;
                        if (heap[parent].CompareTo(heap[i]) <= 0) { break; }

                        Swap(i, parent);
                        i = parent;
                    }
                    break;
                case false:
                    while (true)
                    {
                        int lChild = 2 * i + 1;
                        int rChild = 2 * i + 2;
                        int smallest = i;

                        if (lChild < Count && heap[lChild].CompareTo(heap[smallest]) < 0) { smallest = lChild; }
                        if (rChild < Count && heap[rChild].CompareTo(heap[smallest]) < 0) { smallest = rChild; }

                        if (smallest == i) { break; }

                        Swap(i, smallest);
                        i = smallest;
                    }
                    break;
            }
        }


        /// <summary>
        /// Swaps the indexes in the heap
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        void Swap(int a, int b) => (heap[b], heap[a]) = (heap[a], heap[b]);

        // ---
    }
}