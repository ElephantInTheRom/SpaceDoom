using System;
using System.Collections.Generic;
using Godot;
using static Godot.GD;
using SpaceDoom.Library.Abstract;

namespace SpaceDoom.Systems.Combat
{
    /*
    Hey Hayden, I hear you asking, why do we have to have a whole seperate class just to throw a raycast
    that returns multiple objects? 
    Well, fellow reviewer, this is because a regular raycast node does not return all objects it intersects with
    instead, it ONLY has the ability to return the first node it intersects with
    Why? I have no idea.
    So whats the next best thing right? Well that would be to have an area on my projectile that just tracks what collides with it
    Okay, so we do that and collisions arent detected from an area until the next _Fixed_Process() rotation/ frame
    So we have to keep a whole area on every entity that will need to do piercing hitscan shots
    This class will need to keep a list of all objects its intersecing with, along with all damageable entities its intersecting with
    Because reasons. 
    :(
    */
    public class HitscanBeam : Area2D
    {
        //Lists 
        public List<IDamageable> Damageables { get; private set; }
        public List<Godot.Object> Colliders { get; private set; }

        public override void _Ready()
        {
            base._Ready();

            Damageables = new List<IDamageable>();
            Colliders = new List<Godot.Object>();
        }

        //When objects enter and exit
        public void BodyEntered(PhysicsBody2D body)
        {
            if (!Colliders.Contains(body))
            {
                Colliders.Add(body);
                if (body is IDamageable) { Damageables.Add(body as IDamageable); }
            }
        }

        public void BodyExited(PhysicsBody2D body)
        {
            if (Colliders.Contains(body))
            {
                Colliders.Remove(body);
                if (body is IDamageable) { Damageables.Remove(body as IDamageable); }
            }
        }
    }
}