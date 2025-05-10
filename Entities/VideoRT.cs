using UnityEngine;
using UnityEngine.PSVita;

public class VideoRT : MonoBehaviour
{
    public string m_MoviePath;
    public RenderTexture m_RenderTexture;    
	bool m_IsPlaying = false;

    void Start()
    {
        PSVitaVideoPlayer.Init(m_RenderTexture);
    }

    public void PlayVideo()
    {
        PSVitaVideoPlayer.Play(m_MoviePath,
            PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.RenderToTexture);
    }

    public void PlayVideo(string movieName)
    {
        PSVitaVideoPlayer.Play($"StreamingAsset/{movieName}.mp4",
            PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.RenderToTexture);
    }

    public void StopVideo() { PSVitaVideoPlayer.Stop(); }

    void OnPreRender()
    {
        PSVitaVideoPlayer.Update();
    }
    
	void OnMovieEvent(int eventID)
	{
		PSVitaVideoPlayer.MovieEvent movieEvent = (PSVitaVideoPlayer.MovieEvent)eventID;
		switch (movieEvent)
		{
			case PSVitaVideoPlayer.MovieEvent.PLAY:
				m_IsPlaying = true;
				break;

			case PSVitaVideoPlayer.MovieEvent.STOP:
				m_IsPlaying = false;
				break;
		}
	}   
}
