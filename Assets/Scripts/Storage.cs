﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource { Food, Wood }

public class Storage : Building {
    private static int warehouseCapacity = 36;
    private Transform[] elements = new Transform[warehouseCapacity];
    private Resource elementType;
    public Transform resourcePrefab;

    private new void Start() {
        base.Start();
        AddElement(Resource.Wood);
        AddElement(Resource.Wood);
        AddElement(Resource.Wood);
        AddElement(Resource.Wood);
        AddElement(Resource.Wood);
        AddElement(Resource.Wood);
    }
    
    public bool AddElement(Resource elemType) { //False : Full warehouse or bad element type
        if (IsEmpty()) {
            elementType = elemType; 
        }
        if (elemType == elementType) {
            for (int i = 0; i < warehouseCapacity; i++) {
                if (elements[i] == null) {
                    elements[i] = CreateElement(i);
                    return true; 
                }
            }
        }
        return false; 
    }

    public bool RemoveElement(Resource elemType) {
        if (IsEmpty()) return false;
        if (elemType == elementType) {
            for (int i = warehouseCapacity - 1; i >= 0; i--) {
                if (elements[i] != null) {
                    EraseElement(i);
                    return true; 
                } 
            }
        }
        return false; 
    }
    
    private bool IsEmpty() {
        for (int i = 0; i < warehouseCapacity; i++) {
            if (elements[i]) return false; 
        }
        return true; 
    }

    public bool Has(Resource elemType) {
        return !IsEmpty() && elementType == elemType;
    }

    public bool CanStore(Resource elemType) {
        return IsEmpty() || elementType == elemType;
    }

    private Transform CreateElement(int index) {
        var grid = WorldGrid.instance;
        var wPos = grid.RealPos(pos, 0, false);
        var i = index / 9;
        var j = index % 9;
        
        var offset = new Vector3(1, 1 + (i % 2) * 8 + (j % 3) * 2, 1 + (i / 2) * 8 + (j / 3) * 2);
        wPos += offset / 2.5f;
        return Instantiate(resourcePrefab, wPos, Quaternion.identity);
    }
    
    private void EraseElement(int index) {
        Destroy(elements[index].gameObject);
    }
}