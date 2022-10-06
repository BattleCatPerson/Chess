using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceName
{
    pawn,knight,bishop,rook,queen,king,none
}
public enum Team
{
    white, black, none
}

public class Piece : MonoBehaviour
{
    public PieceName pieceName;
    public Team team;
}
