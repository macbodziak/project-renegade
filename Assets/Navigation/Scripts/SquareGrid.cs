using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Navigation
{
    public class SquareGrid : NavGrid
    {
        #region Fields
        static private readonly int DIAGONAL_COST = 14;
        static private readonly int STRAIGHT_COST = 10;
        static private readonly Vector2Int[] neighbours = {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
        };
        #endregion

        #region Properties
        public Vector2Int[] Neighbours { get => neighbours; }
        public static int DiagonalCost { get => DIAGONAL_COST; }
        public static int StraightCost { get => STRAIGHT_COST; }
        public override Bounds WorldBounds
        {
            get
            {
                Bounds bounds = new Bounds();
                bounds.size = new Vector3(Width * TileSize, 1f, Height * TileSize);
                bounds.center = new Vector3((Width - 1f) * TileSize * 0.5f + transform.position.x,
                 0.5f + transform.position.y,
                (Height - 1f) * TileSize * 0.5f + transform.position.z);
                return bounds;
            }
        }
        #endregion

        protected override bool TestForWalkability(Vector3 nodeWorldPosition, LayerMask notWalkableLayers, float colliderSize, float rayLength)
        {
            Vector3 center = nodeWorldPosition + new Vector3(0f, rayLength, 0f);
            Vector3 halfExtents = Vector3.one * TileSize * 0.5f * colliderSize;
            Vector3 direction = Vector3.down;
            float maxDistance = rayLength;

            if (Physics.BoxCast(center, halfExtents, direction, Quaternion.identity, maxDistance, notWalkableLayers))
            {
                return false;
            }
            return true;
        }


        protected override void SetupCollider(int collisionLayer)
        {
            //check if there are already colliders attached and remove them
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                Debug.Log("<color=#ffa500ff>removing existing NavGrid collider</color>: " + col);
                DestroyImmediate(col);
            }

            gameObject.layer = collisionLayer;

            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(Width * TileSize, 0.1f, Height * TileSize);
            boxCollider.center = new Vector3((Width - 1f) * TileSize * 0.5f, 0f, (Height - 1f) * TileSize * 0.5f);
        }

        protected override Vector2Int WorldPositionToGridCoordinates(Vector3 worldPosition)
        {
            int x = (int)((worldPosition.x - transform.position.x) / TileSize + 0.5f);
            int z = (int)((worldPosition.z - transform.position.z) / TileSize + 0.5f);
            return new Vector2Int(x, z);
        }

        protected override Vector3 GridCoordinatesToWorldPosition(int x, int z)
        {
            return new Vector3(transform.position.x + x * TileSize,
                    transform.position.y,
                    transform.position.z + z * TileSize);
        }
#if UNITY_EDITOR
        protected override void DrawNodeCenterOutineGizmos(Node n)
        {
            Vector3 worldPos = nodeWorldPositions[n.id];

            if (DebugDrawTileCenter)
            {
                Gizmos.DrawCube(nodeWorldPositions[n.id], new Vector3(0.1f, 0.1f, 0.1f));
            }

            if (DebugDrawTileOutline)
            {
                Vector3[] points = new Vector3[4];
                points[0] = new Vector3(worldPos.x - TileSize * 0.49f, worldPos.y, worldPos.z + TileSize * 0.49f);
                points[1] = new Vector3(worldPos.x + TileSize * 0.49f, worldPos.y, worldPos.z + TileSize * 0.49f);
                points[2] = new Vector3(worldPos.x + TileSize * 0.49f, worldPos.y, worldPos.z - TileSize * 0.49f);
                points[3] = new Vector3(worldPos.x - TileSize * 0.49f, worldPos.y, worldPos.z - TileSize * 0.49f);
                Gizmos.DrawLineStrip(points, true);
            }
        }
#endif

        public override List<int> AdjacentNodeIndexes(int index)
        {
            List<int> result = new();
            Vector2Int currentCoordinates = GridCoordinatesAt(index);

            for (int i = 0; i < 8; i++)
            {
                if (CheckIfInBound(currentCoordinates + neighbours[i]))
                {
                    result.Add(IndexAt(currentCoordinates + neighbours[i]));
                }
            }

            return result;
        }

        public override bool AreAdjacent(int firstIndex, int secondIndex)
        {
            Vector2Int currentCoordinates = GridCoordinatesAt(firstIndex);

            int index;
            for (int i = 0; i < 8; i++)
            {
                index = IndexAt(currentCoordinates + neighbours[i]);

                if (index == secondIndex)
                {
                    return true;
                }
            }

            return false;
        }

        public override List<Actor> AdjacentActors(int index)
        {
            List<Actor> result = new();
            Vector2Int currentCoordinates = GridCoordinatesAt(index);
            int neighbourIndex;

            for (int i = 0; i < 8; i++)
            {
                neighbourIndex = IndexAt(currentCoordinates + neighbours[i]);
                if (neighbourIndex != -1)
                {
                    if (actors.ContainsKey(neighbourIndex))
                    {
                        result.Add(actors[neighbourIndex]);
                    }
                }
            }

            return result;
        }

    }
}