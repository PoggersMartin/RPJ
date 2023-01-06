using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;
    public GameObject shopMenu;
    public GameObject notification;
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public int money;
    public TextMeshProUGUI moneyTXT;
    public TextMeshProUGUI healthTXT;
    public TextMeshProUGUI ammoTXT;
    public TextMeshProUGUI NotificationTXT;
    public TextMeshProUGUI teacher;

    public GameObject aimCursor;



    public float timer, refresh, avgFramerate;
    public string display = "{0} FPS";
    public TextMeshProUGUI m_Text;

    public static bool isPaused;
    public static bool shopIsOpened;
    public static bool cantPause;
    public static bool cantOpenShop;
    public static bool fps = true;
    public static bool isDead;

    public GameObject notificationTeacher;


    Player player1;
    Shoot shoot1;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        shoot1 = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Shoot>();
       
        UpdateMoneyTXT();
        UpdateHealthTXT();
        UpdateammoTXT();
        isDead = false;
        Time.timeScale = 1;
        FriendlyNPC.counter = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        UpdateHealthTXT();
        RescueTeachears();
        if (!cantPause) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (isPaused)
                {

                    PauseMenuResume();


                }
                else
                {
                    PauseMenu();
                }
            }


        }
        if (!isPaused && !shopIsOpened) {
            if (fps) {
                StartCoroutine(WaitAndShowFps());

            }

        }
        if (player1.health == 0)
        {


            player1.anim.SetTrigger("dead");
            isDead = true;
            shoot1.DestroyWeapon();
            StartCoroutine(Wait());
        }
        

    }
    IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.6f);
            DeathMenu();
        



    }

    public void RefreshRate()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_Text.text = string.Format(display, avgFramerate.ToString());
    }

    IEnumerator WaitAndShowFps()
    {
        yield return new WaitForSeconds(1);
        RefreshRate();
    }


    public void Shop()
    {
        Time.timeScale = 0;
        shopMenu.SetActive(true);
        notification.SetActive(false);
        shopIsOpened = true;
        cantPause = true;
        fps = false;
        Cursor.visible = true;
        aimCursor.SetActive(false);

    }

    public void Resume()
    {
        Time.timeScale = 1;
        shopMenu.SetActive(false);
        notification.SetActive(true);
        shopIsOpened = false;
        cantPause = false;
        fps = true;
        Cursor.visible = false;
        aimCursor.SetActive(true);
    }
    public void UpdateHealthTXT() {
        healthTXT.text = player1.health.ToString();

    }
    public void UpdateMoneyTXT()
    {
        moneyTXT.text = money.ToString() + "$";
    }
    public void UpdateammoTXT()
    {
        ammoTXT.text = shoot1.magSize.ToString();

    }
    public void NotifícationVisible()
    {
        notification.SetActive(true);
    }
    public void NotifícationNonVisible()
    {
        notification.SetActive(false);
    }

    public void PauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
        cantOpenShop = true;
        fps = false;
        Cursor.visible = true;
        aimCursor.SetActive(false);


    }
    public void PauseMenuResume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
        cantOpenShop = false;
        fps = true;
        Cursor.visible = false;
        aimCursor.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        pauseMenu.SetActive(false); Time.timeScale = 1;
        isPaused = false;
    }
    public void DeathMenu()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        aimCursor.SetActive(false);
    }

    public void RestartGameLevel1()
    {
        SceneManager.LoadScene("GameScene");
        deathMenu.SetActive(false);
    }

    public void RescueTeachears()
    {
        teacher.text = "Rescue teachers: "+FriendlyNPC.counter.ToString()+"/1";
    }
    
    public void NotificationTeacherOn()
    {
        notificationTeacher.SetActive(true);
    }
    public void NotificationTeacherOff()
    {
        notificationTeacher.SetActive(false);
    }







}
