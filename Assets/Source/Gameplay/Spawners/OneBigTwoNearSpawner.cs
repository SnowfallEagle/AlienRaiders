using UnityEngine;
using UnityEngine.Assertions;

public class OneBigTwoNearSpawner : Spawner
{
    protected override GameObject[] OnSpawn()
    {
        const int NumAliens = 3;
        const float SpaceBetweenAliens = 1.5f;

        var BigPrefab = Resources.Load<GameObject>("Ships/BigAlien"); // @INCOMPLETE: m_Config.StringValues["BigResourcePath"]
        var NearPrefab = Resources.Load<GameObject>("Ships/Alien");   // @INCOMPLETE: m_Config.StringValues["NearResourcePath"]

        BigPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        NearPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        Vector3 BigSize = BigPrefab.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 BigHalfSize = BigPrefab.GetComponent<SpriteRenderer>().bounds.size * 0.5f;

        Vector3 NearSize = NearPrefab.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 NearHalfSize = NearPrefab.GetComponent<SpriteRenderer>().bounds.size * 0.5f;

        GameObject[] Aliens = new GameObject[NumAliens]
        {
            SpawnInState(BigPrefab),
            SpawnInState(NearPrefab),
            SpawnInState(NearPrefab)
        };

        Vector3 Position = s_Precomputed.CenterTop;
        Position.y += s_Precomputed.TargetSize.y * 0.1f;

        Aliens[1].transform.position = Position;

        Position.x += NearSize.x + SpaceBetweenAliens;
        Aliens[0].transform.position = Position;

        Position.x += NearSize.x + SpaceBetweenAliens;
        Aliens[2].transform.position = Position;

        return Aliens;
    }
}
