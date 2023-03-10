namespace PawnGame.GameObjects.Enemies
{

    internal abstract class Enemy : Entity
    {
        protected bool _isAlerted;

        public Enemy(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
<<<<<<< HEAD
            _isAlerted = false;
            //AbilityToDrop = null;
=======
            IsAlerted = false;
            AbilityToDrop = null;
>>>>>>> parent of 7b2df1c (Merge branch 'main' of https://github.com/106-Section-3-Group-2/PawnGame)
        }
        public abstract void Update();
    }
}
