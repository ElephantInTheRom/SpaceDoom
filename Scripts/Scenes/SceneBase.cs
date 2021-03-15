using Godot;
using System;
using static PlayerSingleton;

namespace SpaceDoom.Scenes
{
    //This is the class that represents the base behaviors of every scene
    public class SceneBase : Node
    {
        //Scenes
        protected PackedScene DialogGUIScene { get; set; }

        //Data 
        public bool PlayerLoaded { get; protected set; } = false;

        //Common nodes (Scene with gameplay needs to have these)
        public YSort HitscanLayer { get; protected set; }

        //Ready method
        public override void _Ready()
        {
            base._Ready();
        }

        //Load player into scene
        protected void LoadPlayer(Vector2 position, bool loadGUI, Node parent = null, int zIndex = 1)
        {
            if (parent == null) { parent = this; }
            parent.AddChild(PlayerInstance);
            PlayerScript.Position = position;
            PlayerScript.ZIndex = zIndex;
            PlayerLoaded = true;

            if (loadGUI) 
            { 
                GetNode<CanvasLayer>("UI")?.AddChild(PlayerScript.WeaponManager.GUIRoot); 
            }
        }

        //Default bevahior for unloading a scene but keeping the player instance safe
        public virtual void SwitchScene(string path)
        {
            RemoveChild(PlayerInstance);
            GetTree().ChangeScene(path);
        }

        protected void LoadChildAt(Node2D child, Vector2 position, Node parent = null)
        {
            parent = parent == null ? this : parent;
            parent.AddChild(child);
            child.Position = position;
        }
    }
}