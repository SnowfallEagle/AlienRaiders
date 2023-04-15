using UnityEngine;

// @TODO: Levels should choose cloud color and sprites for this stuff

public class MovingBackground : CustomBehavior
{
    /** Backgrounds */
    private const float BackgroundScale = 3f;

    [Header("Backgrounds")]
    [SerializeField] private float m_BackgroundVelocityY = 1f;
    [SerializeField] private float m_MovingEffectVelocityY = 5f;

    [SerializeField] private Sprite m_BackgroundSpriteOver;
    [SerializeField] private Sprite m_BackgroundSpriteUnder;

    private GameObject m_BackgroundOver;
    private GameObject m_BackgroundUnder;
    private Vector3 m_BackgroundSize;

    /** Clouds */
    [Header("Clouds")]
    [SerializeField] private GameObject m_CloudsPrefab;
    private GameObject m_Clouds;

    /** Moving Effects */
    [Header("Moving Effects")]
    private const int MaxMovingEffects = 8;
    private const int MaxMovingEffectsOnSide = MaxMovingEffects / 2;

    [SerializeField] private GameObject m_MovingEffectPrefab;
    private GameObject[] m_MovingEffects = new GameObject[MaxMovingEffects];

    private Vector3 m_MovingEffectSize;
    private float m_MovingEffectThreshold;
    private float m_MovingEffectStartY;

    /** Stars */
    struct Star
    {
        public SpriteRenderer SpriteRenderer;
        public bool bFadeOut;
    }

    private const int MaxStars = 64;
    private const int MaxNearStars = MaxStars / 4;
    private const int MaxFarStars = MaxStars - MaxNearStars;

    private const float MinStarAlpha = 0f;
    private const float MaxStarAlpha = 0.9f;

    private static Vector3 FarStarScale = new Vector3(0.1f, 0.1f, 1f);
    private static Vector3 NearStarScale = new Vector3(0.25f, 0.25f, 1f);

    [Header("Stars")]
    [SerializeField] private Sprite m_StarSprite;
    private Star[] m_Stars = new Star[MaxStars];

    [SerializeField] private float m_FarStarVelocityY = 0.5f;
    [SerializeField] private float m_NearStarVelocityY = 1f;

    [SerializeField] private float m_StarFadingSpeed = 0.1f;

    private void Start()
    {
        Vector3 Position;

        { // Backgrounds
            m_BackgroundOver = new GameObject();
            m_BackgroundUnder = new GameObject();

            m_BackgroundOver.transform.parent = transform;
            m_BackgroundUnder.transform.parent = transform;

            Vector3 Scale = new Vector3(BackgroundScale, BackgroundScale, BackgroundScale);
            m_BackgroundOver.transform.localScale = Scale;
            m_BackgroundUnder.transform.localScale = Scale;

            var SpriteRenderer = m_BackgroundOver.AddComponent<SpriteRenderer>().sprite = m_BackgroundSpriteOver;
            m_BackgroundUnder.AddComponent<SpriteRenderer>().sprite = m_BackgroundSpriteUnder;
            m_BackgroundSize = SpriteRenderer.bounds.size * BackgroundScale;

            Position = new Vector3(0f, 0f, WorldZLayers.BackgroundSprite);
            m_BackgroundUnder.transform.position = Position;

            Position.y += m_BackgroundSize.y;
            m_BackgroundOver.transform.position = Position;
        }

        { // Clouds
            m_Clouds = SpawnInState(m_CloudsPrefab);

            Position.y -= m_BackgroundSize.y * 0.5f;
            Position.z = WorldZLayers.BackgroundClouds;
            m_Clouds.transform.position = Position;
        }

        { // Moving effects
            m_MovingEffectSize = m_MovingEffectPrefab.GetComponent<BoxCollider2D>().size;
            m_MovingEffectThreshold = RenderingService.Instance.TargetSize.y;
            m_MovingEffectStartY = RenderingService.Instance.CenterTop.y + (m_MovingEffectSize.y * 0.55f);

            Vector3 EffectPositionLeft = new Vector3(
                -(RenderingService.Instance.TargetSize.x * 0.05f) - m_MovingEffectSize.x,
                m_MovingEffectStartY,
                WorldZLayers.BackgroundEffect
            );
            Vector3 EffectPositionRight = EffectPositionLeft;
            EffectPositionRight.x = -EffectPositionRight.x;

            int i;
            for (i = 0; i < MaxMovingEffectsOnSide; ++i)
            {
                m_MovingEffects[i] = SpawnInState(m_MovingEffectPrefab);
                m_MovingEffects[i].transform.position = EffectPositionLeft;
                EffectPositionLeft.y -= m_MovingEffectSize.y;
            }

            for ( ; i < MaxMovingEffects; ++i)
            {
                m_MovingEffects[i] = SpawnInState(m_MovingEffectPrefab);
                m_MovingEffects[i].transform.position = EffectPositionRight;
                EffectPositionRight.y -= m_MovingEffectSize.y;
            }
        }

        { // Stars
            var StarPrefab = new GameObject();
            StarPrefab.name = "Star";
            StarPrefab.AddComponent<SpriteRenderer>().sprite = m_StarSprite;

            for (int i = 0; i < MaxStars; ++i)
            {
                m_Stars[i].SpriteRenderer = SpawnInState(StarPrefab).GetComponent<SpriteRenderer>();
                RespawnStar(i);

                Vector3 StarPosition = m_Stars[i].SpriteRenderer.transform.position;
                StarPosition.y -= Random.Range(0f, RenderingService.Instance.TargetSize.y);
                StarPosition.z = WorldZLayers.BackgroundEffect;
                m_Stars[i].SpriteRenderer.transform.position = StarPosition;

                m_Stars[i].SpriteRenderer.transform.localScale = i < MaxNearStars ? NearStarScale : FarStarScale;
            }

            Destroy(StarPrefab);
        }
    }

