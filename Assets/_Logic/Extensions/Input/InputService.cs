using _GameLogic.Extensions.Patterns;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Logic.Extensions.Input
{
    public class InputService : SingletonBehavior<InputService>, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Vector2 Direction { get; private set; }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                Direction = (eventData.position - eventData.pressPosition).normalized;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Direction = new Vector2();
        }
    }
}