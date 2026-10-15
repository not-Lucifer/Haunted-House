using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace StealthGame
{
    public class GameEnding : MonoBehaviour
    {
        public float fadeDuration = 1f;
        public float displayImageDuration = 1f;
        public GameObject player;
        public UIDocument uiDocument;
        public AudioSource exitAudio;
        public AudioSource caughtAudio;

        bool m_IsPlayerAtExit;
        bool m_IsPlayerCaught;
        float m_Timer;
        bool m_HasAudioPlayed;

        private VisualElement m_EndScreen;
        private VisualElement m_CaughtScreen;
    
        //DEMO ADDITION
        private float m_Demo_GameTimer;
        private bool m_Demo_GameTimerIsTicking;
        private Label m_Demo_GameTimerLabel;

        void Start()
        {
            m_EndScreen = uiDocument.rootVisualElement.Q<VisualElement>("EndScreen");
            m_CaughtScreen = uiDocument.rootVisualElement.Q<VisualElement>("CaughtScreen");

            m_Demo_GameTimerLabel = uiDocument.rootVisualElement.Q<Label>("Demo_TimerLabel");
            m_Demo_GameTimer = 0.0f;
            m_Demo_GameTimerIsTicking = true;
            Demo_UpdateTimerLabel();
        }
    
        void OnTriggerEnter (Collider other)
        {
            if (other.gameObject == player)
            {
                m_IsPlayerAtExit = true;
            }
        }

        public void CaughtPlayer ()
        {
            m_IsPlayerCaught = true;
        }

        void Update ()
        {
            if (m_Demo_GameTimerIsTicking)
            {
                m_Demo_GameTimer += Time.deltaTime;
                Demo_UpdateTimerLabel();
            }
        
            if (m_IsPlayerAtExit)
            {
                EndLevel (m_EndScreen, true, exitAudio);
            }
            else if (m_IsPlayerCaught)
            {
                EndLevel (m_CaughtScreen, true, caughtAudio);
            }
        }

        void EndLevel (VisualElement element, bool doRestart, AudioSource audioSource)
        {
            m_Demo_GameTimerIsTicking = false;
        
            if (!m_HasAudioPlayed)
            {
                audioSource.Play();
                m_HasAudioPlayed = true;
            }
            
            m_Timer += Time.deltaTime;
            element.style.opacity = m_Timer / fadeDuration;

            if (m_Timer > fadeDuration + displayImageDuration)
            {
                if (doRestart)
                {
                    SceneManager.LoadScene("DemoScene");
                }
                else
                {
                    Application.Quit();
                    Time.timeScale = 0;
                }
            }
        }

        void Demo_UpdateTimerLabel()
        {
            m_Demo_GameTimerLabel.text = m_Demo_GameTimer.ToString("0.00");
        }
    }
}