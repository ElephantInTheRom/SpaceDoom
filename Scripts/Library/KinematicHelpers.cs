using System;
using Godot;
using System.Collections.Generic;

namespace SpaceDoom.Library
{
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