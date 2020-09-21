using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text nameText;
    public Text bodyText;
    public Animator animator;

    private Queue<string> senteces;
    

    // Start is called before the first frame update
    void Start()
    {
        senteces = new Queue<string>();
    }

    public void StartDialogue(Dialog dialog)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialog.name;

        senteces.Clear();
        foreach (string s in dialog.sentences)
        {
            senteces.Enqueue(s);
        }

        DisplayNJextSentence();
    }

    public void DisplayNJextSentence()
    {
        if(senteces.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = senteces.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        bodyText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            bodyText.text += letter;
            yield return null;
        }
    }
}
