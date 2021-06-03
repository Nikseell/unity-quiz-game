using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Runtime.InteropServices;


public class Link : MonoBehaviour
{
	public void InfoLink()
	{
		#if !UNITY_EDITOR
		OpenWindow(GameManager.infoLink);
		#endif
	}

	public void DataBaseLink()
	{
		#if !UNITY_EDITOR
		OpenWindow("http://www.balvurcb.lv/kb/");
		#endif
	}

	[DllImport("__Internal")]
	private static extern void OpenWindow(string url);

}