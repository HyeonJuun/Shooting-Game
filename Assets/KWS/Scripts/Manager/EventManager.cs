using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public void ReplayGame()
    {
        StartCoroutine(L());
    }
    IEnumerator L()
    {
        Destroy(ObjectPooling.instance.gameObject);
        yield return SceneManager.LoadSceneAsync("SampleScene");
    }
}
