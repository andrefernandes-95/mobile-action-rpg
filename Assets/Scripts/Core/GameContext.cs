namespace AF
{
    public static class GameContext
    {
        public static CharacterManager Player { get; private set; }

        public static void SetPlayer(CharacterManager characterManager)
        {
            Player = characterManager;
        }
    }
}
