using UnityEngine;
using UnityEngine.Assertions;

public class AlienSpawner : Spawner
{
    public class Config : SpawnerConfig
    {
        public PatternSpawnerConfig Pattern;

        public string ResourcePath = "Ships/Alien";
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

    private GameObject m_AlienPrefab;
    private Config m_Config;

    protected override GameObject[] OnSpawn(SpawnerConfig BaseConfig)
    {
        m_Config = (Config)BaseConfig;

        m_AlienPrefab = Resources.Load<GameObject>(m_Config.ResourcePath);
        Assert.IsNotNull(m_AlienPrefab);

        m_AlienPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        switch (m_Config.Pattern.GetPattern(Pattern.MaxPatterns))
        {
            case Pattern.Single: return SpawnSingle();
            case Pattern.Triple: return SpawnTriple();
            default: return new GameObject[] { };
        }
    }

    private GameObject[] SpawnSingle()
    {
        GameObject Alien = SpawnInState(m_AlienPrefab);

        float XRange = RenderingService.Instance.TargetSize.x * 0.5f;
        Alien.transform.position = new Vector3(
            Random.Range(-XRange, XRange),
            RenderingService.Instance.TargetCenter.y + RenderingService.Instance.TargetSize.y * 0.6f,
            0f
        );

        return new GameObject[] { Alien };
    }

    private GameObject[] SpawnTriple()
    {
        const int NumAliens = 3;
        const float SpaceBetweenAliens = 0.5f;

        // Spawn aliens
        GameObject[] Aliens = new GameObject[NumAliens];
        for (int i = 0; i < NumAliens; ++i)
        {
            Aliens[i] = SpawnInState(m_AlienPrefab);
        }

        // Set up config
        Vector3 AlienSize = Aliens[0].GetComponent<BoxCollider2D>().bounds.size;
        Vector3 FirstPosition;

        float XDiff = 0f;
        float YDiff = 0f;

        switch (m_Config.Pattern.GetSubpattern(TripleSubpattern.MaxPatterns))
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

        if (!m_Config.GetSpawnPosition(out FirstPosition, XDiff * NumAliens))
        {
            FirstPosition = RenderingService.Instance.CenterTop;
            FirstPosition.y += RenderingService.Instance.TargetSize.y * 0.1f;
        }

        // Set positions
        Aliens[0].transform.position = FirstPosition;
        Aliens[1].transform.position = new Vector3(FirstPosition.x + XDiff, FirstPosition.y + YDiff);
        Aliens[2].transform.position = new Vector3(FirstPosition.x - XDiff, FirstPosition.y - YDiff);

        return Aliens;
    }
}
