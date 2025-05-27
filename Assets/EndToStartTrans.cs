using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EndToStartTrans : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Timer());
    }
    
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5);
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }
}
