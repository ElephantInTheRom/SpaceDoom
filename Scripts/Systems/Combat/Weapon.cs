using Godot;
using System;

using SpaceDoom.Library.Abstract;
using SpaceDoom.Scenes;

namespace SpaceDoom.Systems.Combat
{
    //This is the base class for a weapon in the game
    public abstract class Weapon
    {
        //Data
        public string Name { get; protected set; }
        public int Damage { get; protected set; }
        public DamageEffect Effect { get; protected set; }

        protected PackedScene ProjectileScene { get; set; }
        protected YSort ProjectileLayer { get; set; }
        protected int Range { get; set; }

        public abstract void FireWeapon(IAttacker attacker, Vector2 target);
    }

    public abstract class HitscanWeapon : Weapon
    {
        //Node that any hitscan decals will be made a child of
        public YSort DecalLayer { get; set; }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            attacker.HitScanRaycast.CastTo = new Vector2(0, (Range == 0 ? 9999 : Range)); //Set the cast shape
            Godot.Object collision = attacker.HitScanRaycast.GetCollider(); //Grab the collision
            if (collision == null) { return; } //Exit the method if there is no hit
            //If there is a hit
            if (collision is IDamageable)
            {
                IDamageable entityHit = (IDamageable)collision;
                entityHit.ProcessCombatEvent(new CombatEvent(this, attacker));
            }
        }
    }

    public abstract class ProjectileWeapon : Weapon
    {
        protected float ProjectileTime { get; set; }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            throw new NotImplementedException();
        }
    }
}