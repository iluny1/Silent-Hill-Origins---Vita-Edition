using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PSVita;

public class VideoFS : MonoBehaviour
{
    [SerializeField] private string m_MoviePath;
    [SerializeField] private float m_Volume = 1.0f;
    [SerializeField] private int m_AudioStreamIndex = 0;
    private PSVitaVideoPlayer.PlayParams vidParams;

    bool m_IsPlaying = false;

    void Awake()
    {
		// Initialise the video player.
        PSVitaVideoPlayer.Init(null);

		// Start the movie.		
		vidParams.volume = m_Volume;
		vidParams.loopSetting = PSVitaVideoPlayer.Looping.None;
		vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
		vidParams.audioStreamIndex = m_AudioStreamIndex;
	}

    public void PlayVideo()
    {
        PSVitaVideoPlayer.Stop();
        PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
        PSVitaVideoPlayer.Update();
    }

    public void PlayVideo(string fileName)
    {
        PSVitaVideoPlayer.Stop();
        PSVitaVideoPlayer.PlayEx($"StreamingAssets/{fileName}.mp4", vidParams);
        PSVitaVideoPlayer.Update();
    }

    public void StopVideo()
    {
        PSVitaVideoPlayer.Stop();
    }
    
    public void SetLoop(bool state)
    {
        if (state)
            vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
        else
            vidParams.loopSetting = PSVitaVideoPlayer.Looping.None;
    }

    void OnPostRender()
    {
        PSVitaVideoPlayer.Update();
    }

	void OnMovieEvent(int eventID)
	{
		PSVitaVideoPlayer.MovieEvent movieEvent = (PSVitaVideoPlayer.MovieEvent)eventID;
		switch(movieEvent)
		{
			case PSVitaVideoPlayer.MovieEvent.PLAY:
				m_IsPlaying = true;
				break;

			case PSVitaVideoPlayer.MovieEvent.STOP:
				m_IsPlaying = false;
				break;
		}
	}

    void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public float ReturnVideoLength()
    {
        return PSVitaVideoPlayer.videoDuration / 1000f;
    }

    public float ReturnVideoLengthMS()
    {
        return PSVitaVideoPlayer.videoDuration;
    }

    public float ReturnVideoCurrentMS()
    {
        return PSVitaVideoPlayer.videoTime;
    }

}
