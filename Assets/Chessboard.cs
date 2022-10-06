using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class Chessboard : MonoBehaviour
{
    public static Chessboard Instance;
    public PieceHolder[,] grid = new PieceHolder[8, 8];
    public PieceHolder[] inspectorGrid = new PieceHolder[64];
    public PieceHolder selected;
    public PieceHolder clicked;
    public List<PieceHolder> highlighted;
    public Team currentTeam;

    public List<PieceHolder> currentPieces;

    Highlight highlight;

    public int blackCount = 16;
    public int whiteCount = 16;

    private void Awake()
    {
        Instance = this;
        highlight = GetComponent<Highlight>();
    }
    private void Start()
    {
        UpdateInspectorList();
    }
    private void Update()
    {
        if(blackCount == 0 || whiteCount == 0)
        {
            string winner = blackCount == 0 ? "white" : "black";
            print("TEAM " + winner + " WINS!!!");
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<PieceHolder>())
                {
                    PieceHolder holder = hit.collider.GetComponent<PieceHolder>();
                    selected = holder;
                }
            }
            else
            {
                selected = null;
            }
            if (selected == null && Input.GetMouseButtonDown(0))
            {
                clicked = null;
                highlighted = new List<PieceHolder>();
            }
            if (selected != null && Input.GetMouseButtonDown(0))
            {
                if (!highlighted.Contains(selected) && selected.pieceName != PieceName.none && selected.pieceTeam == currentTeam)
                {
                    clicked = selected;
                    PieceName name = selected.pieceName;
                    var index = Array.IndexOf(inspectorGrid, selected);
                    int y = (int)(index / 8);
                    int x = index % 8;
                    Team team = selected.pieceTeam;
                    highlighted = highlight.ReturnHighlight(name, x, y, team, grid, clicked);
                }
                else if (highlighted.Contains(selected))
                {
                    selected.firstMove = false;
                    selected.pieceName = clicked.pieceName;
                    selected.piece = clicked.piece;
                    if (selected.pieceTeam != Team.none)
                    {
                        selected.DestroyPieceModel();
                    }
                    selected.SpawnPieceModel();
                    selected.pieceTeam = clicked.pieceTeam;

                    clicked.DestroyPieceModel();
                    clicked.Reset();
                    clicked = null;

                    highlighted = new List<PieceHolder>();

                    currentTeam = currentTeam == Team.white ? Team.black : Team.white;
                    currentPieces = new List<PieceHolder>();
                    UpdateInspectorList();
                }
            }
        }
        
    }
    public void UpdateInspectorList()
    {
        int i = 0;
        int bC = 0;
        int wC = 0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                grid[x, y] = inspectorGrid[i];
                if (grid[x, y].pieceTeam == Team.white) wC += 1;
                if (grid[x, y].pieceTeam == Team.black) bC += 1;
                if (inspectorGrid[i].piece != null)
                {
                    currentPieces.Add(inspectorGrid[i]);
                }
                i += 1;
            }
        }

        blackCount = bC;
        whiteCount = wC;
    }
}


