using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ChangeScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "GameScene"; 

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished; 
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); 
    }
}
