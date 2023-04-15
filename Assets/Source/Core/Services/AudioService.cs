using UnityEngine;

public class AudioService : Service<AudioService>
{
    private bool m_bMuted = false;
    public bool bMuted => m_bMuted;

    public void Mute(bool bToggle)
    {
        AudioListener.pause = bToggle;
        AudioListener.volume = bToggle ? 0f : 1f;
        m_bMuted = bToggle;
    }
}
