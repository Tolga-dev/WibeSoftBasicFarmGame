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

        public void StartATime(ItemSo itemSo,float time, Action endOfTimeAction = null)
        {
            timerPanel.SetActive(true);
            itemNameText.text = itemSo.itemName;
            StartCoroutine(Time(time, endOfTimeAction));
        }
        private IEnumerator Time(float finishTimeToFinish, Action endOfTimeAction = null)
        {
            var time = finishTimeToFinish;
            var interval = time / 100f;
            var currentTime = 0f;

            slider.maxValue = time;
            slider.value = 0f;

            while (currentTime < time)
            {
                currentTime += interval;
                slider.value = currentTime;
                timerText.text = (time - currentTime).ToString("F2", CultureInfo.InvariantCulture);
                yield return new WaitForSeconds(interval);
            }
            
            endOfTimeAction?.Invoke();
        }

        public void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}