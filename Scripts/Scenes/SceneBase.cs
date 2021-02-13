using Godot;
using System;

namespace SpaceDoom.Scenes
{
    //This is the class that represents the base behaviors of every scene
    public class SceneBase : Node
    {
        //Scenes
        protected PackedScene DialogGUIScene { get; set; }

        //Scripts
        protected Player _player { get; set; }

        //Common nodes (Scene with gameplay needs to have these)
        public YSort HitscanLayer { get; protected set; }

        //Ready method (this means that all method that inherit this one need to call base._Ready()!
        public override void _Ready()
        {
            base._Ready();
        }

        //Default bevahior for unloading a scene but keeping the player instance safe
        public virtual void SwitchScene(string path)
        {
            GetTree().ChangeScene(path);
        }

        protected void LoadChildAt(Node2D child, Vector2 position)
        {
            AddChild(child);
            child.Position = position;
        }
    }
}