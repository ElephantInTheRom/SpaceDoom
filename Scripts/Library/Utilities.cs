using System;
using System.Collections.Generic;
using SpaceDoom.Systems.Movement;

namespace SpaceDoom.Library
{
    public class DirectionQueue
    {
        private FixedQueue<Direction> Queue { get; set; }
        public Direction RealDirection { get => GetRealDirection(); private set { } }

        public DirectionQueue()
        {
            Queue = new FixedQueue<Direction>(2);
        }

        public void Enqueue(Direction dir) => Queue.Enqueue(dir);
        public void Clear() => Queue.Clear();

        //Returns the real directional value stored in this Queue (out of 8 cardinal directions)
        private Direction GetRealDirection()
        {
            if(Queue.Count == 0) { return Direction.Idle; }
            //Check for combinations. This is messy but it is the use case for else ifs
            if(Queue.Contains(Direction.Up) && Queue.Contains(Direction.Left)) { return Direction.UpLeft; }
            else if(Queue.Contains(Direction.Up) && Queue.Contains(Direction.Right)) { return Direction.UpRight; }
            else if(Queue.Contains(Direction.Down) && Queue.Contains(Direction.Right)) { return Direction.DownRight; }
            else if(Queue.Contains(Direction.Down) && Queue.Contains(Direction.Left)) { return Direction.DownLeft; }
            //If none of these combinations are true, than there is either an illegal combination, or no combination.
            //In this case just return the first value in this queue
            else { return Queue.Peek(); }
        }
    }

    /// <summary>
    /// A queue with a fixed size, acts as a circular buffer
    /// </summary>
    public class FixedQueue<T> : Queue<T>
    {
        public int Size { get; protected set; }

        public FixedQueue(int size)
        {
            Size = size;
        }

        //The new Keyword here hides the inherited Enqueue and replaces it with this one
        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            while(base.Count > Size) { base.Dequeue(); }
        }
    }
}