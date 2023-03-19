using UnityEngine;

public class FightStage : CustomBehavior
{
    private void Start()
    {
        var RenderingService = ServiceLocator.Instance.Get<RenderingService>();
        Vector3 TargetSize = RenderingService.TargetSize;
        Vector3 TargetCenter = RenderingService.TargetCenter;

        var Alien = Resources.Load<GameObject>("Ships/Alien");

        SpawnInState(Alien);

        float XRange = TargetSize.x * 0.5f;
        Vector3 SpawnPosition = new Vector3(
            Random.Range(-XRange, XRange),
            TargetCenter.y + TargetSize.y * 0.6f,
            0f
        );

        Alien.transform.position = SpawnPosition;
        Alien.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

#if null
        ServiceLocator.Instance.Get<TimerService>().AddTimer(() =>
            {
                SpawnInState(Alien);

                float XRange = TargetSize.x * 0.5f;
                Vector3 SpawnPosition = new Vector3(
                    Random.Range(-XRange, XRange),
                    TargetCenter.y + TargetSize.y * 0.6f,
                    0f
                );

                Alien.transform.position = SpawnPosition;
                Alien.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            },
            2f,
            true
        );
#endif
    }

    private void Update()
    {
    }
}
