﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VoxelModel))]
public class Meteor : MonoBehaviour {

    private Vector3 velocity;
    public float verticalVelocity;
    public float horizontalVelocity;
    public float noise;
    public float fallDelay;
    public float radius;

    public float woodDestructionFaithBonus;
    
    private VoxelModel model;
    
    private void Start() {
        model = GetComponent<VoxelModel>();
        velocity = new Vector3(
            horizontalVelocity * (Random.value-.5f) * 2, 
            verticalVelocity, 
            horizontalVelocity * (Random.value-.5f) * 2);
        transform.position += new Vector3(
            noise * (Random.value-.5f) * 2, 
            0, 
            noise * (Random.value-.5f) * 2);
        transform.position -= velocity / fallDelay;
    }

    private void Update() {
        transform.position += velocity * Time.deltaTime;
        var faith = FindObjectOfType<Faith>();
        if (transform.position.y < .1f) {
            foreach(var worker in FindObjectsOfType<Worker>())
                if (Vector3.ProjectOnPlane(worker.transform.position - transform.position, Vector3.up).sqrMagnitude <
                    radius * radius)
                    worker.Die("suffered your godly wrath");

            foreach (var b in WorldGrid.instance.buildings) {
                var broken = false;
                for (var y = b.pos.y; y < b.pos.y + b.size.y; y++)
                for (var x = b.pos.x; x < b.pos.x + b.size.x; x++)
                    if (!broken &&
                        Vector3.ProjectOnPlane(WorldGrid.instance.RealPos(new Vector2Int(x, y)) -
                                               transform.position, Vector3.up).sqrMagnitude < radius * radius) {
                        faith.value += b.woodProvided * woodDestructionFaithBonus;
                        b.GetComponent<VoxelModel>().Explode();
                        broken = true;
                    }
            }
                
            model.Explode();
        }
    }
}
