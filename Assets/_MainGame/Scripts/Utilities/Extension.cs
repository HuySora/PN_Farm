namespace FarmGame
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Tilemaps;

    public static class Extension
    {
        public static bool TryNullCheckAndLog<T, TTrace, TSender>(this T target, string message, TTrace trace, TSender sender)
        where T : Object
        where TTrace : Object
        where TSender: Object
        {
            if (target == null && trace == null)
            {
                sender.LogNull($"Parameters are null with this message: {message}");
                return true;
            }
            if (target == null)
            {
                sender.LogNull($"{message} (click to trace {trace.name})", trace);
                return true;
            }

            return false;
        }

        public static TileBase[] CreateTileArray(this TileBase tile, BoundsInt area)
        {
            var tiles = new TileBase[area.size.x * area.size.y];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = tile;
            }
            
            return tiles;
        }
    }
}

