using Godot;
using static Godot.GD;
using System;

using SpaceDoom.Library.Abstract;
using SpaceDoom.Scenes;

namespace SpaceDoom.Systems.Combat
{
    //This is the base class for a weapon in the game
    public abstract class Weapon
    {
        //Data
        public string Name { get; protected set; } //Name must match up with its action in the input map
        public WeaponID ID { get; protected set; }
        public WeaponState State { get; protected set; }
        public int Damage { get; protected set; }
        public DamageEffect Effect { get; protected set; }

        public Timer CooldownTimer { get; protected set; }
        public float CooldownTime { get; protected set; }
        public bool Loaded { get; set; } = true;

        protected PackedScene ProjectileScene { get; set; }
        protected YSort ProjectileLayer { get; set; }
        protected int Range { get; set; }

        public Weapon()
        {
            StateStandby();
        }

        public virtual void FireWeapon(IAttacker attacker, Vector2 target) 
        {
            StateFiring();
            Loaded = false;
            CooldownTimer.Start(CooldownTime);
            StateReloading();
        }

        //Delegate for telling the weapon it is loaded again, called from a weapon manager
        public void Reload() 
        { 
            Loaded = true;
            StateStandby();
        }

        //Methods for changing states
        public void StateStandby() => State = WeaponState.ready;
        public void StateDisable() => State = WeaponState.disabled;
        public void StateReloading() => State = WeaponState.reloading;
        public void StateFiring() => State = WeaponState.firing;
    }

    public abstract class HitscanWeapon : Weapon
    {
        //Node that any hitscan decals will be made a child of
        public YSort DecalLayer { get; set; }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            base.FireWeapon(attacker, target);
        }
    }

    public abstract class ProjectileWeapon : Weapon
    {
        protected float ProjectileTime { get; set; }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            base.FireWeapon(attacker, target);

            Print($"Projectile fired! Name:{Name}");
        }
    }
}