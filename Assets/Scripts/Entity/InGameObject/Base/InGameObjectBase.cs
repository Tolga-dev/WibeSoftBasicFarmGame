using System;
using System.Collections;
using System.Globalization;
using Entity.InGameObject.Controllers.UI;
using Managers;
using So;
using So.GameObjectsSo.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.InGameObject.Base
{
    public class InGameObjectBase : MonoBehaviour
    {
        public SpriteRenderer itemSpriteRenderer;
        public TimerController timerPanel;
        
        public bool isPlaced;
        public BoundsInt area;

        public ItemSo itemSo;
        
        public bool CanBePlace()
        {
            var posInt = BuildingManager.gridLayout.WorldToCell(transform.position);
            var areaTemp = area;
            areaTemp.position = posInt;

            if (BuildingManager.tileMapController.CanTakeArea(areaTemp))
                return true;

            return false;
        }

        public virtual void Place()
        {
            var posInt = BuildingManager.gridLayout.WorldToCell(transform.position);
            var areaTemp = area;
            areaTemp.position = posInt;
            isPlaced = true;
            BuildingManager.tileMapController.TakeArea(areaTemp);
        }
        protected void EndOfTimeAction()
        {
            timerPanel.gameObject.SetActive(false);
            itemSpriteRenderer.sprite = itemSo.itemSprite;
        }

        public virtual bool CanClickToGameObject()
        {
            return true;
        }
        protected GameManager GameManager => GameManager.Instance;
        public BuildingManager BuildingManager => GameManager.buildingManager;

    }
}