    private void Update()
    {
        { // Swap backgrounds and move clouds
            Vector3 Diff = new Vector3(0f, -m_BackgroundVelocityY * Time.deltaTime, 0f);

            Vector3 OverPosition = m_BackgroundOver.transform.position += Diff;
            m_BackgroundUnder.transform.position += Diff;
            m_Clouds.transform.position += Diff;

            if (OverPosition.y < 0f)
            {
                Vector3 NewUnderPosition = m_BackgroundUnder.transform.position;
                NewUnderPosition.y = OverPosition.y + m_BackgroundSize.y;
                m_BackgroundUnder.transform.position = NewUnderPosition;

                GameObject Temp = m_BackgroundUnder;
                m_BackgroundUnder = m_BackgroundOver;
                m_BackgroundOver = Temp;

                Vector3 CloudsPosition = m_Clouds.transform.position;
                CloudsPosition.y = NewUnderPosition.y - (m_BackgroundSize.y * 0.5f);
                m_Clouds.transform.position = CloudsPosition;
            }
        }

        { // Move background a bit
            if (PlayerState.Instance.PlayerShip is PlayerShip Ship && Ship)
            {
                Vector3 Position = new Vector3(
                    -(Ship.transform.position.x / (RenderingService.Instance.TargetSize.x * 0.5f) * 0.1f),
                    Ship.transform.position.y / (RenderingService.Instance.TargetSize.y * 0.5f) * 0.1f,
                    WorldZLayers.BackgroundSprite
                );
                transform.position = Position;
            }
        }

        { // Moving effects
            float YDiff = -m_MovingEffectVelocityY * Time.deltaTime;

            for (int i = 0; i < MaxMovingEffects; ++i)
            {
                Vector3 Position = m_MovingEffects[i].transform.position;
                Position.y += YDiff; // @TODO: Maybe make it a bit random?

                if (Position.y < -m_MovingEffectThreshold)
                {
                    Position.y = m_MovingEffectStartY + (Position.y + m_MovingEffectThreshold);
                }

                m_MovingEffects[i].transform.position = Position;
            }
        }

        { // Stars
            Vector3 NearDiff = new Vector3(0f, -m_NearStarVelocityY * Time.deltaTime);
            Vector3 FarDiff = new Vector3(0f, -m_FarStarVelocityY * Time.deltaTime);

            for (int i = 0; i < MaxStars; ++i)
            {
                Vector3 Position = m_Stars[i].SpriteRenderer.transform.position += (i < MaxNearStars ? NearDiff : FarDiff);
                if (Position.y < -(RenderingService.Instance.TargetSize.y * 0.6f))
                {
                    RespawnStar(i);
                    continue;
                }

                Color Color = m_Stars[i].SpriteRenderer.color;
                if (m_Stars[i].bFadeOut)
                {
                    Color.a -= m_StarFadingSpeed * Time.deltaTime;

                    if (Color.a <= 0f)
                    {
                        RespawnStar(i);
                        continue;
                    }
                }
                else
                {
                    Color.a += System.MathF.Min(m_StarFadingSpeed * Time.deltaTime, MaxStarAlpha);
                }

                m_Stars[i].SpriteRenderer.color = Color;
            }
        }
    }

    private void RespawnStar(int Idx)
    {
        Vector3 Position = m_Stars[Idx].SpriteRenderer.transform.position;
        Position.x = RenderingService.Instance.CenterTop.x + Random.Range(-RenderingService.Instance.TargetSize.x, RenderingService.Instance.TargetSize.x);
        Position.y = RenderingService.Instance.CenterTop.y + RenderingService.Instance.TargetSize.y * 0.1f;
        m_Stars[Idx].SpriteRenderer.transform.position = Position;

        m_Stars[Idx].SpriteRenderer.color = new Color(Random.Range(0.25f, 1f), Random.Range(0f, 0.25f), Random.Range(0.25f, 1f), Random.Range(MinStarAlpha, MaxStarAlpha));
        m_Stars[Idx].bFadeOut = Random.Range(0, 2) == 1 ? true : false;
    }
}
