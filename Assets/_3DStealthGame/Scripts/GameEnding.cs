using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayImageDuration = 1f;
    [SerializeField] private GameObject player;
    [SerializeField] private UIDocument uIDocument;


    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;

    private VisualElement m_EndScreen;
    private VisualElement m_CaughtScreen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_EndScreen = uIDocument.rootVisualElement.Q<VisualElement>("EndScreen");
        m_CaughtScreen = uIDocument.rootVisualElement.Q<VisualElement>("CaughtScreen");        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(m_EndScreen,false);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(m_CaughtScreen,true);
        }
        
    }

    void EndLevel(VisualElement element , bool doRestart)
    {
        m_Timer  += Time.deltaTime;
        element.style.opacity = m_Timer/ fadeDuration;

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                Application.Quit();
                Time.timeScale = 0;
            }
        }

    }
}
