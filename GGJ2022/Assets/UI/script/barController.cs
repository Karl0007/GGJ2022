using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class barController : MonoBehaviour
    {
        public Image energyPointImage;
        public Image energyPointEffect;
        public Image atokinEnergyPointEffect;
        
        public Func<float> getValue;
        [SerializeField] private float spendSpeed = 0.00001f;

        private void Update()
        {
            if (energyPointImage != null && getValue() != 1)
            {
                StartCoroutine(UpdateHpCo());
            }
			if (atokinEnergyPointEffect != null)
			{
				atokinEnergyPointEffect.fillAmount = getValue();
			}
        }

        IEnumerator UpdateHpCo()
        {
            energyPointImage.fillAmount = getValue();
            while (energyPointEffect.fillAmount >= energyPointImage.fillAmount)
            {
                energyPointEffect.fillAmount -= spendSpeed * (energyPointEffect.fillAmount - energyPointImage.fillAmount);
                yield return new WaitForSeconds(spendSpeed);
            }
            if (energyPointEffect.fillAmount < energyPointImage.fillAmount)
            {
                energyPointEffect.fillAmount = energyPointImage.fillAmount;
            }
        }

    }
}