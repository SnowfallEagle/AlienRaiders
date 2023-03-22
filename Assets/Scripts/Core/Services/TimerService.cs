using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerService : Service<TimerService>
{
    public class Handle
    {
        public Timer Timer;
        public bool bValid => Timer != null;

        public void Invalidate()
        {
            Timer = null;
        }
    }

    public class Timer
    {
        public Handle Handle;

        public float TimeRate;
        public float TimeLeftToFire;

        public float FirstDelay;
        public bool bNeedDelay;
        public bool bLoop;

        public Action Callback;
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
                RemoveTimer(Timer.Handle);
            }
        }
    }

    /** Add timer that fire at time rate, attach Timer to Handle.
        Set bLoop = true to loop timer.
        If FirstDelay < 0f and bLoop = true then first time timer fires immediately.
    */
    public void AddTimer(Handle Handle, Action Callback, float TimeRate, bool bLoop = false, float FirstDelay = -1f)
    {
        if (Handle != null)
        {
            // Try to remove timer if handle is already in use
            RemoveTimer(Handle);
        }
        else
        {
            Handle = new Handle();
        }

        // Add new timer
        m_Timers.Add(new Timer
        {
            TimeRate = TimeRate,
            TimeLeftToFire = bLoop && FirstDelay < 0f ? 0f : TimeRate,

            FirstDelay = FirstDelay,
            bNeedDelay = FirstDelay >= 0f,
            bLoop = bLoop,

            Callback = Callback,
        });

        // Set up timer handle
        Timer Timer = m_Timers[m_Timers.Count - 1];

        Handle.Timer = Timer;
        Timer.Handle = Handle;
    }

    /** Add timer that fire at time rate.
        Set bLoop = true to loop timer.
        If FirstDelay < 0f then first time timer fires immediately.
    */
    public void AddTimer(Action Callback, float TimeRate, bool bLoop = false, float FirstDelay = -1f)
    {
        AddTimer(null, Callback, TimeRate, bLoop, FirstDelay);
    }

    public void RemoveTimer(Handle Handle)
    {
        if (Handle.bValid)
        {
            m_Timers.Remove(Handle.Timer);
            Handle.Invalidate();
        }
    }
}
