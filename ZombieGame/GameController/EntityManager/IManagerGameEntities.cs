using GunExtreme.Entities;

namespace ZombieGame.GameController.EntityManager
{
    internal interface IManagerGameEntities
    {
        public void ReleaseEntity(GameObject obj);
        public void AddEntity(GameObject obj);
        public void GenerateNewPlayer(string name);
        public void GenerateNewZombie(string type);
        public Bullet GenerateNewBullet();
        public Point GetMousePosition();
    }
}