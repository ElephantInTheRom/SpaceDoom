using Godot;
using System;

using SpaceDoom.Systems.Combat;

namespace SpaceDoom.Enemies
{
    public class EnemyLaser : KinematicBody2D
    {
        //This is a basic laser attack, it flies on a vector and if it hits the pleyer it deals damage and dissolves

        //Data
        private Vector2 StartPos { get; set; }
        private Vector2 Velocity { get; set; }
        public float Speed { get; private set; }
        public CombatEvent _CombatEvent { get; private set; }

        //GD methods
        public override void _Ready()
        {
            base._Ready();


        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

            var collision = 
                MoveAndCollide(Velocity * Speed * delta);

            if(collision != null)
            {
                //Do stuff
                GD.Print("Collided!");
            }
        }

        //Init
        public void InjectData(CombatEvent evnt, Vector2 start, Vector2 target, float speed)
        {
            _CombatEvent = evnt;
            StartPos = start;

            Velocity = new Vector2(target.x - start.x, target.y - start.y);
            if (!Velocity.IsNormalized()) { Velocity = Velocity.Normalized(); }

            Speed = speed;
            Position = start;
        }
    }
}