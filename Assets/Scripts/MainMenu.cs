using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text headLineText;
    private Coroutine timerCoroutine;
    private float timerLength;

    private void Start()
    {
        timerCoroutine = StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            headLineText.text = "hello world";
            yield return new WaitForSeconds(1);
            headLineText.text = "Main menu";
        }
    }
}
