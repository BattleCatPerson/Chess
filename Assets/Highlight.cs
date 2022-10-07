using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public List<PieceHolder> ReturnHighlight(PieceName name, int x, int y, Team team, PieceHolder[,] grid, PieceHolder clicked, bool check = false)
    {
        List<PieceHolder> hList = new List<PieceHolder>();
        List<PieceHolder> captureList = new List<PieceHolder>();
        if (name == PieceName.pawn)
        {
            if (team == Team.white)
            {
                if (y + 1 < 8 && grid[x, y + 1].piece == null)
                {
                    int distance = clicked.firstMove ? 2 : 1;

                    for (int i = 1; i <= distance; i++)
                    {
                        if (grid[x, y + i].pieceTeam == Team.none) hList.Add(grid[x, y + i]);
                        else break;
                    }
                }
                if (y + 1 < 8)
                {
                    if (x - 1 >= 0 && grid[x - 1, y + 1].pieceTeam == Team.black)
                    {
                        hList.Add(grid[x - 1, y + 1]);
                        captureList.Add(grid[x - 1, y + 1]);
                    }
                    if (x + 1 < 8 && grid[x + 1, y + 1].pieceTeam == Team.black)
                    {
                        hList.Add(grid[x + 1, y + 1]);
                        captureList.Add(grid[x + 1, y + 1]);
                    }
                }
            }
            else
            {
                if (y - 1 >= 0 && grid[x, y - 1].piece == null)
                {
                    int distance = clicked.firstMove ? 2 : 1;
                    for (int i = 1; i <= distance; i++)
                    {
                        if (grid[x, y - i].pieceTeam == Team.none) hList.Add(grid[x, y - i]);
                        else break;
                    }
                }
                if (y - 1 >= 0)
                {
                    if (x - 1 >= 0 && grid[x - 1, y - 1].pieceTeam == Team.white)
                    {
                        hList.Add(grid[x - 1, y - 1]);
                        captureList.Add(grid[x - 1, y - 1]);
                    }
                    if (x + 1 < 8 && grid[x + 1, y - 1].pieceTeam == Team.white)
                    {
                        hList.Add(grid[x + 1, y - 1]);
                        captureList.Add(grid[x + 1, y - 1]);
                    }
                }
            }
        }

        if (name == PieceName.knight)
        {
            for (int i = -2; i < 5; i += 4)
            {
                if (y + i >= 0 && y + i < 8)
                {
                    if (x + 1 < 8 && grid[x + 1, y + i].pieceTeam != team)
                    {
                        if (grid[x + 1, y + i].pieceTeam != Team.none) captureList.Add(grid[x + 1, y + i]);
                        hList.Add(grid[x + 1, y + i]);
                    }
                    if (x - 1 >= 0 && grid[x - 1, y + i].pieceTeam != team)
                    {
                        if (grid[x - 1, y + i].pieceTeam != Team.none) captureList.Add(grid[x - 1, y + i]);
                        hList.Add(grid[x - 1, y + i]);
                    }
                }
                if (x + i >= 0 && x + i < 8)
                {
                    if (y + 1 < 8 && grid[x + i, y + 1].pieceTeam != team)
                    {
                        if (grid[x + i, y + 1].pieceTeam != Team.none) captureList.Add(grid[x + i, y + 1]);
                        hList.Add(grid[x + i, y + 1]);
                    }
                    if (y - 1 >= 0 && grid[x + i, y - 1].pieceTeam != team)
                    {
                        if (grid[x + i, y - 1].pieceTeam != Team.none) captureList.Add(grid[x + i, y - 1]);
                        hList.Add(grid[x + i, y - 1]);
                    }
                }
            }
        }

        if (name == PieceName.bishop)
        {
            int cX = x;
            int cY = y;
            int xDir = 1;
            int yDir = 1;

            for (int i = 0; i < 4; i++)
            {
                cX = x;
                cY = y;
                if (i == 1) xDir = -1;
                if (i == 2) yDir = -1;
                if (i == 3) xDir = 1;
                while (cX >= 0 && cX < 8 && cY >= 0 && cY < 8)
                {
                    cX += xDir;
                    cY += yDir;
                    if (cX < 0 || cX >= 8 || cY < 0 || cY >= 8 || grid[cX, cY].pieceTeam == team) break;
                    if (grid[cX, cY].pieceTeam != Team.none && grid[cX, cY].pieceTeam != team)
                    {
                        hList.Add(grid[cX, cY]);
                        captureList.Add(grid[cX, cY]);
                        break;
                    }

                    hList.Add(grid[cX, cY]);
                }
            }
        }

        if (name == PieceName.rook)
        {
            int cX = x;
            int cY = y;

            for (int i = 0; i < 2; i++)
            {
                cX = x;
                int xDir = i == 0 ? 1 : -1;
                while (cX >= 0 && cX < 8 && cY >= 0 && cY < 8)
                {
                    cX += xDir;
                    if (cX < 0 || cX >= 8 || cY < 0 || cY >= 8 || grid[cX, cY].pieceTeam == team) break;
                    if (grid[cX, cY].pieceTeam != Team.none && grid[cX, cY].pieceTeam != team)
                    {
                        hList.Add(grid[cX, cY]);
                        captureList.Add(grid[cX, cY]);
                        break;
                    }
                    hList.Add(grid[cX, cY]);
                }
            }

            cX = x;
            cY = y;

            for (int i = 0; i < 2; i++)
            {
                cY = y;
                int yDir = i == 0 ? 1 : -1;
                while (cX >= 0 && cX < 8 && cY >= 0 && cY < 8)
                {
                    cY += yDir;
                    if (cX < 0 || cX >= 8 || cY < 0 || cY >= 8 || grid[cX, cY].pieceTeam == team) break;
                    if (grid[cX, cY].pieceTeam != Team.none && grid[cX, cY].pieceTeam != team)
                    {
                        hList.Add(grid[cX, cY]);
                        captureList.Add(grid[cX, cY]);
                        break;
                    }
                    hList.Add(grid[cX, cY]);
                }
            }
        }
        if (name == PieceName.queen)
        {
            int cX = x;
            int cY = y;

            for (int i = 0; i < 2; i++)
            {
                cX = x;
                int xD = i == 0 ? 1 : -1;
                while (cX >= 0 && cX < 8 && cY >= 0 && cY < 8)
                {
                    cX += xD;
                    if (cX < 0 || cX >= 8 || cY < 0 || cY >= 8 || grid[cX, cY].pieceTeam == team) break;
                    if (grid[cX, cY].pieceTeam != Team.none && grid[cX, cY].pieceTeam != team)
                    {
                        hList.Add(grid[cX, cY]);
                        captureList.Add(grid[cX, cY]);
                        break;
                    }
                    hList.Add(grid[cX, cY]);
                }
            }

            cX = x;
            cY = y;

            for (int i = 0; i < 2; i++)
            {
                cY = y;
                int yD = i == 0 ? 1 : -1;
                while (cX >= 0 && cX < 8 && cY >= 0 && cY < 8)
                {
                    cY += yD;
                    if (cX < 0 || cX >= 8 || cY < 0 || cY >= 8 || grid[cX, cY].pieceTeam == team) break;
                    if (grid[cX, cY].pieceTeam != Team.none && grid[cX, cY].pieceTeam != team)
                    {
                        hList.Add(grid[cX, cY]);
                        captureList.Add(grid[cX, cY]);
                        break;
                    }
                    hList.Add(grid[cX, cY]);
                }
            }


            cX = x;
            cY = y;
            int xDir = 1;
            int yDir = 1;

            for (int i = 0; i < 4; i++)
            {
                cX = x;
                cY = y;
                if (i == 1) xDir = -1;
                if (i == 2) yDir = -1;
                if (i == 3) xDir = 1;
                while (cX >= 0 && cX < 8 && cY >= 0 && cY < 8)
                {
                    cX += xDir;
                    cY += yDir;
                    if (cX < 0 || cX >= 8 || cY < 0 || cY >= 8 || grid[cX, cY].pieceTeam == team) break;
                    if (grid[cX, cY].pieceTeam != Team.none && grid[cX, cY].pieceTeam != team)
                    {
                        hList.Add(grid[cX, cY]);
                        captureList.Add(grid[cX, cY]);
                        break;
                    }
                    hList.Add(grid[cX, cY]);
                }
            }
        }
        if (name == PieceName.king)
        {
            for (int i = -1; i < 2; i += 2)
            {
                if (x + i >= 0 && x + i < 8 && grid[x + i, y].pieceTeam != team) hList.Add(grid[x + i, y]);
                if (y + i >= 0 && y + i < 8 && grid[x, y + i].pieceTeam != team) hList.Add(grid[x, y + i]);
            }
            int xDir = 1;
            int yDir = 1;
            for (int i = 0; i < 4; i++)
            {
                if (i == 1) xDir = -1;
                if (i == 2) yDir = -1;
                if (i == 3) xDir = 1;
                int newX = x + xDir;
                int newY = y + yDir;
                if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8 && grid[newX, newY].pieceTeam != team) hList.Add(grid[newX, newY]);

            }
        }
        int ind = 0;
        while(ind < captureList.Count)
        {
            if (!captureList[ind].selectable) captureList.RemoveAt(ind);
            else ind += 1;
        }
        ind = 0;
        while (ind < hList.Count)
        {
            if (!hList[ind].selectable) hList.RemoveAt(ind);
            else ind += 1;
        }
        if (check) return captureList;
        return hList;
    }

}
