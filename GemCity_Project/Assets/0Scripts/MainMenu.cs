using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public string levelToLoad;
    private void Start()
    {
		GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MusicClass>().PlayMusic();
		//levelToLoad = "Level1";

	}
    //public SceneFader sceneFader;

    public void Play()
	{
		SceneManager.LoadScene(levelToLoad);
	}

	public void Quit()
	{
		Debug.Log("Exciting...");
		Application.Quit();
	}

}