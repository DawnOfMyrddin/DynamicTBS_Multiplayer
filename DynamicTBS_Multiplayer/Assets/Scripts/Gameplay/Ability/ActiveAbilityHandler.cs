using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilityHandler : MonoBehaviour
{
    [SerializeField] private Button activeAbilityButton;
    private Character currentCharacter;

    private void Awake()
    {
        SubscribeEvents();
        ChangeButtonVisibility(null);
    }

    public void ExecuteActiveAbility()
    {
        GameplayEvents.ExecuteActiveAbilityStarted();
        currentCharacter.GetActiveAbility().Execute();
        currentCharacter.SetActiveAbilityOnCooldown();
        // Please remember to call this after every execution (in AAHandler classes): GameplayEvents.ActionFinished(UIActionType.ActiveAbility);
    }

    private void ChangeButtonVisibility(Character character)
    {
        currentCharacter = character;

        bool active = character != null;
        bool disabled = active && character.IsActiveAbilityOnCooldown();

        activeAbilityButton.gameObject.SetActive(active);
        activeAbilityButton.interactable = !disabled;
    }

    #region EventsRegion

    private void SubscribeEvents()
    {
        GameplayEvents.OnCharacterSelectionChange += ChangeButtonVisibility;
    }

    private void UnsubscribeEvents()
    {
        GameplayEvents.OnCharacterSelectionChange -= ChangeButtonVisibility;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}