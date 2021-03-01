using System;
using System.Collections.Generic;
using Godot;
using SpaceDoom.Library.Abstract;

namespace SpaceDoom.Library.Extensions
{
    public static class MyExtensions
    {
        /// <summary>
        /// Makes the vector2 relative to an origin point.
        /// </summary>
        public static Vector2 RelativeTo(this Vector2 destination, Vector2 origin)
        {
            return new Vector2(destination.x - origin.x, destination.y - origin.y);
        }      

        /// <summary>
        /// The same as GetCollider(), but only looks for IDamageables
        /// </summary>
        /// <param name="range">Optional range variable</param>
        /// <returns>A damaeable if it hits one, or null if not.</returns>
        public static IDamageable GetDamageableCollider(this RayCast2D cast, float range = 9999f)
        {
            cast.CastTo = new Vector2(0, range); //Set the cast shape
            Godot.Object collision = cast.GetCollider(); //Grab the collision
            if (collision == null || !(collision is IDamageable)) { return null; } 
            else { return collision as IDamageable; }
        }

        public static bool IsOdd(this int i) => (i % 2) != 0;
    }
}
