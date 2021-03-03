using System;
using System.Collections.Generic;
using System.Linq;

using Godot;
using static Godot.GD;

using SpaceDoom.Systems.Combat;
using SpaceDoom.Library.Abstract;


namespace SpaceDoom.Systems.Projectiles
{
    public class FireProjectile : Area2D
    {
        //This class represents an area for flamethrower damage. 
        //When this node gets created, it must fire its projectiles, and damage all entities within it. 
        private bool SendingDamage { get; set; } = false;

        private IAttacker Attacker { get; set; }
        public CombatEvent _CombatEvent { get; private set; }
        private List<PhysicsBody2D> BodiesInRadius { get; set; }
        private Queue<IDamageable> DamageableQueue { get; set; }

        private Particles2D Emitter { get; set; }

        public void SetDirection(IAttacker attacker, CombatEvent combatEvent)
        {
            Position = attacker.Position;
            RotationDegrees = attacker.RotationDegrees;
            _CombatEvent = combatEvent;
            Attacker = attacker;
        }

        public override void _Ready()
        {
            base._Ready();

            Emitter = GetNode<Particles2D>("Particles");

            BodiesInRadius = new List<PhysicsBody2D>();
            DamageableQueue = new Queue<IDamageable>();

            Emitter.Emitting = true;
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

            //Once we are sending damage to nodes
            if (SendingDamage)
            {
                //If it is assigned and has items, send damage, otherwise check if particles are over
                //If particles are over, free node. 
                if (DamageableQueue.Count > 0)
                {
                    DamageableQueue.Dequeue().ProcessCombatEvent(_CombatEvent);
                }
                else if (!Emitter.Emitting)
                {
                    QueueFree();
                }
            }
        }

        //Signal from timer to send damage
        public void DamageTimeout()
        {
            SendingDamage = true;

            if (BodiesInRadius.Count > 0)
            {
                var query = from body in BodiesInRadius
                            where body is IDamageable
                            orderby body.Position.DistanceTo(Position) ascending
                            select body;

                foreach (var b in query.ToArray())
                {
                    DamageableQueue.Enqueue(b as IDamageable);
                }
            }
        }

        //Bodies entering the flame range
        public void BodyEntered(PhysicsBody2D body)
        {
            if (body is IDamageable) { BodiesInRadius.Add(body); }
        }

        public void BodyExited(PhysicsBody2D body)
        {
            if (body is IDamageable && BodiesInRadius.Contains(body)) { BodiesInRadius.Remove(body); }
        }
    }
}