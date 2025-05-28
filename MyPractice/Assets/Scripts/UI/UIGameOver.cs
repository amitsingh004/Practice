
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void onRestartButtonClicked()
    {
        
        GameEvents.TriggerGameRestart();
        
    }
}
