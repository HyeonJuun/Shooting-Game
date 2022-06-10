using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject clear_stage_ui;
    [SerializeField]
    private GameObject start_stage_ui;
    [SerializeField]
    private Text next_stage_text;
    [SerializeField]
    private Text start_stage_text;
    [SerializeField]
    private Image hp_bar;
    public IEnumerator ActiveClearUIDuringTime(float time, string text)
    {
        next_stage_text.text = text;

        clear_stage_ui.SetActive(true);
        yield return new WaitForSeconds(time);
        clear_stage_ui.SetActive(false);
    }
    public IEnumerator ActiveStartUIDuringTime(float time, string text)
    {
        start_stage_text.text = text;

        start_stage_ui.SetActive(true);
        yield return new WaitForSeconds(time);
        start_stage_ui.SetActive(false);
    }

    public void UpdateHp(float value)
    {
        hp_bar.fillAmount = value;
    }
}
