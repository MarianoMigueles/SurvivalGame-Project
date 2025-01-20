using System.Numerics;

namespace Utilities.DirectionalUtilities
{
    /// <summary>
    /// Provides utility methods for calculating directions between two points in 2D space.
    /// </summary>
    public static class DirectionHelper
    {
        /// <summary>
        /// Calculates a normalized direction vector from an origin point to a target point.
        /// </summary>
        /// <param name="origin">The starting point (origin) as a <see cref="Vector2"/>.</param>
        /// <param name="target">The destination point (target) as a <see cref="Vector2"/>.</param>
        /// <returns>
        /// A <see cref="Vector2"/> representing the direction from the origin to the target, normalized to a magnitude of 1. 
        /// If the origin and target are the same, returns a zero vector.
        /// </returns>
        public static Vector2 CalculateDirection(Vector2 origin, Vector2 target)
        {
            float deltaX = target.X - origin.X;
            float deltaY = target.Y - origin.Y;
            float magnitude = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            Vector2 result;

            if (magnitude > 0)
            {
                result.X = deltaX / magnitude;
                result.Y = deltaY / magnitude;
            }
            else
            {
                result.X = 0;
                result.Y = 0;
            }

            return result;
        }
    }
}

