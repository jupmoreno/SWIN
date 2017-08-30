﻿using UnityEngine;

[System.Serializable]
public class Boundary1D
{
    public float XMin;
    public float XMax;
}

public class SpaceShipController : MonoBehaviour 
{
    public Transform ShotSpawn;
    public Boundary1D Boundary;

    private const float Speed = 0.08f;
    private const float FireRate = 0.2F;

    private float _remainingCoolDownTime;

    private void Update ()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        Vector2 pos = transform.position;
        // Move input.
        // TODO: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = Move(pos.x - Speed, pos.y);
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = Move(pos.x + Speed, pos.y);
        }

        // Only shoot if we are requested to and if the weapon has finished cooling down.
        _remainingCoolDownTime -= Time.deltaTime;
        if (Input.GetButton("Fire1") && _remainingCoolDownTime < 0) Shoot();
    }

    private Vector2 Move(float newX, float newY)
    {
        return new Vector2(Mathf.Clamp(newX, Boundary.XMin, Boundary.XMax), newY);
    }

    private void Shoot()
    {
        // Get the bolt to shoot.
        var boltGameObject = StaticShotPool.Instance.GetShot();
        if (boltGameObject == null) return; // There was no bolt => cannot shoot.
        
        // Position the bolt to be correctly fired & enable it
        boltGameObject.transform.position = ShotSpawn.position;
        boltGameObject.transform.rotation = ShotSpawn.rotation;
        boltGameObject.SetActive(true);
        
        // Cold down the weapon
        _remainingCoolDownTime = FireRate;
    }
}