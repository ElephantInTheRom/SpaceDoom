using Godot;
using SpaceDoom.Systems.Combat;
using System;

using SpaceDoom.Components;
using SpaceDoom.Library.Extensions;

namespace SpaceDoom.Library.Abstract
{
    //The base class for all enemies in the game
    public class Enemy : KinematicBody2D, IDamageable
    {
        //Data
        [Export] public int Health { get; protected set; }
        [Export] public int Armor { get; protected set; }

        //Engine Data
        private Physics2DDirectSpaceState SpaceState { get; set; }

        //Nodes
        protected AnimatedSprite Sprite { get; set; }
        public DamageFXManager DMFXManager { get; set; }

        //Scripts
        private HealthDisplay Healthbar { get; set; }

        //Delegates
        public delegate void EnemyHealthDelegate(int difference);
        //Events
        public event EnemyHealthDelegate @HealthChanged;
        public event EnemyHealthDelegate @Died;

        //Godot methods
        public override void _Ready()
        {
            base._Ready();

            Sprite = GetNode<AnimatedSprite>("Sprite");
            DMFXManager = GetNode<DamageFXManager>("DmgFXManager");
            DMFXManager.Parent = this;

            Healthbar = GetNode<HealthDisplay>("HealthDisplay");
            Healthbar.InjectData(Health, Health, true, this);
        }

        public override void _PhysicsProcess(float delta)
        {
            SpaceState = GetWorld2d().DirectSpaceState; //Set the space state

            base._PhysicsProcess(delta);
        }

        //- - - Damage interaction methdos - - -\\
        public virtual void ProcessCombatEvent(CombatEvent comEvent)
        {
            bool fatal = (Health - comEvent.DamageSent) <= 0;
            int actualDamage = fatal ? Health : comEvent.DamageSent;
            //Invoke event
            @HealthChanged?.Invoke(-actualDamage);
            //Change health
            Health -= actualDamage;
            comEvent.Attacker.ProcessCombatReply(new CombatReply(fatal, this, actualDamage));
            if (fatal) { TriggerDeath(actualDamage); }
        }

        public virtual void TriggerDeath(int finalBlow)
        {
            @Died?.Invoke(finalBlow);
            QueueFree();
        }

        protected void _invokeDealthAction(int finalBlow) => @Died?.Invoke(finalBlow);

        // - - - Targeting/Attacking Methods&Data - - -\\
        protected bool CanSeePlayer()
        {
            var cast = SendComplexCast(PlayerSingleton.PlayerInstance.GlobalPosition);
            if(!cast.NoCollision && cast.ObjectHit is Player)
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }


        //Send out a complex raycast, and return IDamageable if there was a hit.
        public RaycastResults SendComplexCast(float range = 5000, float angleOffset = 0)
        {
            float theta = (RotationDegrees + angleOffset).NormalizeRotation();

            var destination = GlobalPosition.GetDistantPoint(theta + angleOffset, range);

            var result = SpaceState.IntersectRay(GlobalPosition, destination, new Godot.Collections.Array { this });

            return new RaycastResults(result, destination);
        }

        public RaycastResults SendComplexCast(Vector2 destination)
        {
            var result = SpaceState.IntersectRay(GlobalPosition, destination, new Godot.Collections.Array { this });

            return new RaycastResults(result, destination);
        }
    }
}