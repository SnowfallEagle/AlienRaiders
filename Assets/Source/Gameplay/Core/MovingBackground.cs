using UnityEngine;

// @TODO: Levels should choose cloud colors and sprites for this stuff
// @TODO: Moving effects

public class MovingBackground : CustomBehavior
{
    private const float BackgroundScale = 3f;

    [SerializeField] protected float YVelocity = 1f;

    [SerializeField] protected Sprite m_BackgroundSpriteOver;
    [SerializeField] protected Sprite m_BackgroundSpriteUnder;

    private GameObject m_BackgroundOver;
    private GameObject m_BackgroundUnder;
    private Vector3 m_BackgroundSize;

    [SerializeField] protected GameObject m_Clouds;

    public void Start()
    {
        m_BackgroundOver = new GameObject();
        m_BackgroundUnder = new GameObject();

        m_BackgroundOver.transform.parent = transform;
        m_BackgroundUnder.transform.parent = transform;

        Vector3 Scale = new Vector3(BackgroundScale, BackgroundScale, BackgroundScale);
        m_BackgroundOver.transform.localScale = Scale;
        m_BackgroundUnder.transform.localScale = Scale;

        var SpriteRenderer1 = m_BackgroundOver.AddComponent<SpriteRenderer>().sprite = m_BackgroundSpriteOver;
        m_BackgroundUnder.AddComponent<SpriteRenderer>().sprite = m_BackgroundSpriteUnder;
        m_BackgroundSize = SpriteRenderer1.bounds.size * BackgroundScale;

        Vector3 Position = new Vector3(0f, 0f, WorldZLayers.BackgroundSprite);
        m_BackgroundUnder.transform.position = Position;

        Position.y += m_BackgroundSize.y;
        m_BackgroundOver.transform.position = Position;

        Position.y -= m_BackgroundSize.y * 0.5f;
        Position.z = WorldZLayers.BackgroundEffect;
        m_Clouds.transform.position = Position;
    }

    public void Update()
    {
        Vector3 Diff = new Vector3(0f, YVelocity * Time.deltaTime, 0f);

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
}
