using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace StealthGame
{
    public class MainMenu : MonoBehaviour
    {
        private UIDocument m_UIDocument;

        private Button m_StartButton;
        private Button m_ExitButton;
    
        private void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            m_StartButton = m_UIDocument.rootVisualElement.Q<Button>("StartButton");
            m_ExitButton = m_UIDocument.rootVisualElement.Q<Button>("ExitButton");

            m_StartButton.clicked += () =>
            {
                //The game scene needs to be set in the Build Profile Scene List
                SceneManager.LoadScene(1);
            };

            m_ExitButton.clicked += () =>
            {
                Application.Quit();
            };
        }
    }
}