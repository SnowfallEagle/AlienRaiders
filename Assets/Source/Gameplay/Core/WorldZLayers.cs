public static class WorldZLayers
{
    public const float Camera           = -500f;
    public const float Player           = 100f;
    public const float Alien            = Player + 1;
    public const float ProjectileAlien  = Alien + 1;
    public const float ProjectilePlayer = ProjectileAlien + 1;
    public const float Pickup           = ProjectilePlayer + 1;
    public const float BackgroundClouds = Pickup + 1;
    public const float BackgroundEffect = BackgroundClouds + 1;
    public const float BackgroundSprite = BackgroundEffect + 1;
}
