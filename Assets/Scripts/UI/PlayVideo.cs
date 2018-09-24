using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Source https://www.youtube.com/watch?v=nGA3jMBDjHk&t=220s

public class PlayVideo : MonoBehaviour 
{
    private RawImage image;
    private VideoPlayer movie;
    private bool isPlaying;

    void OnEnable()
    {
        Application.runInBackground = true;
        movie = GetComponent<VideoPlayer>();
        image = GetComponent<RawImage>();
        movie.source = VideoSource.VideoClip;
        StartCoroutine(PrepareVideo());
    }

    private IEnumerator PrepareVideo()
    {
        movie.Prepare();
        WaitForSeconds wait = new WaitForSeconds(1);
        while(!movie.isPrepared)
        {
            yield return wait;
        }

        Debug.Log("Video is Prepared");
        image.texture = movie.texture;
        movie.Play();
    }
}

