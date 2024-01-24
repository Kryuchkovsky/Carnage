using _Logic.Core;
using _Logic.Gameplay.Input.Components;
using Scellecs.Morpeh;
using UnityEngine.EventSystems;

namespace _Logic.Gameplay.Input
{
    public class InputCatcher : GameObjectProvider<InputDataComponent>, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            ref var inputDataComponent = ref Entity.GetComponent<InputDataComponent>();
            
            if (eventData.dragging)
            {
                inputDataComponent.Direction = (eventData.position - eventData.pressPosition).normalized;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ref var inputDataComponent = ref Entity.GetComponent<InputDataComponent>();
            inputDataComponent = new InputDataComponent();
        }
    }
}