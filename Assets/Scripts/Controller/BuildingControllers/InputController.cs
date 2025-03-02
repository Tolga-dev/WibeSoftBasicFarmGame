using System;
using Controller.Base;
using Entity.InGameObject.Base;
using Managers;
using Managers.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    [Serializable]
    public class InputController : ControllerBase
    {
        public void HandleInput(InGameObjectBase temp, GridLayout gridLayout, Camera cam, ref Vector3Int prevPos, BuildingPlacementController buildingPlacementController)
        {
            if (!temp)
                return;

            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(0))
                    return;

                if (!temp.isPlaced)
                {
                    var touchPos = cam.ScreenToWorldPoint(Input.mousePosition);
                    touchPos.z = 0;
                    var cellPos = gridLayout.LocalToCell(touchPos);

                    if (prevPos != cellPos)
                    {
                        temp.transform.position = gridLayout.CellToLocalInterpolated(cellPos);
                        prevPos = cellPos;
                        buildingPlacementController.FollowBuilding(temp, gridLayout, BuildingManager.tileMapController);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (temp.CanBePlace())
                    temp.Place();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                BuildingManager.CleanObjects();
            }
        }
        public BuildingManager BuildingManager => (BuildingManager)ManagerBase;
    }

}