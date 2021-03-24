using Godot;
using SpaceDoom.Systems.Combat;
using System;

using SpaceDoom.Components;

namespace SpaceDoom.Library.Abstract
{
    //The base class for all enemies in the game
    public class Enemy : KinematicBody2D, IDamageable
    {
        //Data
        [Export]
        public int Health { get; protected set; }
        [Export]
        public int Armor { get; protected set; }

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
    }
}