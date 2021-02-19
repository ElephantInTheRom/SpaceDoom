using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

using SpaceDoom.Systems.Combat;

//This class holds all references to the players collection of weapons
//It also holds information about those weapons cooldowns. 
public class PlayerWeaponManager : Node
{
    //Weapon Collections
    public List<Weapon> EquippedWeapons { get; private set; }
    public Weapon SelectedWeapon { get; private set; }
    //Actions
    public event Action SelectionChanged;

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
            new Shotgun(hitLayer, GetNode<Timer>("ShotgunTimer")),
            new Crossbow(hitLayer, GetNode<Timer>("CrossbowTimer")),
            new Flamethrower(hitLayer, GetNode<Timer>("FlamethrowerTimer")),
            new Grenade(projLayer, GetNode<Timer>("GrenadeTimer")),
            new Pulsar(projLayer, GetNode<Timer>("PulsarTimer"))
        };

        EquipWeapon(WeaponID.pl_lsrgun);
    }

    //Methods for sorting and selecting a specified weapon
    public void EquipWeapon(WeaponID weaponId)
    {
        SelectedWeapon = GrabWeapon(weaponId);
        if (SelectionChanged != null) { SelectionChanged(); }
    }

    public Weapon GrabWeapon(WeaponID weaponId)
    {
        var q = from wpn in EquippedWeapons
                where wpn.ID == weaponId
                select wpn;
        return q.FirstOrDefault();
    }

    public int GetWeaponIndex(WeaponID weaponId) => EquippedWeapons.IndexOf(GrabWeapon(weaponId));

    //Timer signals to call reload delegates
    public void Lsrguntimeout() => GrabWeapon(WeaponID.pl_lsrgun).Reload();
    public void Lsrbeamtimeout() => GrabWeapon(WeaponID.pl_lsrbeam).Reload();
    public void Grenadetimeout() => GrabWeapon(WeaponID.pl_grenade).Reload();
    public void Pulsartimeout() => GrabWeapon(WeaponID.pl_pulsar).Reload();
    public void Crossbowtimeout() => GrabWeapon(WeaponID.pl_crossbow).Reload();
    public void Flamethrowertimeout() => GrabWeapon(WeaponID.pl_flamethrower).Reload();
    public void Shotguntimeout() => GrabWeapon(WeaponID.pl_shotgun).Reload();
}