﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Building {
    public float matureTime = 15; // time in seconds

    public struct Corn {
        public float plantTime;
        public int maxStage;
        public int stage;
        public VoxelModel model;
    }

    Corn[] corn;

    // Start is called before the first frame update
    new void Start() {
        base.Start();
        VoxelModel[] models = GetComponentsInChildren<VoxelModel>();

        corn = new Corn[models.Length];

        for (int i = 0; i < corn.Length; i++) {
            corn[i].model = models[i];
            corn[i].maxStage = models[i].VoxelsList[models[i].VoxelsList.Count - 1].depth;
            Replant(i);
        }
    }

    void Replant(int i) {
        corn[i].plantTime = Time.time;
        corn[i].stage = -1; // To trigger a (re)draw, since the actual stage is 0
    }

    // Wether at least one corn is mature
    public bool HasCorn() {
        return corn.Any(t => t.plantTime + matureTime <= Time.time);
    }

    // Tries to harvest one corn. Returns true if it succeeded. Automatically replant.
    public bool Harvest() {
        for (int i = 0; i < corn.Length; i++) {
            if (corn[i].plantTime + matureTime <= Time.time) {
                Replant(i);
                return true;
            }
        }
        return false;
    }

    private void Update() {
        // Recompute stages, updating models if need be
        for (int i = 0; i < corn.Length; i++) {
            int stage = (int)(corn[i].maxStage * Mathf.Min(1, (Time.time - corn[i].plantTime) / matureTime));
            if (stage != corn[i].stage) {
                corn[i].stage = stage;
                // Redraw
                corn[i].model.GenerateMesh(stage);
            }
        }
    }
}
