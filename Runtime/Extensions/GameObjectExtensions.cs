﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using elZach.Common;

//namespace elZach.Common
//{
public static class GameObjectExtensions
{
    public static GameObject Copy(this GameObject source)
    {
        return GameObject.Instantiate(source);
    }

    public static void Reset(this Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    public static void Reset(this Rigidbody rb, Rigidbody _)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    //---------Pool spawning---------//
    public static GameObject Spawn(this GameObject prefab)
    {
        return Pool.Spawn(prefab, null);
    }
    public static GameObject Spawn(this GameObject prefab, Transform parent)
    {
        return Pool.Spawn(prefab, parent);
    }

    public static GameObject SpawnAndStart(this GameObject prefab)
    {
        return Pool.SpawnAndStart(prefab, null);
    }
    public static GameObject SpawnAndStart(this GameObject prefab, Transform parent)
    {
        return Pool.SpawnAndStart(prefab, parent);
    }

    public static GameObject SpawnAndReset(this GameObject prefab, Transform parent, Component toReset)
    {
        return Pool.SpawnAndReset(prefab, parent, toReset);
    }

    public static GameObject SpawnAndReset(this GameObject prefab, Transform parent, Component[] toReset)
    {
        return Pool.SpawnAndReset(prefab, parent, toReset);
    }

    public static void Despawn(this GameObject go)
    {
        Pool.Despawn(go);
    }
}
//}