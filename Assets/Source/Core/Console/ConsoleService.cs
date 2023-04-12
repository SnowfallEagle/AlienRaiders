using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleCommand
{
    public struct ArgumentInfo
    {
        public string Name;
        public Type Type;
        public object Default;
    }

    public ArgumentInfo[] ArgumentsInfo = new ArgumentInfo[0];

    public abstract void Execute(object[] Args);

    public void LogInfo(string CommandName)
    {
        string Info = ArgumentsInfo.Length > 0 ?
            $"{ CommandName }: " :
            $"{ CommandName }: no arguments";

        foreach (var Argument in ArgumentsInfo)
        {
            Info += Argument.Default != null ?
                $"<{ Argument.Name }: { Argument.Type.Name } = { Argument.Default }>" :
                $"<{ Argument.Name }: { Argument.Type.Name }> ";
        }

        Debug.Log(Info);
    }
}

public class LevelConsoleCommand : ConsoleCommand
{
    public LevelConsoleCommand()
    {
        ArgumentsInfo = new ArgumentInfo[]
        {
            new ArgumentInfo { Name = "name",    Type = typeof(string), Default = "IntroLevel" },
            new ArgumentInfo { Name = "stage",   Type = typeof(int),    Default = 0 },
            new ArgumentInfo { Name = "spawner", Type = typeof(int),    Default = 0 },
        };
    }

    public override void Execute(object[] Args)
    {
        foreach (var Arg in Args)
        {
            Debug.Log(Arg);
        }

        // @INCOMPLETE: Change state on new FightGameState with arguments
    }
}

public class ConsoleService : Service<ConsoleService>
{
    private bool m_bShown = false;

    private string m_Input = "";
    private string m_SavedInputWhileBrowsing = "";
    private string m_InputControlName = "ConsoleInput";

    private const int MaxHistory = 24;
    private string[] m_History = new string[MaxHistory];

    private int m_HistoryAddCursor = 0;
    private int m_HistoryBrowseCursor = 0;

    private Dictionary<string, ConsoleCommand> m_Commands = new Dictionary<string, ConsoleCommand>
    {
        { "level", new LevelConsoleCommand() }
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            m_bShown ^= true;
        }
    }

    private void OnGUI()
    {
        if (!m_bShown)
        {
            return;
        }

        bool bMoveCursorToEnd = false;

        Event Event = Event.current;
        if (Event.type == EventType.KeyDown)
        {
            switch (Event.keyCode)
            {
                case KeyCode.BackQuote:
                    m_bShown ^= true;
                    return;

                case KeyCode.Return:
                    ProcessInput();
                    if (m_Input != "")
                    {
                        PushHistory(m_Input);
                        m_Input = "";
                    }
                    break;

                case KeyCode.UpArrow:
                    int PrevCursor = m_HistoryBrowseCursor - 1;
                    if (PrevCursor < 0)
                    {
                        PrevCursor = MaxHistory - 1;
                    }

                    if (m_History[PrevCursor] != null)
                    {
                        if (m_HistoryBrowseCursor == m_HistoryAddCursor)
                        {
                            m_SavedInputWhileBrowsing = m_Input;
                        }

                        m_Input = m_History[PrevCursor];
                        m_HistoryBrowseCursor = PrevCursor;
                    }

                    bMoveCursorToEnd = true;
                    break;

                case KeyCode.DownArrow:
                    if (m_HistoryBrowseCursor != m_HistoryAddCursor)
                    {
                        if (++m_HistoryBrowseCursor >= MaxHistory)
                        {
                            m_HistoryBrowseCursor = 0;
                        }

                        m_Input = m_HistoryBrowseCursor == m_HistoryAddCursor ?
                            m_SavedInputWhileBrowsing :
                            m_History[m_HistoryBrowseCursor];
                    }
                    break;
            }
        }

        GUI.Box(new Rect(0f, 0f, Screen.width, 30f), "");

        GUI.SetNextControlName(m_InputControlName);
        GUI.backgroundColor = new Color(0f, 0f, 0f, 0f);

        m_Input = GUI.TextField(new Rect(10f, 5f, Screen.width - 20f, 20f), m_Input);
        GUI.FocusControl(m_InputControlName);

        if (bMoveCursorToEnd)
        {
            TextEditor Editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            Editor.MoveTextEnd();
        }
    }

    private void PushHistory(string Input)
    {
        m_History[m_HistoryAddCursor] = Input;
        if (++m_HistoryAddCursor >= MaxHistory)
        {
            m_HistoryAddCursor = 0;
        }

        m_HistoryBrowseCursor = m_HistoryAddCursor;
    }

    private void ProcessInput()
    {
        var InputPieces = m_Input.Split(' ');
        if (InputPieces.Length <= 0)
        {
            return;
        }

        ConsoleCommand Command;
        if (!m_Commands.TryGetValue(InputPieces[0], out Command))
        {
            Debug.LogWarning($"Unknown command: { InputPieces[0] }");
            return;
        }

        int ArgsCount = InputPieces.Length - 1;
        if (ArgsCount != Command.ArgumentsInfo.Length)
        {
            if (ArgsCount > Command.ArgumentsInfo.Length)
            {
                Debug.LogWarning("Too many arguments!");
                Command.LogInfo(InputPieces[0]);
                return;
            }

            if ((ArgsCount <= 0 && Command.ArgumentsInfo[0].Default == null) ||
                (ArgsCount > 0 && Command.ArgumentsInfo[ArgsCount - 1].Default == null))
            {
                Debug.LogWarning("Too few arguments!");
                Command.LogInfo(InputPieces[0]);
                return;
            }
        }

        object[] Args = new object[InputPieces.Length - 1];
        for (int PieceIdx = 1, ArgIdx = 0; PieceIdx < InputPieces.Length; ++PieceIdx, ++ArgIdx)
        {
            Type ArgType = Command.ArgumentsInfo[ArgIdx].Type;

            if (ArgType == typeof(string))
            {
                Args[ArgIdx] = InputPieces[PieceIdx];
            }
            else if (ArgType == typeof(int))
            {
                int Result;
                if (int.TryParse(InputPieces[PieceIdx], out Result))
                {
                    Args[ArgIdx] = Result;
                    continue;
                }

                Debug.LogWarning("Can't parse int!");
                return;
            }
            else if (ArgType == typeof(float))
            {
                float Result;
                if (float.TryParse(InputPieces[PieceIdx], out Result))
                {
                    Args[ArgIdx] = Result;
                    continue;
                }

                Debug.LogWarning("Can't parse float!");
                return;
            }
        }

        Command.Execute(Args);
    }
}
