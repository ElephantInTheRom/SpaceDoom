using System;
using System.Collections.Generic;

using Godot;

using SpaceDoom.Systems.Movement;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Systems.Combat;

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

    /// <summary>
    /// A data structure for defining results from a complex raycast.
    /// If there was no hit, this structs "NoCollision" bool will be true.
    /// </summary>
    public struct RaycastResults
    {
        public static RaycastResults Empty = new RaycastResults() { DamageableHit = false, NoCollision = true };
        
        public bool NoCollision { get; private set; }

        public bool DamageableHit { get; private set; }
        public IDamageable Damageable { get; private set; }

        public Vector2 EndPoint { get; private set; }
        public Vector2 CollisionPoint { get; private set; }
        public Vector2 CollisionNormal { get; private set; }
        public Godot.Object ObjectHit { get; private set; }

        public RaycastResults(Godot.Collections.Dictionary dictionary, Vector2 endpoint)
        {
            if(dictionary.Count > 0)
            {
                NoCollision = false;
                CollisionPoint = (Vector2)dictionary["position"];
                EndPoint = endpoint;
                CollisionNormal = (Vector2)dictionary["normal"];
                //ColliderID = (ulong)dictionary["collider_id"];
                ObjectHit = (Godot.Object)dictionary["collider"];

                if (ObjectHit is IDamageable)
                {
                    Damageable = ObjectHit as IDamageable;
                    DamageableHit = true;
                }
                else
                {
                    Damageable = null;
                    DamageableHit = false;
                }
            }
            else
            {
                this = Empty;
                EndPoint = endpoint;
            }
        }
    }
}