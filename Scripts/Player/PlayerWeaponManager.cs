using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

using SpaceDoom.Systems.Combat;
using SpaceDoom.Library.Abstract;

//This class holds all references to the players collection of weapons
//It also holds information about those weapons cooldowns. 
public class PlayerWeaponManager : Node
{
    //Weapon Collections
    public List<Weapon> EquippedWeapons { get; private set; }
    public Weapon SelectedWeapon { get; private set; }
    //Events
    public delegate void PlayerWeaponDelegate();
    public event PlayerWeaponDelegate WeaponChanged;
    public event PlayerWeaponDelegate EquippedWeapon;
    public event PlayerWeaponDelegate WeaponStateChanged;

    public override void _Ready()
    {
        base._Ready();
    }

    public void SetLayers(YSort hitLayer, YSort projLayer)
    {
        //For testing we will start this list off will all weapons
        EquippedWeapons = new List<Weapon>()
        {
            new LaserGun(hitLayer, GetNode<Timer>("LaserGunTimer")),
            new LaserBeam(hitLayer, GetNode<Timer>("LaserBeamTimer")),
            new Grenade(projLayer, GetNode<Timer>("GrenadeTimer")),
            new Pulsar(projLayer, GetNode<Timer>("PulsarTimer")),
            new Crossbow(hitLayer, GetNode<Timer>("CrossbowTimer")),
            new Flamethrower(hitLayer, GetNode<Timer>("FlamethrowerTimer")),
            new Shotgun(hitLayer, GetNode<Timer>("ShotgunTimer"))
        };

        EquipWeapon(WeaponID.pl_lsrgun);
    }

    //Methods for sorting, selecting and firing weapons

    public void FireCurrentWeapon(IAttacker attacker, Vector2 target)
    {
        if(SelectedWeapon.Loaded)
        {
            SelectedWeapon.FireWeapon(attacker, target);
        }
    }

    public void EquipWeapon(WeaponID weaponId)
    {
        SelectedWeapon = GetWeapon(weaponId);
        if (WeaponChanged != null) { WeaponChanged(); }
    }

    public Weapon GetWeapon(WeaponID weaponId)
    {
        var q = from wpn in EquippedWeapons
                where wpn.ID == weaponId
                select wpn;
        return q.FirstOrDefault();
    }

    public int GetWeaponIndex(WeaponID weaponId) => EquippedWeapons.IndexOf(GetWeapon(weaponId));

    //Timer signals to call reload delegates
    public void Lsrguntimeout() => GetWeapon(WeaponID.pl_lsrgun).Reload();
    public void Lsrbeamtimeout() => GetWeapon(WeaponID.pl_lsrbeam).Reload();
    public void Grenadetimeout() => GetWeapon(WeaponID.pl_grenade).Reload();
    public void Pulsartimeout() => GetWeapon(WeaponID.pl_pulsar).Reload();
    public void Crossbowtimeout() => GetWeapon(WeaponID.pl_crossbow).Reload();
    public void Flamethrowertimeout() => GetWeapon(WeaponID.pl_flamethrower).Reload();
    public void Shotguntimeout() => GetWeapon(WeaponID.pl_shotgun).Reload();
}