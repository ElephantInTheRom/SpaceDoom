using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDoom.Systems.Combat
{
    //This is an enum of every weapon outlined from the weapon class
    //Note: is this bad practice? Should I be using reflection?
    //"pl_" are player weapons
    public enum WeaponID
    {
        pl_lsrgun,
        pl_lsrbeam,
        pl_shotgun,
        pl_crossbow,
        pl_flamethrower,
        pl_grenade,
        pl_pulsar
    }
}