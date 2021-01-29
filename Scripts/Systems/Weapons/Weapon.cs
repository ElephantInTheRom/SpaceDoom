using Godot;
using System;

using SpaceDoom.Library.Abstract;

namespace SpaceDoom.Systems.Weapons
{
    //This is the base class for a weapon in the game
    public abstract class Weapon
    {
        //Data
        protected string Name { get; set; }
        protected int Damage { get; set; }
        protected DamageEffect Effect { get; set; }
        protected int Range { get; set; }

        public abstract void FireWeapon(IAttacker sender);
    }

    public abstract class HitscanWeapon : Weapon
    {

    }

    public abstract class ProjectileWeapon : Weapon
    {
        protected float ProjectileTime { get; set; }
    }
}