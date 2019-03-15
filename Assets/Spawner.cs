/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class Spawner : MonoBehaviour {


    private static EntityManager entityManager;
    private static MeshInstanceRenderer cubeRenderer;
    private static EntityArchetype cubeArchetype;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
        cubeArchetype = entityManager.CreateArchetype();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitalizeWithScene()
    {
        cubeRenderer = GameObject.FindObjectOfType<MeshInstanceRendererComponent>().Value;

        for (int i = 0; i < 100000; i++)
        {
            SpawnCube();
        }
    }


    private static void SpawnCube()
    {
        Entity cubeEntity = entityManager.CreateEntity(cubeArchetype);

        float2 direction = new float2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

        entityManager.SetComponentData(cubeEntity, new Position { Value = new float3(0, 0, 0) });
        entityManager.SetComponentData(cubeEntity, new Heading { Value = direction });
        entityManager.SetComponentData(cubeEntity, new MoveSpeed { speed = 1 });
        
        entityManager.AddSharedComponentData(cubeEntity, cubeRenderer);
        
    }


}
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Samples.Common
{
    public class Spawner : MonoBehaviour
    {
        public GameObject prefab;
        public int count = 10000;
        public float radius = 4.0F;
        public int transformsPerHierarchy = 500;
        public enum ActivateMode { None, ActivateDeactivateAll }
        public ActivateMode activateMode;
        public Vector2 nextSpawnPos = Vector2.zero;
        public float spawnOffset = 1f;

        private List<GameObject> roots = new List<GameObject>();

        void OnEnable()
        {
            Profiler.BeginSample("Spawn '" + prefab.name + "'");
            GameObject root = null;

            for (int i = 0; i != count; i++)
            {
                if (transformsPerHierarchy != 0 && i % transformsPerHierarchy == 0)
                {
                    root = new GameObject("Chunk " + i);
                    root.transform.hierarchyCapacity = transformsPerHierarchy;
                    roots.Add(root);
                }

                //Instantiate(prefab, Random.insideUnitSphere * radius + transform.position, Random.rotation, root.transform);
                Instantiate(prefab, nextSpawnPos, Random.rotation, root.transform);
                nextSpawnPos = new Vector2(nextSpawnPos.x + spawnOffset, 0f);
            }

            Profiler.EndSample();
        }

        void Update()
        {
            if (activateMode == ActivateMode.ActivateDeactivateAll)
            {
                foreach (var go in roots)
                    go.SetActive(false);
                foreach (var go in roots)
                    go.SetActive(true);
            }
        }
    }
}