using System;
using System.Collections;
using System.Globalization;
using So;
using So.GameObjectsSo.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Entity.InGameObject.Controllers.UI
{
    public class TimerController : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI itemNameText;
        public Slider slider;
        public GameObject timerPanel;

        public void StartATime(ItemSo itemSo, float leftTime = 0, Action endOfTimeAction = null, Action<float> updateTimeAction = null)
        {
            timerPanel.SetActive(true);
            itemNameText.text = itemSo.itemName;
            StartCoroutine(Timer(itemSo, leftTime, endOfTimeAction, updateTimeAction));
        }

        private IEnumerator Timer(ItemSo itemSo, float leftTime = 0, Action endOfTimeAction = null, Action<float> updateTimeAction = null)
        {
            var maxItemSo = itemSo.finishForProduction;
            slider.maxValue = maxItemSo;
            slider.value = maxItemSo - leftTime;

            var startTime = Time.time;
            var endTime = startTime + leftTime;

            while (Time.time < endTime)
            {
                var currentTimeLeft = endTime - Time.time;
                slider.value = maxItemSo - currentTimeLeft;
                timerText.text = currentTimeLeft.ToString("F2", CultureInfo.InvariantCulture);
                updateTimeAction?.Invoke(maxItemSo - currentTimeLeft);
                yield return null;  
            }

            slider.value = 0f;
            timerText.text = "0.00";
            endOfTimeAction?.Invoke();
        }
        
        public void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}