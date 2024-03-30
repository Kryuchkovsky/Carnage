using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Items.Requests
{
    public struct ExperienceEssenceCreationRequest : IRequestData
    {
        public Vector3 Position;
        public float ExperienceAmount;
    }
}