using System;
using System.Collections.Generic;
using Godot;

using SpaceDoom.Systems.Combat;

namespace SpaceDoom.Library.Abstract
{
    public interface IDamageFXManager
    {
        void TakeDamage(DamageEffect effect);
    }

    //An IDamageable is any entitiy that can recieve a combat event
    public interface IDamageable
    {
        void ProcessCombatEvent(CombatEvent comEvent);
    }

    //An IAttacker is any entity that can send a combat event and recieve a reply about an event
    public interface IAttacker
    {
        void ProcessCombatReply(CombatReply comReply);
        RaycastResults SendComplexCast(float range, float angleOffset);

        RayCast2D HitscanRaycast { get; set; }
        Vector2 Position { get; set; }
    }
}