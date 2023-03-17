using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerService : CustomBehaviour
{
    private class Timer
    {
        public float TimeRate;
        public float TimeLeftToFire;

        public float FirstDelay;
        public bool bNeedDelay;
        public bool bLoop;

        public Action Callback;
    }

    public struct Handle
    {
        public int TimerIndex;

        public Handle(int InTimerIndex = -1)
        {
            TimerIndex = InTimerIndex;
        }
    }

    private List<Timer> m_Timers = new List<Timer>();

    private void Update()
    {
        for (int i = m_Timers.Count - 1; i >= 0; --i)
        {
            var Timer = m_Timers[i];

            if (Timer.bNeedDelay)
            {
                Timer.FirstDelay -= Time.deltaTime;
                if (Timer.FirstDelay <= 0f)
                {
                    Timer.TimeLeftToFire -= Mathf.Abs(Timer.FirstDelay);
                    Timer.bNeedDelay = false;
                }

                continue;
            }

            Timer.TimeLeftToFire -= Time.deltaTime;
            if (Timer.TimeLeftToFire <= 0f)
            {
                if (Timer.bLoop)
                {
                    float AbsTimeLeftToFire = Mathf.Abs(Timer.TimeLeftToFire);

                    float NextFireTimeRemainder = AbsTimeLeftToFire % Timer.TimeRate;
                    Timer.TimeLeftToFire = Timer.TimeRate - NextFireTimeRemainder;

                    int FireCount = 1 + (int)(AbsTimeLeftToFire / Timer.TimeRate);
                    for (int FireIndex = 0; FireIndex < FireCount; ++FireIndex)
                    {
                        Timer.Callback?.Invoke();
                    }

                    continue;
                }

                Timer.Callback?.Invoke();
                m_Timers.RemoveAt(i);
            }
        }
    }

    /** Add timer that fire at time rate.
        Set bLoop = true to loop timer.
        If FirstDelay < 0f then first time timer fires immediately.
    */
    public Handle AddTimer(Action Callback, float TimeRate, bool bLoop = false, float FirstDelay = -1f)
    {
        m_Timers.Add(new Timer
        {
            TimeRate = TimeRate,
            TimeLeftToFire = FirstDelay >= 0f ? TimeRate : 0f,

            FirstDelay = FirstDelay,
            bNeedDelay = FirstDelay >= 0f,
            bLoop = bLoop,

            Callback = Callback,
        });

        return new Handle(m_Timers.Count - 1);
    }

    public void RemoveTimer(Handle Handle)
    {
        if (Handle.TimerIndex >= 0 && Handle.TimerIndex < m_Timers.Count)
        {
            m_Timers.RemoveAt(Handle.TimerIndex);
        }
    }
}
