﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NodeType
    {
        NONE = 0,
        PAWN = 1,
        BISHOP = 2,
        KNIGHT = 3,
        ROOK = 4,
        QUEEN = 5,
        KING = 6,
        TOTAL = 7
    };

    public enum NodeTeam
    {
        NONE = 0,
        WHITE = 1,
        BLACK = 2
    };

    [SerializeField] private NodeType nodeType;
    [SerializeField] private NodeTeam nodeTeam;

    [SerializeField] private Vector2 currentPos;
    [SerializeField] private List<Node> nodesToCheck;

    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private GameObject bishopPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject rookPrefab;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject kingPrefab;

    [SerializeField] private GameObject currentPrefab;

    [SerializeField] private int moveCounter;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetCurrentPos(int x, int y)
    {
        currentPos.x = x;
        currentPos.y = y;
    }

    public NodeType GetNodeType()
    {
        return nodeType;
    }

    public void SetNodeType(int type)
    {
        nodeType = (NodeType)type;
    }

    public NodeTeam GetNodeTeam()
    {
        return nodeTeam;
    }

    public void SetNodeTeam(int team)
    {
        nodeTeam = (NodeTeam)team;
    }

    public void UpdateNode()
    {
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }

        if (nodeType == NodeType.PAWN)
        {
            CheckPawn();
        }
        else if (nodeType == NodeType.BISHOP)
        {
            CheckBishop();
        }
        else if (nodeType == NodeType.KNIGHT)
        {
            CheckKnight();
        }
        else if (nodeType == NodeType.ROOK)
        {
            CheckRook();
        }
        else if (nodeType == NodeType.QUEEN)
        {
            CheckQueen();
        }
        else if (nodeType == NodeType.KING)
        {
            CheckKing();
        }
        else
        {
            CheckEmpty();
        }
    }

    public void CheckPawn()
    {
        currentPrefab = Instantiate(pawnPrefab, transform.position, Quaternion.identity, transform);
        SpriteRenderer sRend = currentPrefab.GetComponent<SpriteRenderer>();

        if (nodeTeam == NodeTeam.WHITE)
        {
            sRend.color = Color.white;
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            sRend.color = Color.black;
        }
    }

    public void CheckPawnMovement()
    {
        nodesToCheck.Clear();

        BoardManager board = GetComponentInParent<BoardManager>();
        List<Node> nodeList = board.GetNodeList();

        if (nodeTeam == NodeTeam.WHITE)
        {
            int i;
            int j;

            i = (int)currentPos.x;
            j = (int)currentPos.y;
            
            j++;

            while (j <= (int)currentPos.y + 2)
            {
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam != NodeTeam.NONE)
                {
                    break;
                }

                if (moveCounter < 1)
                {
                    j++;
                }
            }

            i = (int)currentPos.x;
            j = (int)currentPos.y;

            i++;
            j++;

            while (i <= (int)currentPos.x + 1 && j <= (int)currentPos.y + 2)
            {
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.BLACK)
                {
                    nodesToCheck.Add(node);
                }

                i++;
                j++;
            }

            i = (int)currentPos.x;
            j = (int)currentPos.y;

            i--;
            j++;

            while (i >= 0 && j <= (int)currentPos.y + 2)
            {
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.BLACK)
                {
                    nodesToCheck.Add(node);
                }

                i--;
                j++;
            }
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            int i;
            int j;

            i = (int)currentPos.x;
            j = (int)currentPos.y;

            j--;

            while (j >= (int)currentPos.y - 2)
            {
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam != NodeTeam.NONE)
                {
                    break;
                }

                // Will have to fix the counter not counting when moveCounter is more than 0.
                // Add a condition where it will move twice when moveCounter is 0.
                if (moveCounter < 1)
                {
                    j--;
                }
            }

            i = (int)currentPos.x;
            j = (int)currentPos.y;

            i++;
            j--;

            while (i <= (int)currentPos.x + 1 && j >= (int)currentPos.y - 2)
            {
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.WHITE)
                {
                    nodesToCheck.Add(node);
                }

                i++;
                j--;
            }

            i = (int)currentPos.x;
            j = (int)currentPos.y;

            i--;
            j--;

            while (i >= (int)currentPos.x - 1 && j >= (int)currentPos.y - 2)
            {
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.WHITE)
                {
                    nodesToCheck.Add(node);
                }

                i--;
                j--;
            }
        }

        //=====================================================
        // ARCHIVED CODE
        //=====================================================

        /*
        if (nodeTeam == NodeTeam.WHITE)
        {
            for (int i = 0; i < nodeList.Count; ++i)
            {
                //Checking relevant positions for the pawn.
                Node node = nodeList[i];

                //Checking for attackables, pawn only.
                if (node.currentPos.x == currentPos.x - 1 && node.currentPos.y == currentPos.y + 1 ||
                    node.currentPos.x == currentPos.x + 1 && node.currentPos.y == currentPos.y + 1)
                {
                    if (node.nodeTeam == NodeTeam.BLACK)
                    {
                        nodesToCheck.Add(node);
                    }
                }
                //Checking for movables.
                if (node.currentPos.x == currentPos.x && node.currentPos.y == currentPos.y + 1)
                {
                    if (node.nodeType == NodeType.NONE)
                    {
                        nodesToCheck.Add(node);
                    }
                    else
                    {
                        return;
                    }
                }
                // Checks whether the pawn has made the first move.
                if (moveCounter < 1)
                {
                    // If it hasn't, it can choose to take two steps.
                    if (node.currentPos.x == currentPos.x && node.currentPos.y == currentPos.y + 2)
                    {
                        if (node.nodeType == NodeType.NONE)
                        {
                            nodesToCheck.Add(node);
                        }
                    }
                }
            }
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            for (int i = 0; i < nodeList.Count; ++i)
            {
                //Checking relevant positions for the pawn.
                Node node = nodeList[i];

                //Checking for attackables, pawn only.
                if (node.currentPos.x == currentPos.x - 1 && node.currentPos.y == currentPos.y - 1 ||
                node.currentPos.x == currentPos.x + 1 && node.currentPos.y == currentPos.y - 1)
                {
                    if (node.nodeTeam == NodeTeam.WHITE)
                    {
                        nodesToCheck.Add(node);
                    }
                }
                //Checking for movables.
                if (node.currentPos.x == currentPos.x && node.currentPos.y == currentPos.y - 1)
                {
                    if (node.nodeType == NodeType.NONE)
                    {
                        nodesToCheck.Add(node);
                    }
                    else
                    {
                        return;
                    }
                }
                // Checks whether the pawn has made the first move.
                if (moveCounter < 1)
                {
                    if (node.currentPos.x == currentPos.x && node.currentPos.y == currentPos.y - 2)
                    {
                        if (node.nodeType == NodeType.NONE)
                        {
                            nodesToCheck.Add(node);
                        }
                    }
                }
            }
        }
        */

        PaintMovables();
    }

    public void CheckBishop()
    {
        currentPrefab = Instantiate(bishopPrefab, transform.position, Quaternion.identity, transform);
        SpriteRenderer sRend = currentPrefab.GetComponent<SpriteRenderer>();

        if (nodeTeam == NodeTeam.WHITE)
        {
            sRend.color = Color.white;
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            sRend.color = Color.black;
        }
    }

    public void CheckBishopMovement()
    {
        nodesToCheck.Clear();

        BoardManager board = GetComponentInParent<BoardManager>();
        List<Node> nodeList = board.GetNodeList();

        if (nodeTeam == NodeTeam.WHITE)
        {
            int i;
            int j;

            //==================================================
            // Checking "X+ Y+" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i++;
            j++;

            // Loop stops when i or j is out of range
            while (i < 8 && j < 8)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    break;
                }

                // Increment
                i++;
                j++;
            }

            //==================================================
            // Checking "X- Y+" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i++;
            j--;
            
            // Loop stops when i or j is out of range
            while (i < 8 && j >= 0)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    break;
                }

                // Increment
                i++;
                j--;
            }

            //==================================================
            // Checking "X+ Y-" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i--;
            j++;

            // Loop stops when i or j is out of range
            while (i >= 0 && j < 8)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    break;
                }

                // Increment
                i--;
                j++;
            }

            //==================================================
            // Checking "X- Y-" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i--;
            j--;

            // Loop stops when i or j is out of range
            while (i >= 0 && j >= 0)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    break;
                }

                // Increment
                i--;
                j--;
            }
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            int i;
            int j;

            //==================================================
            // Checking "X+ Y+" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i++;
            j++;

            // Loop stops when i or j is out of range
            while (i < 8 && j < 8)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    break;
                }

                // Increment
                i++;
                j++;
            }

            //==================================================
            // Checking "X- Y+" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i++;
            j--;

            // Loop stops when i or j is out of range
            while (i < 8 && j >= 0)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    break;
                }

                // Increment
                i++;
                j--;
            }

            //==================================================
            // Checking "X+ Y-" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i--;
            j++;

            // Loop stops when i or j is out of range
            while (i >= 0 && j < 8)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    break;
                }

                // Increment
                i--;
                j++;
            }

            //==================================================
            // Checking "X- Y-" Diagonal Route
            //==================================================

            // Resets i and j back to bishop's position
            i = (int)currentPos.x;
            j = (int)currentPos.y;

            // Increment
            i--;
            j--;

            // Loop stops when i or j is out of range
            while (i >= 0 && j >= 0)
            {
                // Get node at the position (i, j)
                Node node = nodeList[j * 8 + i];

                if (node.nodeTeam == NodeTeam.NONE)
                {
                    nodesToCheck.Add(node);
                }
                else if (node.nodeTeam == NodeTeam.WHITE)
                {
                    nodesToCheck.Add(node);
                    break;
                }
                else if (node.nodeTeam == NodeTeam.BLACK)
                {
                    break;
                }

                // Increment
                i--;
                j--;
            }
        }

        PaintMovables();
    }

    public void CheckKnight()
    {
        currentPrefab = Instantiate(knightPrefab, transform.position, Quaternion.identity, transform);
        SpriteRenderer sRend = currentPrefab.GetComponent<SpriteRenderer>();

        if (nodeTeam == NodeTeam.WHITE)
        {
            sRend.color = Color.white;
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            sRend.color = Color.black;
        }
    }

    public void CheckKnightMovement()
    {
        nodesToCheck.Clear();

        BoardManager board = GetComponentInParent<BoardManager>();
        List<Node> nodeList = board.GetNodeList();

        if (nodeTeam == NodeTeam.WHITE)
        {
            for (int i = 0; i < nodeList.Count; ++i)
            {
                //Checking relevant positions for the knight.
                Node node = nodeList[i];
                
                //Checking for knight movables.
                if (node.currentPos.x == currentPos.x + 2 && node.currentPos.y == currentPos.y + 1 ||
                    node.currentPos.x == currentPos.x + 2 && node.currentPos.y == currentPos.y - 1 ||
                    node.currentPos.x == currentPos.x + 1 && node.currentPos.y == currentPos.y + 2 ||
                    node.currentPos.x == currentPos.x + 1 && node.currentPos.y == currentPos.y - 2 ||
                    node.currentPos.x == currentPos.x - 2 && node.currentPos.y == currentPos.y + 1 ||
                    node.currentPos.x == currentPos.x - 2 && node.currentPos.y == currentPos.y - 1 ||
                    node.currentPos.x == currentPos.x - 1 && node.currentPos.y == currentPos.y + 2 ||
                    node.currentPos.x == currentPos.x - 1 && node.currentPos.y == currentPos.y - 2)
                {
                    if (node.nodeTeam != NodeTeam.WHITE)
                    {
                        nodesToCheck.Add(node);
                    }
                }
                
            }
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            for (int i = 0; i < nodeList.Count; ++i)
            {
                //Checking relevant positions for the knight.
                Node node = nodeList[i];

                //Checking for knight movables.
                if (node.currentPos.x == currentPos.x + 2 && node.currentPos.y == currentPos.y + 1 ||
                node.currentPos.x == currentPos.x + 2 && node.currentPos.y == currentPos.y - 1 ||
                node.currentPos.x == currentPos.x + 1 && node.currentPos.y == currentPos.y + 2 ||
                node.currentPos.x == currentPos.x + 1 && node.currentPos.y == currentPos.y - 2 ||
                node.currentPos.x == currentPos.x - 2 && node.currentPos.y == currentPos.y + 1 ||
                node.currentPos.x == currentPos.x - 2 && node.currentPos.y == currentPos.y - 1 ||
                node.currentPos.x == currentPos.x - 1 && node.currentPos.y == currentPos.y + 2 ||
                node.currentPos.x == currentPos.x - 1 && node.currentPos.y == currentPos.y - 2)
                {
                    if (node.nodeTeam != NodeTeam.BLACK)
                    {
                        nodesToCheck.Add(node);
                    }
                }
            }
        }

        PaintMovables();
    }

    public void CheckRook()
    {
        currentPrefab = Instantiate(rookPrefab, transform.position, Quaternion.identity, transform);
        SpriteRenderer sRend = currentPrefab.GetComponent<SpriteRenderer>();

        if (nodeTeam == NodeTeam.WHITE)
        {
            sRend.color = Color.white;
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            sRend.color = Color.black;
        }
    }

    public void CheckQueen()
    {
        currentPrefab = Instantiate(queenPrefab, transform.position, Quaternion.identity, transform);
        SpriteRenderer sRend = currentPrefab.GetComponent<SpriteRenderer>();

        if (nodeTeam == NodeTeam.WHITE)
        {
            sRend.color = Color.white;
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            sRend.color = Color.black;
        }
    }

    public void CheckKing()
    {
        currentPrefab = Instantiate(kingPrefab, transform.position, Quaternion.identity, transform);
        SpriteRenderer sRend = currentPrefab.GetComponent<SpriteRenderer>();

        if (nodeTeam == NodeTeam.WHITE)
        {
            sRend.color = Color.white;
        }
        else if (nodeTeam == NodeTeam.BLACK)
        {
            sRend.color = Color.black;
        }
    }

    public void CheckEmpty()
    {
        nodeType = NodeType.NONE;
        nodeTeam = NodeTeam.NONE;

        moveCounter = 0;

        UnPaintMovables();
        
        Destroy(currentPrefab);
        currentPrefab = null;
    }

    public void UnPaintMovables()
    {
        for (int i = 0; i < nodesToCheck.Count; ++i)
        {
            Node node = nodesToCheck[i];

            SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.white;
        }

        SpriteRenderer sRendThis = GetComponent<SpriteRenderer>();
        sRendThis.material.color = Color.white;
    }

    public void PaintMovables()
    {
        for (int i = 0; i < nodesToCheck.Count; ++i)
        {
            Node node = nodesToCheck[i];

            SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.green;
        }
    }

    public void PaintSelected()
    {
        SpriteRenderer sRendThis = GetComponent<SpriteRenderer>();
        sRendThis.material.color = Color.yellow;
    }

    public void UpdateMoveCounter(int value)
    {
        moveCounter += value;
    }

    public Vector2 GetNodePosition()
    {
        return currentPos;
    }

    public List<Node> GetNodesToCheck()
    {
        return nodesToCheck;
    }

    public int GetMoveCounter
    {
        get
        {
            return moveCounter;
        }
    }
}
