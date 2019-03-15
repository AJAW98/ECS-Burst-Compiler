using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Enemy : MonoBehaviour {

    public float speed;

}

class EnemySystem : ComponentSystem
{

    struct Components
    {
        public Enemy enemy;
        public Transform transform;
    }

    protected override void OnUpdate()
    {
        foreach (var e in GetEntities<Components>())
        {
            e.transform.Rotate(0f, e.enemy.speed * Time.deltaTime, 0f);
        }
    }
}
