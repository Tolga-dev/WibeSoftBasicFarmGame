using Entity.InGameObject.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entity.InGameObject.Controllers.InputController
{
    public class ClickController : MonoBehaviour
    {
        public InGameObjectBase gameObjectBase;
        public void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject() || 
                (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))) 
                return;

            gameObjectBase.CanClickToGameObject();
        }

    }

}