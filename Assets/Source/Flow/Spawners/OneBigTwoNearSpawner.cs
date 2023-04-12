using UnityEngine;
using UnityEngine.Assertions;

public class OneBigTwoNearSpawner : Spawner
{
    public class Config : SpawnerConfig
    {
        public string BigResourcePath = "Ships/BigAlien";
        public string NearResourcePath = "Ships/Alien";
    }

    protected override GameObject[] OnSpawn(SpawnerConfig BaseConfig)
    {
        const int NumAliens = 3;
        const float SpaceBetweenAliens = 1f;

        Config Config = (Config)BaseConfig;

        var BigPrefab = Resources.Load<GameObject>(Config.BigResourcePath);
        var NearPrefab = Resources.Load<GameObject>(Config.NearResourcePath);

        BigPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        NearPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        Vector3 NearSize = NearPrefab.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 BigSize = BigPrefab.GetComponent<SpriteRenderer>().bounds.size;

        GameObject[] Aliens = new GameObject[NumAliens]
        {
            SpawnInState(BigPrefab),
            SpawnInState(NearPrefab),
            SpawnInState(NearPrefab)
        };

        float GroupWidth = (NearSize.x + SpaceBetweenAliens) * 2 + BigSize.x;
        float GroupHeight = BigSize.y;

        Vector3 Position;
        if (!Config.GetSpawnPosition(out Position, GroupWidth, GroupHeight))
        {
            Config.GetSpawnPosition(out Position, SpawnerConfig.AlignType.Center, GroupWidth, GroupHeight);
        }

        Vector3 NearPositionDiff = Vector3.zero;
        NearPositionDiff.x = NearSize.x + SpaceBetweenAliens;

        Aliens[0].transform.position = Position;
        Aliens[1].transform.position = Position + NearPositionDiff;
        Aliens[2].transform.position = Position - NearPositionDiff;

        return Aliens;
    }
}
