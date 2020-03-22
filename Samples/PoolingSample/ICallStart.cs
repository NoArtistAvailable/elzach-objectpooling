using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace elZach.Samples
{
    public class ICallStart : MonoBehaviour
    {
        public int publicProperty = 0;
        public new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        void Start()
        {
            Debug.Log("I call start!");
            publicProperty++;
            renderer.material.color = Random.ColorHSV();
        }

        private void Reset(ICallStart source)
        {
            if(source)
                publicProperty = source.publicProperty;
        }

    }
}
