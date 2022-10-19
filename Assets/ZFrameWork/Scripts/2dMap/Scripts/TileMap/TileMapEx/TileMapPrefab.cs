using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class TileMapPrefab : TileBase
{
    public string makeyType = "";
    public Sprite look;
    public bool SaveLook;
    public GameObject prefab;
    public Vector3 offset = new Vector3(0.5f,0.5f,0);
    public Tile.ColliderType colliderType = Tile.ColliderType.None;
    private TileData tileData_;
    private GameObject go_;
    private Dictionary<int, GameObject> prefabMap = new Dictionary<int, GameObject>();
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) 
    {
        tileData.gameObject = prefab;
        tileData.colliderType = colliderType;
        //Debug.LogError("GetTileData >> ");
        if (!Application.isPlaying){
            tileData.sprite = look;
            base.GetTileData(position, tilemap, ref tileData);
        }
        else{
            if (!SaveLook)
            {
                //tileData.sprite = null;
            }
            else {
                tileData.sprite = look;
            }
            
            base.GetTileData(position, tilemap, ref tileData);
        }

        tileData_ = tileData;
    }
   
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (Application.isPlaying &&!SaveLook)
        {
            //tileData_.sprite = null;
        }
        return base.StartUp(position, tilemap, go);
    }
    
    private void LoadPrefab(Vector3Int position,GameObject go) {
        if (Application.isPlaying)
        {
            if (prefab != null && go==null)
                go_ = GameObject.Instantiate<GameObject>(prefab);
            if (go_ != null)
                go_.transform.position = position + offset;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TileMapPrefab))]
public class TileMapPrefabEditor : Editor
{
    private TileMapPrefab tile { get { return (target as TileMapPrefab); } }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();
        tile.look = EditorGUILayout.ObjectField("Default Sprite", tile.look, typeof(Sprite), false) as Sprite;
        tile.prefab = EditorGUILayout.ObjectField("LoadPrefab", tile.prefab, typeof(GameObject), false) as GameObject;
        tile.makeyType = EditorGUILayout.TextField("MakeyType", tile.makeyType);
        tile.offset = EditorGUILayout.Vector3Field("Offset", tile.offset);
        tile.colliderType = (Tile.ColliderType)EditorGUILayout.EnumPopup("Default Collider", tile.colliderType);
        tile.SaveLook = EditorGUILayout.Toggle("SaveLook", tile.SaveLook);
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(tile);
    }
}
#endif
