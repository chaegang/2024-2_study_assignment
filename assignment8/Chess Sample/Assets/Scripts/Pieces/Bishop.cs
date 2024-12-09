using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        List<MoveInfo> moves = new List<MoveInfo>();

        int[] xy = new int[] {1, -1};

        for (int dis = 1; dis <= 7; dis++)
        {
            foreach (var x in xy)
            {
                foreach (var y in xy)
                {
                    moves.Add(new MoveInfo(x, y, dis));
                }
            }
        }

        return moves.ToArray();
        
        // ------
    }
}