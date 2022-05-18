using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelStats : MonoBehaviour
{
    public Text wavesText;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //wavesText.text = PlayerStats.waves.ToString();
        StartCoroutine(AnimateText());
    }
    IEnumerator AnimateText()
    {
        int round = 0;
        wavesText.text = "0";
        yield return new WaitForSeconds(0.2f);
        while (round < PlayerStats.waves)
        {
            round++;

            wavesText.text = round.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
