using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] hearts;
    public Sprite halfFullHeart;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;

    void Start()
    {
        InitHearts();
    }
    public void InitHearts()
    {
        for(int i = 0; i < heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }
    public void UpdateHearts()
    {
        float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for(int i = 0; i< heartContainers.initialValue; i++)
        {
            if(i <= tempHealth-1)
            {//full
                hearts[i].sprite = fullHeart;
            }else if (i >= tempHealth)
            {
                //empty
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //half
                hearts[i].sprite = halfFullHeart;
            }
        }
    }
}
