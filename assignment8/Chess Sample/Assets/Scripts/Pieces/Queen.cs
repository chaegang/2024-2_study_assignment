using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        List<MoveInfo> moves = new List<MoveInfo>();

        int[] xy = new int[] {1, -1, 0};

        for (int dis = 1; dis <= 7; dis++)
        {
            foreach (var x in xy)
            {
                foreach (var y in xy)
                {
                    if (x != 0 || y != 0)
                    {
                        moves.Add(new MoveInfo(x, y, dis));
                    }
                }
            }
        }

        return moves.ToArray();
        // ------
    }
}