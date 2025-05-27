using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckAllTasks : MonoBehaviour
{
    public TMP_Text messageText; 
    public AudioSource audioSource; 
    public float messageDuration = 5f; 
    public AudioSource backgroundMusicSource;
    public Animator animator1;
    public Animator animator2;
    public AudioSource monsterAudioSource;
    public AudioClip newAudioClip;
    
    public IEnumerator ShowMessageAndPlayMusic()
    {
        
        messageText.text = "Doors opened! Run!\nbut i'll catch you!";        
        messageText.gameObject.SetActive(true);
        
        
        if (audioSource != null)
        {
            backgroundMusicSource.Stop();
            audioSource.Play();
        }
        
        if (monsterAudioSource != null)
        {
            monsterAudioSource.Stop();
            monsterAudioSource.clip = newAudioClip;
            monsterAudioSource.Play(); 
        }
        
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            light.enabled = false;
        }   
        
        yield return new WaitForSeconds(5f); 

        if (animator1 != null)
        {
            animator1.SetTrigger("doorOpen");
        }
        if (animator2 != null)
        {
            animator2.SetTrigger("doorOpen");
        }
        
        yield return new WaitForSeconds(messageDuration);

        
        messageText.gameObject.SetActive(false);
    }
}