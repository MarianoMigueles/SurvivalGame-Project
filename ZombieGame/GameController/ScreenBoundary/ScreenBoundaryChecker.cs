using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities;

namespace ZombieGame.GameController.ScreenBoundary
{
    /// <summary>
    /// Provides methods to check if a rectangular object is within the boundaries of the game screen.
    /// </summary>
    internal static class ScreenBoundaryChecker
    {
        /// <summary>
        /// Checks if the specified rectangle is within the boundaries of the screen,
        /// given the screen's width and height.
        /// </summary>
        /// <param name="obj">The rectangle representing the object's position and size.</param>
        /// <param name="screenWidth">The width of the screen in pixels.</param>
        /// <param name="screenHeight">The height of the screen in pixels.</param>
        /// <returns>True if the rectangle is entirely within the screen boundaries; otherwise, false.</returns>
        public static bool IsInBound(Rectangle obj, int screenWidth, int screenHeight)
        {
            int x = obj.X;
            int y = obj.Y;

            // Check if the rectangle is within the screen boundaries.
            bool inBound = x >= 0 && (x + obj.Width) <= screenWidth &&
                           y >= 0 && (y + obj.Height) <= screenHeight;

            return inBound;
        }

        /// <summary>
        /// Checks if the specified rectangle is within the boundaries of the screen,
        /// using the current active form's dimensions as the screen size.
        /// </summary>
        /// <param name="obj">The rectangle representing the object's position and size.</param>
        /// <returns>True if the rectangle is entirely within the active form's boundaries; otherwise, false.</returns>
        public static bool IsInBound(Rectangle obj)
        {
            // Use the active form's width and height as the screen size.
            return IsInBound(obj, GetFormWidth(), GetFormHeight());
        }

        /// <summary>
        /// Retrieves the width of the current active form.
        /// </summary>
        /// <returns>The width of the active form in pixels, or a default value of 800 if no form is active.</returns>
        private static int GetFormWidth()
        {
            // Return the active form's width, or 800 as the default if no form is active.
            return Form.ActiveForm?.Width ?? 800;
        }

        /// <summary>
        /// Retrieves the height of the current active form.
        /// </summary>
        /// <returns>The height of the active form in pixels, or a default value of 600 if no form is active.</returns>
        private static int GetFormHeight()
        {
            // Return the active form's height, or 600 as the default if no form is active.
            return Form.ActiveForm?.Height ?? 600;
        }
    }
}

