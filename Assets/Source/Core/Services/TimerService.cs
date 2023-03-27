using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerService : Service<TimerService>
{
    public class Handle
    {
        public Timer Timer;

        public UnityEngine.Object Owner;
        public bool bOwned;

        public bool bValid => (!bOwned || Owner) && Timer != null;

        public void Invalidate()
        {
            TimerService.Instance.RemoveTimer(this);
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
            Timer Timer = m_Timers[i];

            // Check if we lost reference
            if (!Timer.Handle.bValid)
            {
                RemoveTimer(Timer.Handle);
                continue;
            }

            // Process delay
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

            // Process time left to fire
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

    /** Add timer that fire at time rate.
        Attach Timer to Handle with ownership if Handle and Owner != null.
        Set bLoop = true to loop timer.
        If FirstDelay < 0f and bLoop = true then first time timer fires immediately.
        With delay timer can fire only on second Update() after adding.
    */
    public void AddTimer(Handle Handle, UnityEngine.Object Owner, Action Callback, float TimeRate, bool bLoop = false, float FirstDelay = -1f)
    {
        // Check given handle
        if (Handle != null)
        {
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

        Handle.Owner = Owner;
        Handle.bOwned = Owner ? true : false;
    }

    /** Add timer without ownership.
        Dangerous! Timer can call Action with this == null.
    */
    public void AddTimer(Action Callback, float TimeRate, bool bLoop = false, float FirstDelay = -1f)
    {
        AddTimer(null, null, Callback, TimeRate, bLoop, FirstDelay);
    }

    public void RemoveTimer(Handle Handle)
    {
        if (Handle.bValid)
        {
            m_Timers.Remove(Handle.Timer);

            Handle.Timer = null;
            Handle.Owner = null;
            Handle.bOwned = false;
        }
    }
}