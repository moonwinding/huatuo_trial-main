using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class FogTileMap : TileBase
{
    public Sprite look;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = look;
        base.GetTileData(position, tilemap, ref tileData);
    }
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        
        return base.StartUp(position, tilemap, go);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(FogTileMap))]
public class FogTileMapEditor : Editor
{
    private FogTileMap tile { get { return (target as FogTileMap); } }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();
        tile.look = EditorGUILayout.ObjectField("Default Sprite", tile.look, typeof(Sprite), false) as Sprite;
        
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(tile);
    }

}
#endif
