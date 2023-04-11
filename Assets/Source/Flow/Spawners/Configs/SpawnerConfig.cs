using UnityEngine;

public class SpawnerConfig
{
    public static int AnyValue = -1;
    public static Color AnyColor = Color.black;

    public float NumGridCells = 1;
    public float GridPosition = AnyValue;

    public Color ShipColor = AnyColor;
    public BuffMultipliers Buffs = new BuffMultipliers();

    public bool GetSpawnPosition(out Vector3 Position)
    {
        if (GridPosition > 0)
        {
            float CellWidth = RenderingService.Instance.TargetSize.x / (NumGridCells > 0f ? NumGridCells : 1f);

            Position = RenderingService.Instance.LeftTop;
            Position.x += CellWidth * (GridPosition - 1) + (CellWidth * 0.5f);
            return true;
        }

        Position = Vector3.zero;
        return false;
    }
}
