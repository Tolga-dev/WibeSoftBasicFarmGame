using UnityEngine;
using UnityEngine.UI;

namespace Scenes.farm_game_tutorial
{
    public class PlantItem : MonoBehaviour
    {
        public PlantObject plant;

        public Text nameTxt;
        public Text priceTxt;
        public Image icon;

        public Image btnImage;
        public Text btnTxt;

        FarmManager fm;

        // Start is called before the first frame update
        void Start()
        {
            fm = FindObjectOfType<FarmManager>();
            InitializeUI();
        }

        public void BuyPlant()
        {
            Debug.Log("Bought " + plant.plantName);
            fm.SelectPlant(this);
        }

        void InitializeUI()
        {
            nameTxt.text = plant.plantName;
            priceTxt.text = "$" + plant.buyPrice;
            icon.sprite = plant.icon;
        }

    }
}
