using System;
using System.Collections.Generic;

using Godot;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Library.Extensions;
using SpaceDoom.Library;
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
            BasicHitscanFire(attacker, target);

            base.FireWeapon(attacker, target);
        }
    }

    
    public class Shotgun : HitscanWeapon
    {
        public int Pellets { get; private set; } = 7;
        public float Cone { get; private set; } = 30f; //The offset for the pellets angle, in each direction

        public Shotgun(YSort projLayer, Timer cooldownTimer)
        {
            Name = "Shotgun";
            ID = WeaponID.pl_shotgun;
            Damage = 4; //This is per shotgun PELLET
            CooldownTimer = cooldownTimer;
            CooldownTime = 1;
            Range = 200;

            DecalLayer = projLayer;
            ProjectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/ShotgunPellet01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            //To avoid an endless loop, there must be enough space for pellets to be generated. 
            if(Cone * 2 < Pellets || Pellets < 2) { throw new Exception("Shotgun stats are set incorrectly!"); }

            var offsets = new List<float>(Pellets);
            int rPellets; //The remaining amount of pellets to assign, will only change if pellet count is odd

            if (Pellets.IsOdd()) { offsets.Add(0); rPellets = --Pellets / 2; }
            else { rPellets = Pellets / 2; }

            float origin = 0;
            float increment = Cone / rPellets;

            for(var p = 0; p < rPellets; p++)
            {
                offsets.Add(origin + increment);
                offsets.Add(-(origin + increment));
                origin += increment;
            }

            //List should be finished, now send raycasts. 
            foreach(var off in offsets)
            {
                var cast = attacker.SendComplexCast(Range, off);
                if (cast.DamageableHit) { cast.Damageable.ProcessCombatEvent(new CombatEvent(this, attacker)); }
                //Send projectile
                var pInst = ProjectileScene.Instance();
                HitscanProjectile instScript = pInst as HitscanProjectile;

                if (cast.NoCollision) { instScript.SetDirection(attacker.Position, cast.EndPoint, true); }
                else { instScript.SetDirection(attacker.Position, cast.CollisionPoint, true); }

                DecalLayer.AddChild(pInst);
            }

            base.FireWeapon(attacker, target);
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

            DecalLayer = projLayer;
            ProjectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/CrossbowBolt01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            BasicHitscanFire(attacker, target);

            base.FireWeapon(attacker, target);
        }
    }


    // - - - Projectile Weapons - - - \\
    public class LaserBeam : ProjectileWeapon
    {
        public LaserBeam(YSort projLayer, Timer cooldownTimer)
        {
            Name = "LaserBeam";
            ID = WeaponID.pl_lsrbeam;
            Damage = 10;
            CooldownTimer = cooldownTimer;
            CooldownTime = 4;

            ProjectileLayer = projLayer;
            ProjectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/Laserbeam01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            var instance = ProjectileScene.Instance();
            var instScript = instance as BeamProjectile;
            instScript.SetBeam(attacker.Position, target, new CombatEvent(this, attacker));
            ProjectileLayer.AddChild(instance);

            base.FireWeapon(attacker, target);
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