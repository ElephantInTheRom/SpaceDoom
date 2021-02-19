using System;

using Godot;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Scenes;

namespace SpaceDoom.Systems.Combat
{
    // - - - Hitscan Weapons - - - \\
    public class LaserGun : HitscanWeapon
    {
        public LaserGun(YSort projLayer, Timer cooldownTimer)
        {
            Name = "LaserGun";
            ID = WeaponID.pl_lsrgun;
            Damage = 5;
            Effect = new BluntDamageEffect();
            CooldownTimer = cooldownTimer;

            DecalLayer = projLayer;
            ProjectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/Laser01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            //Instanse the scene, grab its script, set its data, and create it.
            var pInst = ProjectileScene.Instance();
            HitscanProjectile instScript = pInst as HitscanProjectile;
            instScript.SetDirection(attacker.Position, target);
            DecalLayer.AddChild(pInst);

            base.FireWeapon(attacker, target);
        }
    }


    public class LaserBeam : HitscanWeapon
    {
        public LaserBeam(YSort projLayer, Timer cooldownTimer)
        {
            Name = "LaserBeam";
            ID = WeaponID.pl_lsrbeam;
            Damage = 10;
            CooldownTimer = cooldownTimer;
        }
    }


    public class Shotgun : HitscanWeapon
    {
        public int Pellets { get; private set; } = 5;

        public Shotgun(YSort projLayer, Timer cooldownTimer)
        {
            Name = "Shotgun";
            ID = WeaponID.pl_shotgun;
            Damage = 4; //This is per shotgun PELLET
            CooldownTimer = cooldownTimer;
        }
    }


    public class Crossbow : HitscanWeapon
    {
        public Crossbow(YSort projLayer, Timer cooldownTimer)
        {
            Name = "Crossbow";
            ID = WeaponID.pl_crossbow;
            Damage = 100;
            CooldownTimer = cooldownTimer;
        }
    }


    public class Flamethrower : ProjectileWeapon
    {
        public Flamethrower(YSort projLayer, Timer cooldownTimer)
        {
            Name = "Flamethrower";
            ID = WeaponID.pl_flamethrower;
            Damage = 3;
            ProjectileTime = 1.5f;
            CooldownTimer = cooldownTimer;
        }
    }

    // - - - Projectile Weapons - - - \\

    public class Grenade : ProjectileWeapon
    {
        public Grenade(YSort projLayer, Timer cooldownTimer)
        {
            Name = "Grenade";
            ID = WeaponID.pl_grenade;
            Damage = 20;
            Effect = new ExplosionDamageEffect();
            ProjectileTime = 3f;
            CooldownTimer = cooldownTimer;
            ProjectileLayer = projLayer;
            ProjectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/Grenade01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            //Create a new projectile and pass this data onto its script
            var pInst = ProjectileScene.Instance();
            var pScript = pInst as GrenadeProjectile;
            pScript.SetData(target, new CombatEvent(this, attacker), ProjectileTime);
            ProjectileLayer.AddChild(pInst);
        }
    }


    public class Pulsar : ProjectileWeapon
    {
        public Pulsar(YSort projLayer, Timer cooldownTimer)
        {
            Name = "Pulsar";
            ID = WeaponID.pl_pulsar;
            Damage = 0;
            ProjectileTime = 100f;
            CooldownTimer = cooldownTimer;
        }
    }
}