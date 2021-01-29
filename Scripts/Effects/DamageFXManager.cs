using Godot;
using System;

using SpaceDoom.Library.Abstract;
using SpaceDoom.Systems.Weapons;
//This class controls the node that handles the visual effects and timers 
//associated with recieving damage and combat in the game. 
public class DamageFXManager : Node2D, IDamageFXManager
{
    //Nodes
    public Timer DamageTimer01 { get; private set; }
    public Timer DamageTimer02 { get; private set; }

    public Particles2D LaserHitParticles { get; private set; }

    //Data
    public IDamageableEntity Parent { private get; set; }

    //Godot methods - - - \\
    public override void _Ready()
    {
        DamageTimer01 = GetNode<Timer>("DMGTimer01");
        DamageTimer02 = GetNode<Timer>("DMGTimer02");
        LaserHitParticles = GetNode<Particles2D>("LaserHit");
    }

    //Damage Effect Routing - - -\\
    public void TakeDamage(IAttacker sender, Weapon weaponUsed, DamageEffect effect)
    {
        switch(effect.EffectType)
        {
            case DamageEffectType.BluntDamage:
                //Just do damage to the parent
                GD.Print("Blunt Damage Recieved!");
                LaserHitParticles.Emitting = true;
                break;
            case DamageEffectType.FireDamage:
                //Do damage to the parent, and repeat damage at interval for period of time
                GD.Print("Fire Damage Recieved!");
                break;
            case DamageEffectType.FrostDamage:
                //Do damage and freeze entity
                GD.Print("Frost Damage Recieved!");
                break;
            case DamageEffectType.ExplosionDamage:
                //Do damage and inflict fire and freeze
                GD.Print("Explosion Damage Recieved!");
                break;
            case DamageEffectType.BleedDamage:
                //Do damage over time
                GD.Print("Bleed Damage Recieved!");
                break;
        }
    }
}