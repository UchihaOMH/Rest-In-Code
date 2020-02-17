using UnityEngine;

public class Test : MonoBehaviour
{
    private string curr = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (curr != "TE")
        {
            curr = "TE";
            Debug.Log("Trigger Enter");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (curr != "TS")
        {
            curr = "TS";
            Debug.Log("Trigger Stay");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (curr != "TEx")
        {
            curr = "TEx";
            Debug.Log("Trigger Exit");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (curr != "CE")
        {
            curr = "CE";
            Debug.Log("Collision Enter");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (curr != "CS")
        {
            curr = "CS";
            Debug.Log("Collision Stay");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (curr != "CEx")
        {
            curr = "Cex";
            Debug.Log("Collision Exit");
        }
    }
}
