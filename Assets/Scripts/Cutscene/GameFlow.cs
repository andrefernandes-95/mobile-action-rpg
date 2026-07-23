namespace AF
{
    public static class GameFlow
    {
        public static bool IsInCutscene
        {
            get;
            private set;
        }

        public static void SetCutscene(bool value)
        {
            IsInCutscene = value;
        }
    }
}
