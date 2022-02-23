using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMuteConroller : MonoBehaviour
{
    [SerializeField] private AudioController audioController = null;

    [SerializeField] private GameObject mutedImage = null;
    [SerializeField] private GameObject unmutedImage = null;

	private void Start()
	{
        UpdateImage(audioController.GetSettingFromPlayerPrefs("Audio_Muted"));
    }

	public void UpdateImage()
	{
        if(audioController.isMuted)
		{
            mutedImage.SetActive(true);
            unmutedImage.SetActive(false);
        }
        else
		{
            unmutedImage.SetActive(true);
            mutedImage.SetActive(false);
        }
	}

    public void UpdateImage(bool isMuted)
    {
        if (isMuted)
        {
            mutedImage.SetActive(true);
            unmutedImage.SetActive(false);
        }
        else
        {
            unmutedImage.SetActive(true);
            mutedImage.SetActive(false);
        }
    }
}
