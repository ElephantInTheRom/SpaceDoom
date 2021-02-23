using System;

using Godot;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Library.Extensions;
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
            CooldownTime = 0.1f;

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

            //Then send out the raycast for the laser MOVE THIS TO AN EXTENSION METHOD
            attacker.HitScanRaycast.CastTo = new Vector2(0, (Range == 0 ? 9999 : Range)); //Set the cast shape
            Godot.Object collision = attacker.HitScanRaycast.GetCollider(); //Grab the collision
            if (collision == null) { return; } //Exit the method if there is no hit
            //If there is a hit
            if (collision is IDamageable)
            {
                IDamageable entityHit = (IDamageable)collision;
                entityHit.ProcessCombatEvent(new CombatEvent(this, attacker));
            }

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
            CooldownTime = 4;

            DecalLayer = projLayer;
            ProjectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/Laserbeam01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            //Instanse the scene, grab its script, set its data, and create it.
            var pInst = ProjectileScene.Instance();
            HitscanBeam instScript = pInst as HitscanBeam;
            instScript.SetDirection(attacker.Position, target);
            DecalLayer.AddChild(pInst);
            
            
            if (raycastResults != null)
            {
                foreach (var col in raycastResults)
                {
                    var damageable = col;
                    damageable.ProcessCombatEvent(new CombatEvent(this, attacker));
                }
            }

            base.FireWeapon(attacker, target);
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
            CooldownTime = 2;
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
            CooldownTime = 5;
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
            CooldownTime = 10;
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
            CooldownTime = 2;
            ProjectileLayer = projLayer;
            ProjectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/Grenade01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            base.FireWeapon(attacker, target);
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
            CooldownTime = 4;
        }
    }
}