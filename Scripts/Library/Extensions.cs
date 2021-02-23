using System;
using Godot;

namespace SpaceDoom.Library.Extensions
{
    public static class MyExtensions
    {
        /// <summary>
        /// Makes the vector2 relative to an origin point.
        /// Returns destination.x 
        /// </summary>
        public static Vector2 RelativeTo(this Vector2 destination, Vector2 origin)
        {
            return new Vector2(destination.x - origin.x, destination.y - origin.y);
        }

    }
}
