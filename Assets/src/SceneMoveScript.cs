using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToSoloMode()
    {
        SceneManager.LoadScene("SoloMode");
    }

    public void moveToWorldBattleMode()
    {
        SceneManager.LoadScene("WorldBattleMode");
    }

    public void moveToDeveloperBattleMode()
    {
        SceneManager.LoadScene("DeveloperBattleMode");
    }

    public void moveToExtra()
    {
        SceneManager.LoadScene("Extra");
    }

    public void moveToCredit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void moveToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
