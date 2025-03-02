using System;
using System.Collections;
using System.Globalization;
using Controller;
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
        
        public bool CanBePlace() => TileMapController.CanTakeArea(GetArea());
        
        public virtual void Place()
        {
            isPlaced = true;
            TileMapController.TakeArea(GetArea());
        }
        
        protected virtual void EndOfTimeAction()
        {
            timerPanel.gameObject.SetActive(false);
            itemSpriteRenderer.sprite = itemSo.itemSprite;
        }

        public virtual bool CanClickToGameObject() => true;
        
        private BoundsInt GetArea()
        {
            var tempArea = area;
            tempArea.position = BuildingManager.gridLayout.WorldToCell(transform.position);
            return tempArea;
        }
        
        protected TileMapController TileMapController => BuildingManager.tileMapController;
        protected GameManager GameManager => GameManager.Instance;
        public BuildingManager BuildingManager => GameManager.buildingManager;

    }
}