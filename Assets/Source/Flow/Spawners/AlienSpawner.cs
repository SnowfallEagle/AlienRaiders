using UnityEngine;
using UnityEngine.Assertions;

public class AlienSpawner : Spawner
{
    public class Config : SpawnerConfig
    {
        public PatternSpawnerConfig Pattern;

        public string ResourcePath = "Aliens/Alien";
    }

    public static class Pattern
    {
        public const int Single = 0;
        public const int Triple = 1;

        public const int MaxPatterns = 2;
    }

    public static class TripleSubpattern
    {
        public const int Left   = 0;
        public const int Right  = 1;
        public const int Row    = 2;
        public const int Column = 3;

        public const int MaxPatterns = 4;
    }

    protected override GameObject[] OnSpawn(SpawnerConfig BaseConfig)
    {
        Config Config = (Config)BaseConfig;

        var AlienPrefab = Resources.Load<GameObject>(Config.ResourcePath);
        Assert.IsNotNull(AlienPrefab);

        AlienPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        switch (Config.Pattern.GetPattern(Pattern.MaxPatterns))
        {
            case Pattern.Single: return SpawnSingle(Config, AlienPrefab);
            case Pattern.Triple: return SpawnTriple(Config, AlienPrefab);
            default: return new GameObject[] { };
        }
    }

    private GameObject[] SpawnSingle(Config Config, GameObject AlienPrefab)
    {
        GameObject Alien = SpawnInState(AlienPrefab);
        Vector3 Size = Alien.GetComponent<SpriteRenderer>().bounds.size;

        Vector3 Position;
        if (!Config.GetSpawnPosition(out Position, Size.x, Size.y))
        {
            float XRange = RenderingService.Instance.TargetSize.x * 0.5f;
            Position = new Vector3(Random.Range(-XRange, XRange), RenderingService.Instance.TargetCenter.y + RenderingService.Instance.TargetSize.y * 0.6f);
        }
        Alien.transform.position = Position;

        return new GameObject[] { Alien };
    }

    private GameObject[] SpawnTriple(Config Config, GameObject AlienPrefab)
    {
        const int NumAliens = 3;
        float SpaceBetweenAliens = Config.SpaceBetweenAliens > 0f ? Config.SpaceBetweenAliens : 0.5f;

        // Spawn aliens
        GameObject[] Aliens = new GameObject[NumAliens];
        for (int i = 0; i < NumAliens; ++i)
        {
            Aliens[i] = SpawnInState(AlienPrefab);
        }

        // Set up config
        Vector3 AlienSize = Aliens[0].GetComponent<BoxCollider2D>().bounds.size;

        float XDiff = 0f;
        float YDiff = 0f;

        switch (Config.Pattern.GetSubpattern(TripleSubpattern.MaxPatterns))
        {
            case TripleSubpattern.Right:
                XDiff = AlienSize.x + SpaceBetweenAliens;
                YDiff = AlienSize.y + SpaceBetweenAliens;
                break;

            case TripleSubpattern.Left:
                XDiff = -(AlienSize.x + SpaceBetweenAliens);
                YDiff = AlienSize.y + SpaceBetweenAliens;
                break;

            case TripleSubpattern.Column:
                XDiff = AlienSize.x + SpaceBetweenAliens;
                break;

            case TripleSubpattern.Row:
                YDiff = AlienSize.y + SpaceBetweenAliens;
                break;
        }

        float GroupWidth = XDiff * NumAliens;
        float GroupHeight = YDiff > 0f ? YDiff * NumAliens : AlienSize.y;

        Vector3 FirstPosition;
        if (!Config.GetSpawnPosition(out FirstPosition, GroupWidth, GroupHeight))
        {
            Config.GetSpawnPosition(out FirstPosition, SpawnerConfig.AlignType.Center, GroupWidth, GroupHeight);
        }

        // Set positions
        Aliens[0].transform.position = FirstPosition;
        Aliens[1].transform.position = new Vector3(FirstPosition.x + XDiff, FirstPosition.y + YDiff);
        Aliens[2].transform.position = new Vector3(FirstPosition.x - XDiff, FirstPosition.y - YDiff);

        return Aliens;
    }
}
