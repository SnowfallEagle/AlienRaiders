using UnityEngine;
using UnityEngine.Assertions;

public class AlienSpawner : Spawner
{
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

    protected override GameObject[] OnSpawn()
    {
        string ResourcePath = m_Config.ResourcePath != null ? m_Config.ResourcePath : "Ships/Alien";

        m_AlienPrefab = Resources.Load<GameObject>(ResourcePath);
        Assert.IsNotNull(m_AlienPrefab);

        m_AlienPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        switch (GetPattern(Pattern.MaxPatterns))
        {
            case Pattern.Single: return SpawnSingle();
            case Pattern.Triple: return SpawnTriple();
            default: return new GameObject[] { };
        }
    }

    private GameObject[] SpawnSingle()
    {
        GameObject Alien = SpawnInState(m_AlienPrefab);

        float XRange = s_Precomputed.TargetSize.x * 0.5f;
        Alien.transform.position = new Vector3(
            Random.Range(-XRange, XRange),
            s_Precomputed.TargetCenter.y + s_Precomputed.TargetSize.y * 0.6f,
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
        Vector3 FirstPosition = new Vector3(
            s_Precomputed.TargetCenter.x - s_Precomputed.TargetSize.x * 0.5f,
            s_Precomputed.TargetCenter.y + s_Precomputed.TargetSize.y * 0.6f,
            0f
        );

        float XDiff = 0f;
        float YDiff = 0f;

        switch (GetSubpattern(TripleSubpattern.MaxPatterns))
        {
            case TripleSubpattern.Right:
                XDiff = AlienSize.x + SpaceBetweenAliens;
                YDiff = AlienSize.y + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(0f, s_Precomputed.TargetSize.x - XDiff * NumAliens);
                break;

            case TripleSubpattern.Left:
                XDiff = -(AlienSize.x + SpaceBetweenAliens);
                YDiff = AlienSize.y + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(-XDiff * (NumAliens - 1), s_Precomputed.TargetSize.x + XDiff * NumAliens);
                break;

            case TripleSubpattern.Column:
                XDiff = AlienSize.x + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(0f, s_Precomputed.TargetSize.x - AlienSize.x * (NumAliens - 1));
                break;

            case TripleSubpattern.Row:
                YDiff = AlienSize.y + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(0f, s_Precomputed.TargetSize.x - AlienSize.x);
                break;
        }

        // Set position
        for (int i = 0; i < NumAliens; ++i)
        {
            Aliens[i].transform.position = new Vector3(
                FirstPosition.x + XDiff * i,
                FirstPosition.y + YDiff * i,
                0f
            );
        }

        return Aliens;
    }
}
