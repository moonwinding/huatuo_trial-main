using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    public float speed;
    private bool isMove;
    private float y;
    private Vector3 move;
    private Animator animator;
    public Tilemap tilemap;
    public Tile baseTile;//ʹ�õ��������Tile���������ǰ�ɫ�飬Ȼ������������ò�ͬ��ɫ���ɲ�ͬTile
    // Start is called before the first frame update
    void Start(){
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        this.transform.Translate(move*speed*Time.deltaTime);
        isMove = move.x != 0 || move.y != 0;
        if (isMove)
            animator.SetFloat("Speed", speed);
        else
            animator.SetFloat("Speed", 0);
        if (isMove && move.x != 0)
            this.transform.localScale = new Vector3(move.x > 0 ? 1 : -1, 1, 1);
        ////����
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePosition = Input.mousePosition;
        //    Vector3 wordPosition = this.transform.position;// Camera.main.ScreenToWorldPoint(mousePosition);
        //    Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
        //    //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
        //    Debug.Log("�������" + mousePosition+ "����" + wordPosition + "��λλ��" + cellPosition);
        
        //    //TileBase tb = tilemap.GetTile(cellPosition);
        //    TileBase tb = tilemap.GetTile(cellPosition);
        //    if (tb == null){
        //        return;
        //    }
        //    //tb.hideFlags = HideFlags.None;
        //    Debug.Log("�������" + mousePosition + "����" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
        //    //ĳ���ط�����Ϊ�գ����ǰ��Ǹ��ط�С����������
        //    tilemap.SetTile(cellPosition, null);
        //    //tilemap.RefreshAllTiles();
        //}
        ////�հ׵ط�����ǽ��
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 mousePosition = Input.mousePosition;
        //    Vector3 wordPosition = this.transform.position;// Camera.main.ScreenToWorldPoint(mousePosition);
        //    Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
        //    //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
        //    TileBase tb = tilemap.GetTile(cellPosition);
        //    if (tb != null)
        //    {
        //        return;
        //    }
        //    //tb.hideFlags = HideFlags.None;
        //    //Debug.Log("�������" + mousePosition + "����" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
        //    //�������
        //    tilemap.SetTile(cellPosition, baseTile);
        //    //tilemap.RefreshAllTiles();
        //}
    }
}
