using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public static Raycaster Instance;

    class RaySetup
    {
        public Vector3 from;
        public Vector3 dir;
        public float maxDistance;
        public LayerMask mask;
        public Action<RaycastHit> action;

        public RaySetup(Vector3 from, Vector3 dir, float maxDistance, LayerMask mask, Action<RaycastHit> action)
        {
            this.from = from;
            this.dir = dir;
            this.maxDistance = maxDistance;
            this.mask = mask;
            this.action = action;
        }
    }

    List<RaySetup> queue = new List<RaySetup>();

    public static void CastAndDo(Vector3 from, Vector3 dir, float maxDistance, LayerMask mask, Action<RaycastHit> action)
    {
        if(!Instance)
        {
            var init = new GameObject("Job Raycaster");
            Instance = init.AddComponent<Raycaster>();
        }
        Instance.queue.Add(new RaySetup(from, dir, maxDistance, mask, action));
    }

    public static void ExecuteQueue()
    {
        var queue = Instance.queue;
        var results = new NativeArray<RaycastHit>(queue.Count, Allocator.Temp);
        var commands = new NativeArray<RaycastCommand>(queue.Count, Allocator.Temp);
        for(int i=0; i< queue.Count; i++)
        {
            var item = queue[i];
            commands[i] = new RaycastCommand(item.from, item.dir, item.maxDistance, item.mask);
        }
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));
        handle.Complete();
        for(int i=0; i < queue.Count; i++)
        {
            queue[i].action.Invoke(results[i]);
        }
        results.Dispose();
        commands.Dispose();
        queue.Clear();
    }

    private void Update()
    {
        if (queue.Count > 0)
            ExecuteQueue();
    }


    private void RaycasExample()
    {
        // Perform a single raycast using RaycastCommand and wait for it to complete
        // Setup the command and result buffers
        var results = new NativeArray<RaycastHit>(1, Allocator.Temp);

        var commands = new NativeArray<RaycastCommand>(1, Allocator.Temp);

        // Set the data of the first command
        Vector3 origin = Vector3.forward * -10;

        Vector3 direction = Vector3.forward;

        commands[0] = new RaycastCommand(origin, direction);

        // Schedule the batch of raycasts
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));

        // Wait for the batch processing job to complete
        handle.Complete();

        // Copy the result. If batchedHit.collider is null there was no hit
        RaycastHit batchedHit = results[0];

        // Dispose the buffers
        results.Dispose();
        commands.Dispose();
    }
}
