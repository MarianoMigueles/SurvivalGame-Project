using SpatialStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Entities.Ammunition;
using ZombieGame.Entities.Enemies;
using ZombieGame.Entities.Players;

namespace ZombieGame.GameController
{
    /// <summary>
    /// Handles collision detection and interaction between game entities such as the player, enemies, and ammunition.
    /// </summary>
    public static class CollisionHandler
    {
        // Quadtree to manage collidable entities in the game.
        private static Quadtree<ICollidable> _gameQuadtree;

        // Reference to the player.
        private static IPlayer _player;

        /// <summary>
        /// Initializes the collision system with the player and quadtree instance.
        /// </summary>
        /// <param name="player">The player object implementing IPlayer.</param>
        /// <param name="gameQuadtree">The quadtree managing collidable entities.</param>
        public static void InitializeCollisionGame(IPlayer player, Quadtree<ICollidable> gameQuadtree)
        {
            _gameQuadtree = gameQuadtree;
            _player = player;
        }

        /// <summary>
        /// Checks for collisions between the player, enemies, and ammunition, 
        /// and resolves interactions such as attacks and damage.
        /// </summary>
        public static void CheckCollisions()
        {
            // Skip collision detection if the player is not active.
            if (!_player.IsActive) return;

            // Retrieve areas with potential collisions from the quadtree.
            List<Rectangle> areasWithPotentialCollisions = _gameQuadtree.Retrieve();
            var playerArea = _player.Area;

            // Iterate through each area with potential collisions.
            foreach (Rectangle area in areasWithPotentialCollisions)
            {
                // Retrieve entities in the current area.
                List<ICollidable> potentialCollisions = _gameQuadtree.Retrieve(area);

                // Handle collisions between the player and enemies.
                foreach (ICollidable entity in potentialCollisions)
                {
                    if (entity.IsActive && entity is IEnemy zombie && playerArea.IntersectsWith(zombie.Area))
                    {
                        // If the zombie is already attacking, skip further interactions.
                        if (zombie.IsAttacking) return;

                        // Trigger the zombie's attack on the player.
                        zombie.Atack(_player);
                    }
                }

                // Handle collisions between ammunition and other entities.
                foreach (ICollidable entity in potentialCollisions)
                {
                    if (entity.IsActive && entity is IAmmunition ammunition)
                    {
                        Rectangle ammunitionArea = ammunition.Area;

                        // Check if the ammunition intersects with any enemies.
                        foreach (ICollidable otherEntity in potentialCollisions)
                        {
                            if (otherEntity.IsActive && otherEntity is IEnemy enemy)
                            {
                                if (ammunitionArea.IntersectsWith(enemy.Area))
                                {
                                    // Apply damage to the enemy and remove the ammunition.
                                    enemy.TakeDamage(ammunition.Damage);
                                    ammunition.Disappear();

                                    // Increment player's murder count if the enemy's health reaches zero.
                                    if (enemy.Health == 0)
                                        _player.Murders++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
