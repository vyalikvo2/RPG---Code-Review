namespace SP.Constants
{
    public static class GameConstants
    {
        public const int RUN_ENDURANCE_PER_SECOND = 100000;
        
        public const float REGEN_TO_SECONDS = 1 / 10000.0f; // need to multiply server regen by this value (to get regen percent per second [0-1] )
    }
}
