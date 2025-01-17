using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile
{
    protected TileType type;

    protected int row;
    protected int column;
    protected Vector3 position;
    protected GameObject tileGameObject;
    protected Sprite tileSprite;
    protected Character currentInhabitant;

    public delegate bool IsChangeable();
    public IsChangeable isChangeable;

    protected Tile(int row, int column) 
    {
        this.row = row;
        this.column = column;
        this.position = Board.FindPosition(row, column);
        this.currentInhabitant = null;
        this.isChangeable = () => true;
    }

    protected void Init()
    {
        this.tileGameObject = CreateTileGameObject();
    }

    public int GetRow()
    {
        return row;
    }

    public int GetColumn()
    {
        return column;
    }

    public TileType GetTileType()
    {
        return type;
    }

    public Character GetCurrentInhabitant()
    {
        return currentInhabitant;
    }

    public GameObject GetTileGameObject() { return tileGameObject; }

    public Vector3 GetPosition() 
    {
        return this.position;
    }

    public bool IsOccupied()
    {
        return currentInhabitant != null;
    }

    public bool IsAccessible() 
    {
        return !IsOccupied() && type != TileType.EmptyTile;
    }

    public void SetCurrentInhabitant(Character character)
    {
        currentInhabitant = character;
    }

    public Tile Transform(TileType newTileType)
    {
        Tile newTile = TileFactory.CreateTile(newTileType, this.GetRow(), this.GetColumn());
        newTile.currentInhabitant = this.currentInhabitant;
        GameObject.Destroy(tileGameObject);

        GameplayEvents.TileHasChanged(this, newTile);

        return newTile;
    }

    private GameObject CreateTileGameObject()
    {
        GameObject tile = new GameObject();

        tile.name = this.GetType().Name;
        
        Quaternion startRotation = Quaternion.identity;

        SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.tileSprite;
        tile.transform.position = position;
        tile.transform.rotation = startRotation;

        tile.AddComponent<BoxCollider>();

        return tile;
    }
}