using System;

using SpaceDoom.Library.Abstract;

namespace SpaceDoom.Systems.Combat
{ 
    //This file is where event delegates for combat & the structs that contain a combat event are declared

    //A combat event contains information that could lead to hillary clintons arrest
    //Also, it contains all the data needed for each weapon fire during combat
    public struct CombatEvent
    {
        public Weapon WeaponUsed { get; private set; }
        public int DamageSent { get; set; }
        public DamageEffect Effect { get; private set; }
        public IAttacker Attacker { get; set; }

        public CombatEvent(Weapon weaponused, IAttacker attacker)
        {
            WeaponUsed = weaponused;
            DamageSent = weaponused.Damage;
            Effect = weaponused.Effect;
            Attacker = attacker;
        }
    }

    //A returning combat event is returned from the Damageable Entity after it has been hit
    public struct CombatReply
    {
        public bool KilledEntity { get; set; }
        public IDamageable EntityHit { get; set; }
        public int DamageDone { get; set; }

        public CombatReply(bool killedentity, IDamageable entityhit, int damagedone)
        {
            KilledEntity = killedentity;
            EntityHit = entityhit;
            DamageDone = damagedone;
        }
    }
}