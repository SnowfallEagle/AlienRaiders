using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : Ship
{
    public override Team Team { get => Team.Enemy; }
}
