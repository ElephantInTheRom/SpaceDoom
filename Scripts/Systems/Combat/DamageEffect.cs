using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDoom.Systems.Combat
{
    //This class outlines different damage effects, eg Burning, exploding, freezing

    /* The damage system works as such:
     * An attacker uses a weapon with a damage effect tied to it. 
     * The weapon when used figures out if an attack hit. 
     * If the attack hit, a call to the recipient it sent containing - The sender, the weapon class used and the effect
     * The sender: Information about WHO sent the damage. 
     * The weapon: Information about the weapon used
     * The damage effect: (This) What type of damage to inflict. (How long, how many times? Using what particles?)
     * The damage manager uses this information to apply the correct effects to the recipient
    */

    public enum DamageEffectType 
    {
        BluntDamage,
        FireDamage,
        FrostDamage,
        ExplosionDamage,
        BleedDamage
    }

    public abstract class DamageEffect 
    {
        public DamageEffectType EffectType { get; protected set; }
    }


    //Blunt damage hits once for a certain amount of damage, starts laserhit particles and thats it
    public class BluntDamageEffect : DamageEffect
    {
        
        public int Damage { get; private set; }
        public string Particles { get; private set; }

        public BluntDamageEffect() { EffectType = DamageEffectType.BluntDamage; }
    }

    //Fire Damage hits once and then tapers off for a certain amount of time
    public class FireDamageEffect : DamageEffect
    {
        public FireDamageEffect()
        {
            EffectType = DamageEffectType.FireDamage;
        }
    }

    //
    public class FrostDamageEffect : DamageEffect
    {
        public FrostDamageEffect()
        {
            EffectType = DamageEffectType.FrostDamage;
        }
    }

    //
    public class ExplosionDamageEffect : DamageEffect
    {
        public ExplosionDamageEffect()
        {
            EffectType = DamageEffectType.ExplosionDamage;
        }
    }

    //
    public class BleedDamageEffect : DamageEffect
    {
        public BleedDamageEffect()
        {
            EffectType = DamageEffectType.BleedDamage;
        }
    }
}