using UnityEngine;
using UnityEngine.Assertions;

public class PatternSpawnerConfig
{
    public int SpecificSpawnPattern = SpawnerConfig.AnyValue;
    public int FromSpawnPattern     = SpawnerConfig.AnyValue;
    public int ToSpawnPattern       = SpawnerConfig.AnyValue;

    public int SpecificSpawnSubpattern = SpawnerConfig.AnyValue;
    public int FromSpawnSubpattern     = SpawnerConfig.AnyValue;
    public int ToSpawnSubpattern       = SpawnerConfig.AnyValue;

    public int GetPattern(int MaxPatterns)
    {
        if (SpecificSpawnPattern != SpawnerConfig.AnyValue)
        {
            return SpecificSpawnPattern;
        }

        int From = FromSpawnPattern != SpawnerConfig.AnyValue ? FromSpawnPattern : 0;
        int To = ToSpawnPattern != SpawnerConfig.AnyValue ? ToSpawnPattern + 1 : MaxPatterns;

        return Random.Range(From, To);
    }

    public int GetSubpattern(int MaxPatterns)
    {
        if (SpecificSpawnSubpattern != SpawnerConfig.AnyValue && SpecificSpawnPattern != SpawnerConfig.AnyValue)
        {
            return SpecificSpawnSubpattern;
        }

        int From = FromSpawnSubpattern != SpawnerConfig.AnyValue ? FromSpawnSubpattern : 0;
        int To = ToSpawnSubpattern != SpawnerConfig.AnyValue ? ToSpawnSubpattern + 1 : MaxPatterns;

        return Random.Range(From, To);
    }

    public int GetShipPattern(int MaxPatterns)
    {
        // @INCOMPLETE
        NotImplemented.Assert();
        return 0;
    }
}
