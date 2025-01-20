using GunExtreme.Entities;
using System.Numerics;
using ZombieGame.Entities.Ammunition;
using ZombieGame.Entities.Players;

namespace ZombieGame.GameController.EntityManager
{
    public interface IManagerGameEntities
    {
        public void ReleaseEntity(AbstractEntity obj);
        public Player GenerateNewPlayer(string name);
        public Task GenerateNewZombieAsync(string type, int x, int y);
        public Bullet GenerateNewBullet();
        public Vector2 GetPlayerPosition();
        public Point GetMousePosition();
    }
}