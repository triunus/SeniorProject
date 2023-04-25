using UnityEngine.SceneManagement;

namespace ChangeSceneManager
{
    public class ChangeScene
    {
        public void SceneLoader(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
