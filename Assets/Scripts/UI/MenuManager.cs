using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public sealed class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance
        {
            get;
            private set;
        }

        [SerializeField] bool pauseWhileOpen = true;

        MenuScreen active;

        readonly Dictionary<MenuId, MenuScreen> screens = new();
        public bool AnyOpen => active != null;
        public MenuId ActiveId => active != null ? active.Id : MenuId.None;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Register(MenuScreen screen) => screens[screen.Id] = screen;
        public void Unregister(MenuScreen screen)
        {
            if (screens.TryGetValue(screen.Id, out var s) && s == screen)
            {
                screens.Remove(screen.Id);
            }
        }

        public void Toggle(MenuId id)
        {
            if (active != null && active.Id == id)
            {
                Close();
            }
            else
            {
                Open(id);
            }
        }

        public void Open(MenuId id)
        {
            if (GameFlow.IsInCutscene)
            {
                return;
            }

            if (!screens.TryGetValue(id, out var next))
            {
                return;
            }

            active?.Exit();
            active = next;
            active.Enter();

            if (pauseWhileOpen)
            {
                Time.timeScale = 0f;
            }
        }

        public void Close()
        {
            if (active == null)
            {
                return;
            }

            active.Exit();
            active = null;

            if (pauseWhileOpen)
            {
                Time.timeScale = 1f;
            }
        }
    }
}