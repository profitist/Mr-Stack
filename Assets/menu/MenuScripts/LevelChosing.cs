using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChosing : MonoBehaviour
{
    public void StartLevel0()
    {
        SceneManager.LoadScene("tutorial");
    }
    
    public void StartLevel1()
    {
        SceneManager.LoadScene("level 1");
    }
    
    public void StartLevel2()
    {
        SceneManager.LoadScene("level 2");
    }
    
    public void StartLevel3()
    {
        SceneManager.LoadScene("level 3");
    }    
    
    public void StartLevel4()
    {
        SceneManager.LoadScene("level 4");
    }
    
    public void StartLevel5()
    {
        SceneManager.LoadScene("newLevel_by_Ivan");
    }
        
    public void StartLevel6()
    {
        SceneManager.LoadScene("finalLevel");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
