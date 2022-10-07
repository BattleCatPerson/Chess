using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    Chessboard chessboard;
    [Range(4, 16)] public int tilesToShoot;
    public List<PieceHolder> selectedTiles = new List<PieceHolder>();
    // Start is called before the first frame update
    void Start()
    {
        chessboard = Chessboard.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShootTiles()
    {
        foreach (PieceHolder piece in selectedTiles)
        {
            piece.selectable = true;
        }
        for(int i = 0; i < tilesToShoot; i++)
        {
            SelectTile();
        }
        foreach (PieceHolder piece in selectedTiles)
        {
            piece.selectable = false;
        }
    }
    public void SelectTile()
    {
        while (true)
        {
            int x = Random.Range(0, 8);
            int y = Random.Range(0, 8);
            PieceHolder piece = chessboard.grid[x, y];
            if (!selectedTiles.Contains(piece))
            {
                selectedTiles.Add(piece);
                break;
            }
        }

    }
}
