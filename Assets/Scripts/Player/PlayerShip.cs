using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShip : Ship
{
    public override Team ShipTeam { get => Team.Player; }
}
