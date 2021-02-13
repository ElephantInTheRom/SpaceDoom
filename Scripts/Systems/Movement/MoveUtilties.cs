using System;
using System.Collections.Generic;
using Godot;

namespace SpaceDoom.Systems.Movement
{
    public enum Direction
    {
        Idle,
        //Cardinal directions
        Left,
        Right,
        Up,
        Down,
        //Corners
        UpLeft,
        UpRight,
        DownRight,
        DownLeft
    }

    public static class Maps
    {
        public static Dictionary<Direction, Vector2> DirVector = new Dictionary<Direction, Vector2>
        {
            { Direction.Idle, new Vector2(0,0) },

            { Direction.Left, new Vector2(-1, 0) },
            { Direction.Right, new Vector2(1, 0) },
            { Direction.Up, new Vector2(0, -1) },
            { Direction.Down, new Vector2(0, 1) },

            { Direction.UpLeft, new Vector2(-1,-1) },
            { Direction.UpRight, new Vector2(1,-1) },
            { Direction.DownLeft, new Vector2(-1,1) },
            { Direction.DownRight, new Vector2(1,1) }
        };

        public static Dictionary<Direction, string> PlayerAnimationGroup = new Dictionary<Direction, string>
        {
            {Direction.Up, "walking_up" },
            {Direction.Down, "walking_down" },
            {Direction.Left, "walking_left" },
            {Direction.Right, "walking_right" }
        };
    }
}