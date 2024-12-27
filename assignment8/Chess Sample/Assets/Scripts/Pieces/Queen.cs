using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
            new MoveInfo(0, 1, Utils.FieldHeight),
            new MoveInfo(0, -1, Utils.FieldHeight),
            new MoveInfo(1, 0, Utils.FieldWidth),
            new MoveInfo(1, 1, Utils.FieldWidth),
            new MoveInfo(1, -1, Utils.FieldWidth),
            new MoveInfo(-1, 0, Utils.FieldWidth),
            new MoveInfo(-1, 1, Utils.FieldWidth),
            new MoveInfo(-1, -1, Utils.FieldWidth)

        };
        // ------
    }
}