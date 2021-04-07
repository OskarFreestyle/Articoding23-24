﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    public enum BoardCellState
    {
        NONE,
        FREE,
        PARTIALLY_OCUPPIED,
        OCUPPIED
    }

    private int x;
    private int y;
    private BoardObject placedObject;

    private BoardCellState state = BoardCellState.NONE;

    [SerializeField] private Renderer cellRenderer;

    private void Awake()
    {
        SetState(BoardCellState.FREE);
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        transform.position = new Vector2(x, y);
    }

    public void SetPosition(Vector2Int position)
    {
        x = position.x;
        y = position.y;
        transform.position = new Vector2(x, y);
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(x, y);
    }

    public bool PlaceObject(BoardObject boardObject)
    {
        if (placedObject != null) return false; // Cannot place, cell ocuppied

        placedObject = boardObject;
        placedObject.transform.position = transform.position;
        SetState(BoardCellState.OCUPPIED);
        return true;
    }

    //Removes the reference to the object and optionally delete it
    public bool RemoveObject(bool deleteObject = true)
    {
        if (placedObject == null) return false; // Cannot remove, cell free

        if (deleteObject) Destroy(placedObject.gameObject);
        placedObject = null;
        SetState(BoardCellState.FREE);
        return true;
    }

    public BoardObject GetPlacedObject()
    {
        return placedObject;
    }

    public void SetState(BoardCellState state)
    {
        if (this.state == state) return;

        this.state = state;

#if CELL_COLORS //UNITY_EDITOR
        if (state == BoardCellState.FREE)
            cellRenderer.material.color = Color.green;
        else if (state == BoardCellState.PARTIALLY_OCUPPIED)
            cellRenderer.material.color = Color.yellow;
        else if (state == BoardCellState.OCUPPIED)
            cellRenderer.material.color = Color.red;
#endif
    }

    public BoardCellState GetState()
    {
        return state;
    }
}
