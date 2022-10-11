using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    Chessboard chessboard;
    [Range(2, 16)] public int tilesToShoot;
    public List<PieceHolder> selectedTiles = new List<PieceHolder>();
    public List<PieceHolder> destroyedTiles = new List<PieceHolder>();
    public int turnsBetweenShot = 2;
    public int increasePerShot;
    public int maxTilesToShoot;
    public bool hasShot;
    // Start is called before the first frame update
    void Start()
    {
        chessboard = Chessboard.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Chessboard.Instance.turnsPassed != 0 && Chessboard.Instance.turnsPassed % turnsBetweenShot == 0 && !hasShot)
        {
            for (int i = 0; i < tilesToShoot; i++)
            {
                SelectTile();
            }
            ShootTiles();
            hasShot = true;
            tilesToShoot += increasePerShot;
            tilesToShoot = Mathf.Clamp(tilesToShoot, 0, 16);
        }
        else if (Chessboard.Instance.turnsPassed != 0 && hasShot && Chessboard.Instance.turnsPassed % turnsBetweenShot != 0) hasShot = false;
    }
    public void ShootTiles()
    {

        foreach (PieceHolder piece in destroyedTiles)
        {
            piece.selectable = true;
        }
        destroyedTiles = new List<PieceHolder>();
        foreach (PieceHolder piece in selectedTiles)
        {
            piece.selectable = false;
            piece.DestroyPieceModel();
            piece.Reset();
            destroyedTiles.Add(piece);
        }
        selectedTiles = new List<PieceHolder>();
    }
    public void SelectTile()
    {
        int x = Random.Range(0, 8);
        int y = Random.Range(2, 6);
        PieceHolder piece = chessboard.grid[x, y];
        if (selectedTiles.Contains(piece))
        {
            SelectTile();
        }
        else
        {
            selectedTiles.Add(piece);
        }

    }
}
