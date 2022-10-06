using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PieceHolder : MonoBehaviour
{
    public GameObject piece;
    public GameObject clone;
    public PieceName pieceName;
    public Team pieceTeam;
    public Chessboard board;
    public Material highlight;
    [HideInInspector]
    public Material original;
    public bool firstMove;
    // Start is called before the first frame update
    void Awake()
    {
        firstMove = true;
        if (piece != null) SpawnPieceModel();
        else
        {
            pieceName = PieceName.none;
            pieceTeam = Team.none;
        }
        original = GetComponent<Renderer>().material;
        board = Chessboard.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (board.highlighted.Contains(this)) GetComponent<Renderer>().material = highlight;
        else
        {
            GetComponent<Renderer>().material = original;
        }
    }

    public void SpawnPieceModel()
    {
        clone = Instantiate(piece, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        pieceName = piece.GetComponent<Piece>().pieceName;
        pieceTeam = piece.GetComponent<Piece>().team;
    }

    public void DestroyPieceModel()
    {
        Destroy(clone);
    }

    public void Reset()
    {
        piece = null;
        pieceName = PieceName.none;
        pieceTeam = Team.none;
    }


}
