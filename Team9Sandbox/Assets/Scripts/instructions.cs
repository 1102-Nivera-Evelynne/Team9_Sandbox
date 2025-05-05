using UnityEngine;

public class instructions : MonoBehaviour
{
    public GameObject instructionsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void hideInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
