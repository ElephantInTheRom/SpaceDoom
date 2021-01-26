using System;
using Godot;
using System.Collections.Generic;

namespace SpaceDoom.Library
{
    public enum Direction
    { 
        Left,
        Right,
        Up,
        Down
    }

    public static class Maps
    {
        public static Dictionary<Direction, Vector2> DirVector = new Dictionary<Direction, Vector2>
        {
            { Direction.Left, new Vector2(-1, 0) },
            { Direction.Right, new Vector2(1, 0) },
            { Direction.Up, new Vector2(0, -1) },
            { Direction.Down, new Vector2(0, 1) }
        };

        public static Dictionary<Direction, string> PlayerAnimationGroup = new Dictionary<Direction, string>
        {
            {Direction.Up, "walking_up" },
            {Direction.Down, "walking_down" },
            {Direction.Left, "walking_left" },
            {Direction.Right, "walking_right" }
        };
    }

    //This class is going to help control the animation flow down the line, needs to be moved to a different file
    //Once animations are being worked on more in depth
    public class AnimationController
    {
        private AnimatedSprite Sprite { get; set; }

        public AnimationController(AnimatedSprite sprite)
        {
            Sprite = sprite;
            SetIdleAnimation();
        }

        public void SetIdleAnimation()
        {
            Sprite.Animation = "idle";
        }
    }
}