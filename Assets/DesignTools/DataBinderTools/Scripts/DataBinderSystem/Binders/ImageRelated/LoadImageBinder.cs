using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

[System.Serializable]
public class LoadImageBinder : GenericBinder<Image>
{
    [SerializeField]
    private E.ImageSource m_source;

    [SerializeField]
    private E.CandidateImageType m_imageType;

    public E.ImageSource Source { get { return m_source; } }

    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            switch (m_source)
            {
                case E.ImageSource.Url:
                    MonoEntity.Instance.StartCoroutine(LoadImageUrl(data[Key]));
                    return true;
                case E.ImageSource.Resources:
                    foreach (Image target in m_targets)
                    {
                        FileLoader.LoadResourcesImage(target, m_imageType, data[Key]);
                    }
                    return true;
                case E.ImageSource.StreamingAssets:
                    foreach (Image target in m_targets)
                    {
                        FileLoader.LoadStreamingImage(target, m_imageType, data[Key]);
                    }
                    return true;
                default:
                    return false;
            }           
        }
        else
            return false;
    }

    public override void ClearData()
    {
        foreach (Image target in m_targets)
        {
            target.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadImageUrl(string url)
    {
        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return imageRequest.SendWebRequest();

            if(imageRequest.isNetworkError || imageRequest.isHttpError)
            {
                Debug.LogError($"Request to url failed: {url}");
            }
            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(imageRequest);

                foreach (Image target in m_targets)
                {
                    target.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.zero);
                }
            }
        }
    }
}
