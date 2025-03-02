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
            Debug.Log("Timer Started " + time);
            timerPanel.SetActive(true);
            itemNameText.text = itemSo.itemName;
            StartCoroutine(Timer(time, endOfTimeAction));
        }
        private IEnumerator Timer(float finishTimeToFinish, Action endOfTimeAction = null)
        {
            var time = finishTimeToFinish;
            slider.maxValue = time;
            slider.value = 0f;

            var startTime = Time.time;
            var endTime = startTime + time;

            while (Time.time < endTime)
            {
                var currentTime = Time.time - startTime;
                slider.value = currentTime;
                timerText.text = (time - currentTime).ToString("F2", CultureInfo.InvariantCulture);
                yield return null;
            }

            slider.value = time;
            timerText.text = "0.00";
            endOfTimeAction?.Invoke();
        }


        public void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}