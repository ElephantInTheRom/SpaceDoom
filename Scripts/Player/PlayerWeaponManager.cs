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

    public PlayerWeaponManager(YSort hitLayer, YSort projLayer)
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
    }

    //Methods for sorting and selecting a specified weapon
    public Weapon SelectWeapon(WeaponID weaponId)
    {
        return (Weapon)from wpn in EquippedWeapons
               where wpn.ID == weaponId
               select wpn;
    }

    //Timer signals to call reload delegates
}