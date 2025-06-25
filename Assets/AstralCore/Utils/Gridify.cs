using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>
namespace AstralCore.Utils{
    public readonly struct Gridify{
        public readonly int CELL_SIZE;
        int HALF_SIZE => Mathf.RoundToInt((float)CELL_SIZE/2);
        public Gridify(int cellSize = 1) => CELL_SIZE = cellSize;
        
        /// <summary>
        /// Floors the parsed position to a whole position such as on a grid.
        /// </summary>
        /// <param name="position">The position we want to gridify</param>
        /// <param name="center">Do we want it centered to the cell?</param>
        /// <param name="style">What math function do we want to use?</param>
        /// <returns>The gridified position</returns>
        public Vector3Int Calc(Vector3 position, bool center = false, Style style = Style.Floor){
            Vector3Int newPosition = style switch{
                Style.Round => Round(position),
                Style.Ceil => Ceil(position),
                _ => Floor(position)
            };

            Vector3Int _center = center ? Vector3Int.one * HALF_SIZE : Vector3Int.zero;
            return newPosition + _center;
        }

        int Round(float v) => Mathf.RoundToInt(v / CELL_SIZE) * CELL_SIZE;
        int Floor(float v) => Mathf.FloorToInt(v / CELL_SIZE) * CELL_SIZE;
        int Ceil(float v) => Mathf.CeilToInt(v / CELL_SIZE) * CELL_SIZE;
        Vector3Int Round(Vector3 v) => new(Round(v.x), Round(v.y), Round(v.z));
        Vector3Int Floor(Vector3 v) => new(Floor(v.x), Floor(v.y), Floor(v.z));
        Vector3Int Ceil(Vector3 v) => new(Ceil(v.x), Ceil(v.y), Ceil(v.z));


        /// <summary>
        /// What function will be used to lock position to grid?
        /// </summary>
        public enum Style{
            /// <summary>
            /// Round to nearest
            /// </summary>
            Round,
            /// <summary>
            /// Round down
            /// </summary>
            Floor,
            /// <summary>
            /// Round up
            /// </summary>
            Ceil
        }
    }
}