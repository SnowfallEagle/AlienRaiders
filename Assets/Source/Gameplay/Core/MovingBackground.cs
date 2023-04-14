using UnityEngine;

// @TODO: Levels should choose cloud color and sprites for this stuff
// @TODO: Stars?

public class MovingBackground : CustomBehavior
{
    /** Backgrounds */
    private const float BackgroundScale = 3f;

    [SerializeField] protected float m_BackgroundVelocityY = 1f;
    [SerializeField] protected float m_MovingEffectVelocityY = 5f;

    [SerializeField] protected Sprite m_BackgroundSpriteOver;
    [SerializeField] protected Sprite m_BackgroundSpriteUnder;

    private GameObject m_BackgroundOver;
    private GameObject m_BackgroundUnder;
    private Vector3 m_BackgroundSize;

    /** Clouds */
    [SerializeField] protected GameObject m_CloudsPrefab;
    private GameObject m_Clouds;

    /** Moving Effects */
    private const int MaxMovingEffects = 8;
    private const int MaxMovingEffectsOnSide = MaxMovingEffects / 2;

    [SerializeField] protected GameObject m_MovingEffectPrefab;
    private GameObject[] m_MovingEffects = new GameObject[MaxMovingEffects];

    private Vector3 m_MovingEffectSize;
    private float m_MovingEffectThreshold;
    private float m_MovingEffectStartY;

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
            Position.z = WorldZLayers.BackgroundEffect;
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
                    -(Ship.transform.position.x / (RenderingService.Instance.TargetSize.x * 0.5f) * 0.05f),
                    Ship.transform.position.y / (RenderingService.Instance.TargetSize.y * 0.5f) * 0.05f,
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
    }
}
