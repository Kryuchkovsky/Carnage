using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Core
{
    [RequireComponent(typeof(Collider))]
    public class LinkedCollider : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        
        public void Initiate(Entity entity, int layer)
        {
            Entity = entity;
            gameObject.layer = layer;
        }
    }
}