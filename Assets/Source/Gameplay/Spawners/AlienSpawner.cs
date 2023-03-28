using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawner : Spawner
{
    public enum Pattern
    {
        Single,
        Triple,

        MaxPatterns
    }

    public enum TripleSubpattern
    {
        Left,
        Right,
        Row,
        Column,

        MaxPatterns
    }

    private static GameObject s_AlienPrefab;

    protected override GameObject[] OnSpawn()
    {
        if (!s_AlienPrefab)
        {
            // DEBUG
            // s_AlienPrefab = Resources.Load<GameObject>("Ships/Alien");
            s_AlienPrefab = Resources.Load<GameObject>("Ships/Rocketer");
            s_AlienPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        switch (GetPattern((int)Pattern.MaxPatterns))
        {
            case (int)Pattern.Single: return SpawnSingle();
            case (int)Pattern.Triple: return SpawnTriple();
            default: return new GameObject[] { };
        }
    }

    private GameObject[] SpawnSingle()
    {
        GameObject Alien = SpawnInState(s_AlienPrefab);

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
            Aliens[i] = SpawnInState(s_AlienPrefab);
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

        switch (GetSubpattern((int)TripleSubpattern.MaxPatterns))
        {
            case (int)TripleSubpattern.Right:
                XDiff = AlienSize.x + SpaceBetweenAliens;
                YDiff = AlienSize.y + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(0f, s_Precomputed.TargetSize.x - XDiff * NumAliens);
                break;

            case (int)TripleSubpattern.Left:
                XDiff = -(AlienSize.x + SpaceBetweenAliens);
                YDiff = AlienSize.y + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(-XDiff * (NumAliens - 1), s_Precomputed.TargetSize.x + XDiff * NumAliens);
                break;

            case (int)TripleSubpattern.Column:
                XDiff = AlienSize.x + SpaceBetweenAliens;

                FirstPosition.x += Random.Range(0f, s_Precomputed.TargetSize.x - AlienSize.x * (NumAliens - 1));
                break;

            case (int)TripleSubpattern.Row:
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
