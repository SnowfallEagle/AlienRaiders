using UnityEngine;
using UnityEngine.Assertions;

/** String values:
    BigResourcePath
    NearResourcePath
*/

public class OneBigTwoNearSpawner : Spawner
{
    protected override GameObject[] OnSpawn()
    {
        const int NumAliens = 3;
        const float SpaceBetweenAliens = 1f;

        string BigResourcePath;
        string NearResourcePath;

        if (!m_Config.StringValues.TryGetValue("BigResourcePath", out BigResourcePath))
        {
            BigResourcePath = "Ships/BigAlien";
        }
        if (!m_Config.StringValues.TryGetValue("NearResourcePath", out NearResourcePath))
        {
            NearResourcePath = "Ships/Alien";
        }

        var BigPrefab = Resources.Load<GameObject>(BigResourcePath);
        var NearPrefab = Resources.Load<GameObject>(NearResourcePath);

        BigPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        NearPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        Vector3 NearSize = NearPrefab.GetComponent<SpriteRenderer>().bounds.size;

        GameObject[] Aliens = new GameObject[NumAliens]
        {
            SpawnInState(BigPrefab),
            SpawnInState(NearPrefab),
            SpawnInState(NearPrefab)
        };

        Vector3 Position = s_Precomputed.CenterTop;
        Position.y += s_Precomputed.TargetSize.y * 0.1f;

        Vector3 NearPositionDiff = Vector3.zero;
        NearPositionDiff.x = NearSize.x + SpaceBetweenAliens;

        Aliens[0].transform.position = Position;
        Aliens[1].transform.position = Position + NearPositionDiff;
        Aliens[2].transform.position = Position - NearPositionDiff;

        return Aliens;
    }
}
