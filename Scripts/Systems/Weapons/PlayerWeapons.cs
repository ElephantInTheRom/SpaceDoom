using SpaceDoom.Library.Abstract;
using System;

using Godot;


namespace SpaceDoom.Systems.Weapons
{
    // - - - Hitscan Weapons - - - \\
    public class LaserGun : HitscanWeapon
    {
        public LaserGun()
        {
            Name = "LaserGun";
            Damage = 5;
            Effect = new BluntDamageEffect();
        }

        public override void FireWeapon(IAttacker sender)
        {
            GD.Print("Laser gun recieved call to fire!");

            sender.HitScanRayCast.CastTo = new Vector2(0,(Range == 0 ? -9999 : Range)); //Set the cast shape
            Godot.Object collision = sender.HitScanRayCast.GetCollider(); //Grab the collision
            GD.Print(collision);
            if (collision == null) { return; } //Exit the method if there is no hit

            IDamageableEntity EntityHit;
            if (collision is IDamageableEntity) { EntityHit = collision as IDamageableEntity; }
            else { return; }
            GD.Print("Gun detected enemy hit! Sending to enemy!");
            EntityHit.DMFXManager.TakeDamage(sender, this, Effect);
        }
    }


    public class LaserBeam : HitscanWeapon
    {
        public LaserBeam()
        {
            Name = "LaserBeam";
            Damage = 10;
        }

        public override void FireWeapon(IAttacker sender)
        {
            throw new NotImplementedException();
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

        public override void FireWeapon(IAttacker sender)
        {
            throw new NotImplementedException();
        }
    }


    public class Crossbow : HitscanWeapon
    {
        public Crossbow()
        {
            Name = "Crossbow";
            Damage = 100;
        }

        public override void FireWeapon(IAttacker sender)
        {
            throw new NotImplementedException();
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

        public override void FireWeapon(IAttacker sender)
        {
            throw new NotImplementedException();
        }
    }

    // - - - Projectile Weapons - - - \\

    public class Rocket : ProjectileWeapon
    {
        public Rocket()
        {
            Name = "Rocket";
            Damage = 20;
            ProjectileTime = 3f;
        }

        public override void FireWeapon(IAttacker sender)
        {
            throw new NotImplementedException();
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

        public override void FireWeapon(IAttacker sender)
        {
            throw new NotImplementedException();
        }
    }
}