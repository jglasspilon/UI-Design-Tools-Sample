using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public static class FileLoader
{
    /// <summary>
    /// Load a candidates image from the streaming assets
    /// </summary>
    /// <param name="target">target image object</param>
    /// <param name="imageType">image type whether it is a headshot, cutout, etc</param>
    /// <param name="candidateName">last name of candidate (needs to match the file names)</param>
    public static void LoadStreamingImage(Image target, E.CandidateImageType imageType, string candidateName)
    {
        string extension = imageType == E.CandidateImageType.Cutouts ? ".png" : ".jpg";
        string path = Application.streamingAssetsPath + "/Icons/" + imageType.ToString() + "/" + candidateName + extension;

        if (File.Exists(path))
        {
            byte[] pngBytes = File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);

            Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            target.sprite = fromTex;
            target.gameObject.SetActive(true);
            target.enabled = true;
        }
        else
            target.gameObject.SetActive(false);
    }

    public static bool LoadStreamingImage(Image target, string folder, string fileName)
    {
        string path = Application.streamingAssetsPath + "/" + folder + "/" + fileName + ".jpg";
        if (File.Exists(path))
        {
            byte[] pngBytes = File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);

            Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            target.sprite = fromTex;
            target.gameObject.SetActive(true);
            target.enabled = true;
            return true;
        }
        else
        {
            target.gameObject.SetActive(false);
            Debug.Log($"No image found at {path}");
            return false;
        }
    }

    /// <summary>
    /// Loads a candidate image from the resources 
    /// </summary>
    /// <param name="target">target image object</param>
    /// <param name="imageType">image type whether it is a headshot, cutout, etc</param>
    /// <param name="candidateName">last name of candidate (needs to match the file names)</param>
    public static void LoadResourcesImage(Image target, E.CandidateImageType imageType, string candidateName)
    {
        MonoEntity.Instance.StartCoroutine(LoadResourcesImageAsync(target, imageType, candidateName));   
    }

    private static IEnumerator PlayVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
    }

    private static IEnumerator LoadResourcesImageAsync(Image target, E.CandidateImageType imageType, string candidateName)
    {
        if (target != null)
        {
            string path = "Icons/" + imageType.ToString() + "/" + candidateName;

            ResourceRequest resourceRequest = Resources.LoadAsync<Sprite>(path);
            while (!resourceRequest.isDone)
            {
                yield return null;
            }

            if (target != null)
            {
                if (resourceRequest.asset != null && resourceRequest.asset is Sprite)
                {
                    target.sprite = resourceRequest.asset as Sprite;
                    target.gameObject.SetActive(true);
                    target.enabled = true;
                }
                else
                    target.gameObject.SetActive(false);
            }
        }
    }
}
