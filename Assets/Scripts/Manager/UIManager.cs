using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        #region List.Add

        btnList.Add(unitBtn);
        btnList.Add(buildBtn);
        btnList.Add(itemBtn);
        btnList.Add(optionBtn);
        btnList.Add(mailBtn);

        panelList.Add(unitBtnPanel);
        panelList.Add(buildBtnPanel);
        panelList.Add(itemBtnPanel);
        panelList.Add(optionBtnPanel);
        panelList.Add(mailBtnPanel);

        #endregion

        MakeCursor();
        WaveTextUpdate();
        StartCoroutine(GoodsTextUpdate());
    }
    private void Update()
    {
        MoveCursor();
    }
    private void MakeCursor()
    {
        cursor = new GameObject("Cursor");
        cursorSprite = Resources.Load<Sprite>("Sprites/cursor/cursor(1)");
        
        cursor.AddComponent<CanvasRenderer>();
        cursor.AddComponent<Image>();
        cursor.GetComponent<Image>().sprite = cursorSprite;
        cursor.GetComponent<Image>().raycastTarget = false;
        cursor.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
        cursor.transform.SetParent(canvas.transform);
    }

    #region Button OnClick Methods

    //btnList[idx], panelList[idx] match
    private void ButtonClick(Button _btnType)
    {
        int panelIdx = btnList.IndexOf(_btnType);
        GameObject panel = panelList[panelIdx];

        if(panel.activeSelf==false)
        {
            for(int i=0; i<panelList.Count; ++i)
            {
                GameObject otherPanel = panelList[i];
                otherPanel.SetActive(false);
            }
        }
        panel.SetActive(!panel.activeSelf);
    }

    public void unitBtnClick()
    {
        ButtonClick(unitBtn);
    }
    public void bulidBtnClick()
    {
        ButtonClick(buildBtn);
    }
    public void itemBtnClick()
    {
        ButtonClick(itemBtn);
    }
    public void optionBtnClick()
    {
        ButtonClick(optionBtn);
    }
    public void mailBtnClick()
    {
        ButtonClick(mailBtn);
    }
    public void ExitBtnClick()
    {
        PhotonNetwork.Disconnect();
    }
    public void SetMonsterPanelExitBtn()
    {
        exitBtnPanel.SetActive(false);
    }

    public void DungeonMaintenanceBtn()
    {
        isMaintenance = false;
        maintenanceBtn.gameObject.SetActive(false);
    }

    #endregion

    #region Texts

    private IEnumerator GoodsTextUpdate()
    {
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            boneText.text = gameManager.goods.bone.ToString();
            tearText.text = gameManager.goods.tear.ToString();
            soulgemText.text = gameManager.goods.soulgem.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
    public void WaveTextUpdate()
    {
        waveText.text = GameManager.wave + " wave(" + spawnManager.curHeroCount + " / " + spawnManager.maxHeroCount + ")";
    }

    #endregion

    #region Cursor

    private void MoveCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray))
        {
            Debug.DrawRay(ray.origin, ray.direction,Color.red);
            cursor.GetComponent<Image>().rectTransform.position = Input.mousePosition;
        }
    }

    #endregion


    public bool isMaintenance = false;

    private List<Button> btnList = new List<Button>();
    private List<GameObject> panelList = new List<GameObject>();

    private Sprite cursorSprite = null;
    private Vector3 mouseWorldPos = Vector3.zero;
    private GameObject cursor = null;
    
    [SerializeField] private Text waveText = null;
    [SerializeField] private Canvas canvas = null;

    [Header("Manager")]
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private SpawnManager spawnManager = null;

    [Header("Button")]
    public Button maintenanceBtn = null;
    [SerializeField] private Button unitBtn = null;
    [SerializeField] private Button buildBtn = null;
    [SerializeField] private Button itemBtn = null;
    [SerializeField] private Button optionBtn = null;
    [SerializeField] private Button mailBtn = null;

    [Header("ButtonPanel")]
    [SerializeField] private GameObject unitBtnPanel = null;
    [SerializeField] private GameObject buildBtnPanel = null;
    [SerializeField] private GameObject itemBtnPanel = null;
    [SerializeField] private GameObject optionBtnPanel = null;
    [SerializeField] private GameObject mailBtnPanel = null;
    [SerializeField] private GameObject exitBtnPanel = null;

    [Header("Goods")]
    [SerializeField] private Text boneText = null;
    [SerializeField] private Text tearText = null;
    [SerializeField] private Text soulgemText = null;
}
