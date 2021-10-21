using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    private void Start()
    {
        text = titleText.GetComponent<Text>();
        StartCoroutine(TextMover());
        StartCoroutine(BackgroundImgMover());
        //StartCoroutine(skyAlphaChanger());
        
        InitUI();
    }

    #region Move when Awake
    private IEnumerator TextMover()
    {
        while(text.rectTransform.anchoredPosition != textTargetPos)
        {
            text.rectTransform.anchoredPosition 
                = Vector2.MoveTowards(
                    text.rectTransform.anchoredPosition,
                    textTargetPos,
                    Time.deltaTime * 200f);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator BackgroundImgMover()
    {
        while(sky.transform.localScale != backgroundTargetScale)
        {
            sky.transform.localScale = Vector3.MoveTowards(sky.transform.localScale, backgroundTargetScale, Time.deltaTime);
            moon.transform.localScale = Vector3.MoveTowards(moon.transform.localScale, new Vector3(2, 2, 1), 0.4f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator skyAlphaChanger()
    {
        SpriteRenderer skySprite = sky.GetComponent<SpriteRenderer>();
        skySprite.color = new Color(1, 0, 0, 0);
        while (skySprite.color.g != 1)
        {
            skySprite.color = new Color(skySprite.color.r, skySprite.color.g + 0.1f * Time.deltaTime, skySprite.color.b + 0.1f * Time.deltaTime, skySprite.color.a + 0.2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region BtnManage
    public void StartBtn()
    {
        networkPanel.SetActive(true);
        netWorkManager.Connect();
        netWorkManager.JoinLobby();
    }
    public void OptionBtn()
    {
        ResolutionPanel.SetActive(true);
    }
    public void ExitBtn()
    {
        
    }
    #endregion

    #region ResolutionPanel
    private void InitUI()
    {
        resolutionDropdown.ClearOptions();
        for(int i=0; i<Screen.resolutions.Length; ++i)
        {
            if(Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        int optionNum = 0;
        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            ++optionNum;
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }
    public void OptionAcceptBtn()
    {
        ResolutionPanel.SetActive(false);
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    #endregion

    [SerializeField] private GameObject sky = null;
    [SerializeField] private GameObject titleText = null;
    [SerializeField] private Image moon = null;
    [SerializeField] private GameObject networkPanel = null;
    [SerializeField] private GameObject ResolutionPanel = null;
    [SerializeField] private Toggle fullscreenBtn = null;
    [SerializeField] private NetWorkManager netWorkManager = null;
    List<Resolution> resolutions = new List<Resolution>();
    public Dropdown resolutionDropdown;
    private int resolutionNum = 0;
    FullScreenMode screenMode = FullScreenMode.ExclusiveFullScreen;
    private Text text = null;
    Vector2 textTargetPos = new Vector2(0f, -180f);
    Vector3 backgroundTargetScale = new Vector3(3, 3, 1);
}
