using Godot;
using SpaceDoom.Systems.Weapons;
using System;

namespace SpaceDoom.Library.Abstract
{
    //The base class for all enemies in the game
    public class Enemy : KinematicBody2D, IDamageableEntity
    {
        //Data
        public int Health { get; protected set; } = 100;
        public int Armor { get; protected set; } = 0;
        //Nodes
        protected AnimatedSprite Sprite { get; set; }
        public DamageFXManager DMFXManager { get; set; }

        //Scripts


        //Godot methods
        public override void _Ready()
        {
            base._Ready();

            Sprite = GetNode<AnimatedSprite>("Sprite");
            DMFXManager = GetNode<DamageFXManager>("DmgFXManager");
            DMFXManager.Parent = this;
        }

        //- - - Damage interaction methdos - - -\\
        public void TriggerDeath()
        {

        }
    }
}