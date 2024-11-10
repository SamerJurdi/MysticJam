using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI QuitText;

    public void OnPointerDown(PointerEventData eventData)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuitText.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuitText.color = Color.white;
    }
}
