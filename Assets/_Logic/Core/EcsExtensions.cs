using _GameLogic.Extensions;
using _Logic.Core.Components;
using Scellecs.Morpeh;

namespace _Logic.Core
{
    public static class EcsExtensions
    {
        public static bool TryGetDistanceBetweenClosestPoints(Entity entity1, Entity entity2, out float distance)
        {
            if (entity1.IsNullOrDisposed() || entity2.IsNullOrDisposed() || !entity1.Has<ColliderComponent>() || !entity2.Has<ColliderComponent>())
            {
                distance = 0;
                return false;
            }
            
            var colliderComponent1 = entity1.GetComponent<ColliderComponent>();
            var colliderComponent2 = entity2.GetComponent<ColliderComponent>();
            distance = ExtraMethods.GetDistanceBetweenClosestPointsOfColliders(colliderComponent1.Value, colliderComponent2.Value);
            return true;
        }
    }
}