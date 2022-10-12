using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System.Reflection;

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

    public ParticleSystem pSystem;
    public int turnsPassed;

    public bool whiteNoMoves;
    public bool blackNoMoves;

    public GameObject winPanel;
    public TextMeshProUGUI victoryText;

    GameObject queenWhite;
    GameObject queenBlack;

    bool gameOver;
    private void Awake()
    {
        Instance = this;
        highlight = GetComponent<Highlight>();
    }
    private void Start()
    {
        UpdateInspectorList();
        queenWhite = grid[3, 0].piece;
        queenBlack = grid[3, 7].piece;
    }
    private void Update()
    {
        if (!gameOver)
        {
            if (blackCount == 0 || whiteCount == 0)
            {
                string winner = blackCount == 0 ? "brown" : "red";
                gameOver = true;
                victoryText.text = "Team " + winner + " wins!";
                winPanel.SetActive(true);
            }
            else if (whiteNoMoves)
            {
                victoryText.text = "Team red wins! Team brown has no more moves!";
                winPanel.SetActive(true);
                gameOver = true;
            }
            else if (blackNoMoves)
            {
                victoryText.text = "Team brown wins! Team red has no more moves!";
                winPanel.SetActive(true);
                gameOver = true;
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
                        if (clicked.pieceName == PieceName.pawn)
                        {
                            int ind = Array.IndexOf(inspectorGrid, selected);
                            int y = (int)(ind / 8);
                            if(clicked.pieceTeam == Team.white && y == 7)
                            {
                                selected.pieceName = PieceName.queen;
                                selected.piece = queenWhite;
                            }
                            else if(clicked.pieceTeam == Team.black && y == 0)
                            {
                                selected.pieceName = PieceName.queen;
                                selected.piece = queenBlack;
                            }

                        }
                        if (selected.pieceTeam != Team.none)
                        {
                            selected.DestroyPieceModel();
                        }
                        selected.SpawnPieceModel();
                        selected.SpawnParticle(pSystem);
                        selected.pieceTeam = clicked.pieceTeam;

                        clicked.DestroyPieceModel();
                        clicked.Reset();
                        clicked = null;

                        highlighted = new List<PieceHolder>();

                        currentTeam = currentTeam == Team.white ? Team.black : Team.white;
                        currentPieces = new List<PieceHolder>();
                        UpdateInspectorList();
                        turnsPassed += 1;
                    }
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

        foreach (PieceHolder piece in currentPieces)
        {
            int ind = Array.IndexOf(inspectorGrid, selected);
            int y = (int)(ind / 8);
            int x = ind % 8;
            whiteNoMoves = true;
            if (piece.pieceTeam == Team.white && highlight.ReturnHighlight(piece.pieceName, x, y, Team.white, grid, piece).Count > 0)
            {
                whiteNoMoves = false;
                break;
            }
        }
        foreach (PieceHolder piece in currentPieces)
        {
            int ind = Array.IndexOf(inspectorGrid, selected);
            int y = (int)(ind / 8);
            int x = ind % 8;
            blackNoMoves = true;
            if (piece.pieceTeam == Team.black && highlight.ReturnHighlight(piece.pieceName, x, y, Team.black, grid, piece).Count > 0)
            {
                blackNoMoves = false;
                break;
            }
        }
        blackCount = bC;
        whiteCount = wC;
    }
}


