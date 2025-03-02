using Entity.InGameObject.Base;
using UnityEngine;

namespace Entity.InGameObject.Controllers.Input
{
    public class ClickController : MonoBehaviour
    {
        public InGameObjectBase gameObjectBase;
        public void OnMouseDown()
        {
            gameObjectBase.CanClickToGameObject();
        }
    }

}