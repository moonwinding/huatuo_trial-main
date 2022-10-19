using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapLayer : MonoBehaviour
{
    public int MapId;
    public int LayerId;
    private Tilemap tileMap;
    private Dictionary<Vector3Int, GameObject> mapPrefabs = new Dictionary<Vector3Int, GameObject>();
    private void Awake()
    {
        tileMap = this.gameObject.GetComponent<Tilemap>();
    }
    private void Start()
    {
        //Debug.LogError("TileMapLayer Start >> ");
        tileMap.RefreshAllTiles();
    }
}
