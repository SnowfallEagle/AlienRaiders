using UnityEngine;
using UnityEngine.Assertions;

public class SpawnerConfig
{
    public enum AlignType
    {
        None,
        Left,
        Right,
        Center
    }

    public const int AnyValue = -1;
    public static Color AnyColor = Color.black;

    public Color ShipColor = AnyColor;
    public BuffMultipliers Buffs = new BuffMultipliers();

    public AlignType Align = AlignType.None;

    public float NumGridCells = 1;
    public float GridPosition = AnyValue;

    public bool GetSpawnPosition(out Vector3 Position, float GroupWidth)
    {
        if (Align != AlignType.None)
        {
            switch (Align)
            {
                case AlignType.Left:
                    Position = RenderingService.Instance.LeftTop;
                    Position.x += GroupWidth * 0.5f;
                    break;

                case AlignType.Right:
                    Position = RenderingService.Instance.RightTop;
                    Position.x -= GroupWidth * 0.5f;
                    break;

                case AlignType.Center:
                    Position = RenderingService.Instance.CenterTop;
                    break;

                default:
                    Assert.IsTrue(false);
                    Position = Vector3.zero;
                    break;
            }

            return true;
        }

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
