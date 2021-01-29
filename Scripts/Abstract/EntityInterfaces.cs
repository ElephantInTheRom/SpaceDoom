using System;
using System.Collections.Generic;
using Godot;

using SpaceDoom.Systems.Weapons;

namespace SpaceDoom.Library.Abstract
{
    public interface IDamageFXManager
    {
        void TakeDamage(IAttacker sender, Weapon weaponUsed, DamageEffect effect);
    }

    public interface IDamageableEntity
    {
        DamageFXManager DMFXManager { get; set; }
        //void TakeDamage(IAttacker sender, Weapon weaponUsed, DamageEffect effect);
    }

    public interface IAttacker
    {
        Vector2 Position { get; set; }
        float RotationDegrees { get; set; }
        RayCast2D HitScanRayCast { get; set; }
    }
}