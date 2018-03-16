using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KSP.UI.Screens;
using KSP.IO;
using KSP.UI;

namespace Infinity
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class DebugTest : MonoBehaviour
    {
        private ApplicationLauncherButton appLauncherButton;

        private DebugTest instance = null;

        private PluginConfiguration config;

        private int windowId;
        private Rect windowRect;
        private bool windowVisible = false;
        private Vector2 windowScrollPosition;

        private Vector2 testWindowScrollPosition;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        private void OnAppLauncherReady()
        {
            if (appLauncherButton == null)
            {
                appLauncherButton = ApplicationLauncher.Instance.AddModApplication(
                    WindowOn,
                    WindowOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.ALWAYS,
                    GameDatabase.Instance.GetTexture("Infinity/Plugin/Textures/Off", false)
                );
            }
        }

        public void Start()
        {
            
            DontDestroyOnLoad(this);
            Debug.Log("INFINITY TEST");

            config = PluginConfiguration.CreateForType<DebugTest>();
            config.load();

            windowRect = config.GetValue<Rect>("mainWindowRect", new Rect((Screen.width - 400) / 2, Screen.height / 4, 400, 0));


            windowId = GUIUtility.GetControlID(FocusType.Passive);

            windowScrollPosition.Set(0, 0);

            GameEvents.onGUIApplicationLauncherReady.Add(OnAppLauncherReady);
            GameEvents.onGameSceneSwitchRequested.Add(OnSwitchRequested);
        }

        private void RenderWindow(int windowId)
        {
            GUILayout.BeginVertical();

            GUILayout.Box("This is box");
            GUILayout.Label("This is label");
            GUILayout.TextField("This is textField");
            GUILayout.TextArea("This is textArea\nWith Some text inside\nBlah blah blah");
            GUILayout.Button("This is button");
            GUILayout.Toggle(true, "This is toggle");
            GUILayout.BeginScrollView(Vector2.zero, GUILayout.Height(100));
            GUILayout.Label("This is scrollView");
            GUILayout.Label("With a set of labels");
            GUILayout.Label("To fill it");
            GUILayout.Label("With some text");
            GUILayout.Button("And also a button");
            GUILayout.Toggle(true, "And a toggle");
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void OnSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> ev)
        {

            if (appLauncherButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
                appLauncherButton = null;
            }
            windowVisible = false;
        }

        private void WindowOn()
        {
            Debug.Log("WINDOW ON");
            windowVisible = true;
            appLauncherButton.SetTexture(GameDatabase.Instance.GetTexture("Infinity/Plugin/Textures/On", false));
        }

        private void WindowOff()
        {
            Debug.Log("WINDOW OFF");
            windowVisible = false;
            appLauncherButton.SetTexture(GameDatabase.Instance.GetTexture("Infinity/Plugin/Textures/Off", false));
        }

        public void OnGUI()
        {
            if(windowVisible)
            {
                Debug.Log("THERE ARE ON GUI");
                RenderWindow(71425361);
            }
        }

        public void OnDestroy()
        {
            if (instance == this)
                instance = null;

            GameEvents.onGUIApplicationLauncherReady.Remove(OnAppLauncherReady);
            GameEvents.onGameSceneSwitchRequested.Remove(OnSwitchRequested);
        }
    }
}
