namespace MonoGameQuest
{
    public static class Constants
    {
        public const int DefaultAttackSpeed = 50;
        public const int DefaultIdleSpeed = 450;
        public const int DefaultMoveLength = 8;
        public const int DefaultMoveSpeed = 120;
        public const int DefaultWalkSpeed = 100;

        public static class DrawOrder
        {
            public const int Terrain = 1;
            public const int Sprites = 2;
            public const int Debug = 3;
            public const int Cursor = 4;
        }

        public static class UpdateOrder
        {
            public const int Display = 1;
            public const int Map = 2;
            public const int Models = 3;
            public const int Cursor = 4;
            public const int Terrain = 5;
            public const int Sprites = 6;
            public const int Debug = 7;
        }
    }
}
