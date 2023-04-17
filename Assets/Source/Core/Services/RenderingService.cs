using UnityEngine;
using UnityEngine.Assertions;

public class RenderingService : Service<RenderingService>
{
    [Header("Camera")]
    [SerializeField] private float m_ZNearClip = 0.1f;
    [SerializeField] private float m_ZFarClip = 2000f;

    // If null then use TargetSize and TargetCenter
    [Header("TargetSize")]

    [SerializeField] private SpriteRenderer m_TargetSizeSprite;

    [SerializeField] protected Vector3 m_TargetSize = new Vector3(11.5f, 20f);
    public Vector3 TargetSize => m_TargetSizeSprite ? m_TargetSizeSprite.bounds.size : m_TargetSize;

    [SerializeField] protected Vector3 m_TargetCenter = Vector3.zero;
    public Vector3 TargetCenter => m_TargetSizeSprite ? m_TargetSizeSprite.bounds.center : m_TargetCenter;

    private Vector3 m_LeftTop;
    public Vector3 LeftTop => m_LeftTop;

    private Vector3 m_RightTop;
    public Vector3 RightTop => m_RightTop;

    private Vector3 m_CenterTop;
    public Vector3 CenterTop => m_CenterTop;

    /** Level */
    private LevelAppearance m_Appearance;

    /** Backgrounds */
    private const float BackgroundScale = 3f;

    [Header("Backgrounds")]
    [SerializeField] private float m_BackgroundVelocityY = 1f;
    [SerializeField] private float m_MovingEffectVelocityY = 35f;

    private GameObject m_BackgroundRoot;

    private SpriteRenderer m_BackgroundOver;
    private SpriteRenderer m_BackgroundUnder;
    private Vector3 m_BackgroundSize;

    /** Clouds */
    [Header("Clouds")]
    [SerializeField] private GameObject m_CloudsPrefab;
    private GameObject m_Clouds;
    private SpriteRenderer[] m_CloudSprites;

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

    [SerializeField] private float m_FarStarVelocityY = 1f;
    [SerializeField] private float m_NearStarVelocityY = 2f;

    [SerializeField] private float m_StarFadingSpeed = 0.05f;

    public RenderingService()
    {
        m_LeftTop = TargetCenter;
        m_LeftTop.x -= TargetSize.x * 0.5f;
        m_LeftTop.y += TargetSize.y * 0.5f;

        m_RightTop = TargetCenter + TargetSize * 0.5f;

        m_CenterTop = TargetCenter;
        m_CenterTop.y += TargetSize.y * 0.5f;
    }

    private void Start()
    {
        StartCamera();
        StartMovingBackground();
    }

    private void StartCamera()
    {
        Camera.main.nearClipPlane = m_ZNearClip;
        Camera.main.farClipPlane = m_ZFarClip;

        Vector3 Position = Camera.main.transform.position;
        Position.z = WorldZLayers.Camera;
        Camera.main.transform.position = Position;
    }

    private void StartMovingBackground()
    {
        UpdateAppearance();

        m_BackgroundRoot = new GameObject();
        m_BackgroundRoot.name = "BackgroudRoot";
        m_BackgroundRoot.transform.parent = transform;

        Vector3 Position;
        { // Backgrounds
            m_BackgroundOver = new GameObject().AddComponent<SpriteRenderer>();
            m_BackgroundOver.name = "Background1";

            m_BackgroundOver.transform.parent = m_BackgroundRoot.transform;
            m_BackgroundOver.transform.localScale = new Vector3(BackgroundScale, BackgroundScale, BackgroundScale);

            m_BackgroundUnder = Instantiate(m_BackgroundOver);
            m_BackgroundUnder.name = "Background2";
            m_BackgroundUnder.transform.parent = m_BackgroundRoot.transform;

            m_BackgroundOver.sprite  = Resources.Load<Sprite>(m_Appearance.BackgroundOver);
            m_BackgroundUnder.sprite = Resources.Load<Sprite>(m_Appearance.BackgroundUnder);
            m_BackgroundSize = m_BackgroundOver.bounds.size;

            Position = new Vector3(0f, 0f, WorldZLayers.BackgroundSprite);
            m_BackgroundUnder.transform.position = Position;

            Position.y += m_BackgroundSize.y;
            m_BackgroundOver.transform.position = Position;
        }

        { // Clouds
            m_Clouds = Instantiate(m_CloudsPrefab);
            m_Clouds.transform.parent = m_BackgroundRoot.transform;

            Position.y -= m_BackgroundSize.y * 0.5f;
            Position.z = WorldZLayers.BackgroundClouds;
            m_Clouds.transform.position = Position;

            m_CloudSprites = m_Clouds.GetComponentsInChildren<SpriteRenderer>();
            foreach (var SpriteRenderer in m_CloudSprites)
            {
                SpriteRenderer.color = m_Appearance.CloudsColor;
            }
        }

        { // Moving effects
            m_MovingEffectSize      = m_MovingEffectPrefab.GetComponent<BoxCollider2D>().size;
            m_MovingEffectThreshold = TargetSize.y;
            m_MovingEffectStartY    = CenterTop.y + (m_MovingEffectSize.y * 0.55f);

            Vector3 EffectPositionLeft = new Vector3(
                -(TargetSize.x * 0.05f) - m_MovingEffectSize.x,
                m_MovingEffectStartY,
                WorldZLayers.BackgroundEffect
            );
            Vector3 EffectPositionRight = EffectPositionLeft;
            EffectPositionRight.x = -EffectPositionRight.x;

            for (int i = 0; i < MaxMovingEffects; ++i)
            {
                m_MovingEffects[i] = Instantiate(m_MovingEffectPrefab);
                m_MovingEffects[i].transform.parent = transform;

                if (i < MaxMovingEffectsOnSide)
                {
                    m_MovingEffects[i].transform.position = EffectPositionLeft;
                    EffectPositionLeft.y -= m_MovingEffectSize.y;
                }
                else
                {
                    m_MovingEffects[i].transform.position = EffectPositionRight;
                    EffectPositionRight.y -= m_MovingEffectSize.y;
                }
            }
        }

        { // Stars
            var StarPrefab = new GameObject();
            StarPrefab.name = "Star";
            StarPrefab.AddComponent<SpriteRenderer>().sprite = m_StarSprite;

            for (int i = 0; i < MaxStars; ++i)
            {
                m_Stars[i].SpriteRenderer = Instantiate(StarPrefab).GetComponent<SpriteRenderer>();
                m_Stars[i].SpriteRenderer.transform.parent = i < MaxNearStars ? m_BackgroundRoot.transform : transform;
                RespawnStar(i);

                Vector3 StarPosition = m_Stars[i].SpriteRenderer.transform.position;
                StarPosition.y -= Random.Range(0f, TargetSize.y);
                StarPosition.z = WorldZLayers.BackgroundEffect;
                m_Stars[i].SpriteRenderer.transform.position = StarPosition;

                m_Stars[i].SpriteRenderer.transform.localScale = i < MaxNearStars ? NearStarScale : FarStarScale;
            }

            Destroy(StarPrefab);
        }
    }

