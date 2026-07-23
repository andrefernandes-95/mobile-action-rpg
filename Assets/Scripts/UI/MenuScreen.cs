using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class MenuScreen : MonoBehaviour
    {
        [SerializeField] protected UIDocument document;

        public abstract MenuId Id { get; }

        public bool IsOpen
        {
            get;
            private set;
        }

        protected VisualElement Root
        {
            get;
            private set;
        }

        protected virtual void Awake()
        {
            if (document == null)
            {
                document = GetComponent<UIDocument>();
            }
        }

        protected virtual void OnEnable()
        {
            Root = document.rootVisualElement;

            var closeButton = Root.Q<Button>("CloseButton");
            if (closeButton != null)
            {
                closeButton.clicked += () => MenuManager.Instance?.Close();
            }

            Bind();
            Root.style.display = DisplayStyle.None;
            MenuManager.Instance?.Register(this);
        }

        protected virtual void OnDisable()
        {
            MenuManager.Instance?.Unregister(this);
        }

        // Called only by MenuManager. internal is for classes inside the same assembly
        internal void Enter()
        {
            Root.style.display = DisplayStyle.Flex;
            IsOpen = true;
            OnOpen();
        }

        internal void Exit()
        {
            OnClose();
            Root.style.display = DisplayStyle.None;
            IsOpen = false;
        }

        protected abstract void Bind();
        protected virtual void OnOpen()
        {

        }

        protected virtual void OnClose()
        {

        }
    }
}