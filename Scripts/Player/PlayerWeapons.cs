using System;

using Godot;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Scenes;

namespace SpaceDoom.Systems.Combat
{
    // - - - Hitscan Weapons - - - \\
    public class LaserGun : HitscanWeapon
    {
        public LaserGun(YSort projLayer)
        {
            Name = "LaserGun";
            Damage = 5;
            Effect = new BluntDamageEffect();

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
        public LaserBeam()
        {
            Name = "LaserBeam";
            Damage = 10;
        }
    }


    public class Shotgun : HitscanWeapon
    {
        public int Pellets { get; private set; } = 5;

        public Shotgun()
        {
            Name = "Shotgun";
            Damage = 4; //This is per shotgun PELLET
        }
    }


    public class Crossbow : HitscanWeapon
    {
        public Crossbow()
        {
            Name = "Crossbow";
            Damage = 100;
        }
    }


    public class Flamethrower : ProjectileWeapon
    {
        public Flamethrower()
        {
            Name = "Flamethrower";
            Damage = 3;
            ProjectileTime = 1.5f;
        }
    }

    // - - - Projectile Weapons - - - \\

    public class Grenade : ProjectileWeapon
    {
        public Grenade(YSort projLayer)
        {
            Name = "Grenade";
            Damage = 20;
            Effect = new ExplosionDamageEffect();
            ProjectileTime = 3f;
            ProjectileLayer = projLayer;
            ProjectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/Grenade01.tscn");
        }

        public override void FireWeapon(IAttacker attacker, Vector2 target)
        {
            //Create a new projectile and pass this data onto its script
            var pInst = ProjectileScene.Instance();
            var pScript = pInst as GrenadeProjectile;
            pScript.SetData(target, new CombatEvent(this, attacker));
            ProjectileLayer.AddChild(pInst);
        }
    }


    public class Pulsar : ProjectileWeapon
    {
        public Pulsar()
        {
            Name = "Pulsar";
            Damage = 0;
            ProjectileTime = 100f;
        }
    }
}