    private void Update()
    {
        UpdateScreenRatio();
        UpdateMovingBackground();
    }

    public void UpdateAppearance(int LevelIdx = -1)
    {
        bool bFirstUpdate = m_Appearance == null;

        if (LevelIdx == -1)
        {
            LevelIdx = PlayerState.Instance.Level;
        }

        LevelAppearance NewAppearance = FightGameState.s_Levels[LevelIdx].Appearance;
        if (!bFirstUpdate && m_Appearance == NewAppearance)
        {
            return;
        }

        m_Appearance = NewAppearance;
        if (bFirstUpdate)
        {
            return;
        }

        m_BackgroundOver.sprite = Resources.Load<Sprite>(m_Appearance.BackgroundOver);
        m_BackgroundUnder.sprite = Resources.Load<Sprite>(m_Appearance.BackgroundUnder);

        foreach (var SpriteRenderer in m_CloudSprites)
        {
            SpriteRenderer.color = m_Appearance.CloudsColor;
        }
    }

    private void UpdateScreenRatio()
    {
        Vector3 CurrentTargetSize = TargetSize;

        float ScreenRatio = (float)Screen.width / (float)Screen.height;
        float TargetRatio = CurrentTargetSize.x / CurrentTargetSize.y;
        float TargetYDivTwo = CurrentTargetSize.y * 0.5f;

        if (ScreenRatio >= TargetRatio)
        {
            Camera.main.orthographicSize = TargetYDivTwo;
        }
        else
        {
            /*
                If TargetRatio > ScreenRatio then we need to expand width of target,
                so we can get at least target's needed with and more than target's height.

                We multiply target's height by ratio of target and screen.

                Example:
                    Screen Size: 800x600
                    Target Size: 1280x720

                    Screen Ratio: 1.(3)
                    Target Ratio: 1.(7)
                    Target Ratio / Screen Ratio: 1.(3)

                    Multiply (Target Height / 2) by Target Screen Ratio:
                    360 * 1.(3) = 480

                    So we get at least target width and more than target height.
            */

            float YScale = TargetRatio / ScreenRatio;
            Camera.main.orthographicSize = TargetYDivTwo * YScale;
        }
    }

    private void UpdateMovingBackground()
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

                SpriteRenderer Temp = m_BackgroundUnder;
                m_BackgroundUnder = m_BackgroundOver;
                m_BackgroundOver = Temp;

                Vector3 CloudsPosition = m_Clouds.transform.position;
                CloudsPosition.y = NewUnderPosition.y - (m_BackgroundSize.y * 0.5f);
                m_Clouds.transform.position = CloudsPosition;
            }
        }

        { // Move background a bit
            if (PlayerState.Instance.PlayerShip is PlayerShip Ship)
            {
                Vector3 Position = new Vector3(
                    -(Ship.transform.position.x / (TargetSize.x * 0.5f) * 0.1f),
                    Ship.transform.position.y / (TargetSize.y * 0.5f) * 0.1f,
                    WorldZLayers.BackgroundSprite
                );
                m_BackgroundRoot.transform.position = Position;
            }
        }

        { // Moving effects
            float YDiff = -m_MovingEffectVelocityY * Time.deltaTime;

            for (int i = 0; i < MaxMovingEffects; ++i)
            {
                Vector3 Position = m_MovingEffects[i].transform.position;
                Position.y += YDiff;

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
                if (Position.y < -(TargetSize.y * 0.6f))
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
        Position.x = CenterTop.x + Random.Range(-TargetSize.x, TargetSize.x);
        Position.y = CenterTop.y + TargetSize.y * 0.1f;
        m_Stars[Idx].SpriteRenderer.transform.position = Position;

        m_Stars[Idx].SpriteRenderer.color = new Color(Random.Range(0.25f, 1f), Random.Range(0f, 0.25f), Random.Range(0.25f, 1f), Random.Range(MinStarAlpha, MaxStarAlpha));
        m_Stars[Idx].bFadeOut = Random.Range(0, 2) == 1 ? true : false;
    }
}
