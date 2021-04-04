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
        /// Gets the coordinates of a point given the angle and distance
        /// </summary>
        /// <param name="rotationDegrees">Angle theta to desired point.</param>
        /// <param name="distance">Distance d to that point</param>
        /// <returns></returns>
        public static Vector2 GetDistantPoint(this Vector2 origin, float theta, float distance)
        {
            var x = (distance * Math.Cos(theta.ToRadians())) + origin.x;
            var y = (distance * Math.Sin(theta.ToRadians())) + origin.y;

            return new Vector2((float)x, (float)y);
        }

        /// <summary>
        /// The same as GetCollider(), but only looks for IDamageables
        /// </summary>
        /// <param name="range">Optional range variable</param>
        /// <returns>A damaeable if it hits one, or null if not.</returns>
        public static IDamageable GetDamageableCollider(this RayCast2D cast, float range = 9999f)
        {
            cast.CastTo = new Vector2(range, 0); //Set the cast shape
            Godot.Object collision = cast.GetCollider(); //Grab the collision
            if (collision == null || !(collision is IDamageable)) { return null; } 
            else {
                GD.Print(cast.GetCollisionPoint());
                return collision as IDamageable; 
            }
        }

        public static float NormalizeRotation(this float f)
        {
            if (f > 360) { return f %= 360; }
            else if (f < 0) { return f += 360; }
            else { return f; }
            
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if(val.CompareTo(max) > 0) { return max; }
            else if(val.CompareTo(min) < 0) { return min; }
            else { return val; }
        }

        public static float ToDegrees(this double r) => (float)(r * (180 / Math.PI));
        public static double ToRadians(this float d) => (double)(d * (Math.PI / 180));

        public static bool IsOdd(this int i) => (i % 2) != 0;
    }
}
