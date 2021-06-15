using Godot;
using System;

namespace SpaceDoom.Player
{
    /// <summary>
    /// The main class for the player, controls behavior and features
    /// </summary>
    public class Player : KinematicBody2D
    {
        //Stats
        [Export] private int Speed { get; set; }
        [Export] public int Health { get; private set; }
        [Export] public int Armor { get; private set; }
        public float PlayerScore { get; set; }

        //Nodes
        private AnimatedSprite Sprite { get; set; }
        public Node2D FirePoint { get; private set; }

        // - - - Godot methods - - -
        public override void _Ready()
        {
            base._Ready();


        }

        public override void _Process(float delta)
        {
            base._Process(delta);


        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);


        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);

            if (Input.IsActionJustPressed("move_up")) { }
            if (Input.IsActionJustPressed("move_down")) { }
            if (Input.IsActionJustPressed("move_left")) { }
            if (Input.IsActionJustPressed("move_right")) { }
            if (Input.IsActionJustPressed("move_dash")) { }

        }
    }
}