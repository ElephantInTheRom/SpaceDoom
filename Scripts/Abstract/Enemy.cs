using Godot;
using SpaceDoom.Systems.Combat;
using System;

namespace SpaceDoom.Library.Abstract
{
    //The base class for all enemies in the game
    public class Enemy : KinematicBody2D, IDamageable
    {
        //Data
        public int Health { get; protected set; } = 100;
        public int Armor { get; protected set; } = 0;

        //Nodes
        protected AnimatedSprite Sprite { get; set; }
        public DamageFXManager DMFXManager { get; set; }

        //Scripts
        private Healthbar Healthbar { get; set; }

        //Events
        public event Action<int> HealthChanged; //Action is invoked every time health is changed with the difference in health

        //Godot methods
        public override void _Ready()
        {
            base._Ready();

            Sprite = GetNode<AnimatedSprite>("Sprite");
            DMFXManager = GetNode<DamageFXManager>("DmgFXManager");
            DMFXManager.Parent = this;

            Healthbar = GetNode<Healthbar>("Healthbar");
            Healthbar.SetData(Health, Health);
            HealthChanged += Healthbar.HealthChanged;
        }

        public void TestLog(int line) { GD.Print(line); }

        //- - - Damage interaction methdos - - -\\
        public void ProcessCombatEvent(CombatEvent comEvent)
        {
            bool fatal = (Health - comEvent.DamageSent) <= 0;
            int actualDamage = fatal ? Health : comEvent.DamageSent;
            //
            if (HealthChanged != null) { HealthChanged(-actualDamage); }
            //
            Health -= actualDamage;
            comEvent.Attacker.ProcessCombatReply(new CombatReply(fatal, this, actualDamage));
            if (fatal) { TriggerDeath(); }
        }

        public void TriggerDeath()
        {
            GD.Print("Enemy down!");
            QueueFree();
        }
    }
}