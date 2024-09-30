using UnityEngine;
using UnityEngine.InputSystem;
using CupkekGames.Core;

namespace CupkekGames.HeroManager
{
    public class GameInputManager : MonoBehaviour
    {
        // References
        [SerializeField] private PlayerInput _playerInput;
        public PlayerInput PlayerInput => _playerInput;
        [SerializeField] private CoreEventDatabase _coreEventDatabase;
        [SerializeField] private string _actionEscape = "Escape";

        // Special Actions
        private InputAction _escapeAction;

        private void Awake()
        {
            _escapeAction = _playerInput.actions[_actionEscape];
        }

        private void OnEnable()
        {
            _escapeAction.performed += OnEscape;
        }

        private void OnDisable()
        {
            _escapeAction.performed -= OnEscape;
        }

        private void OnEscape(InputAction.CallbackContext context)
        {
            _coreEventDatabase.InputEscapeEvent?.Invoke();
        }

        public InputAction GetAction(string key)
        {
            return _playerInput.actions[key];
        }
    }
}
