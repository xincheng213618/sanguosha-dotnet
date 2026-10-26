using System.Windows.Input;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public ICommand SelectAllZhihengHandCommand { get; private set; } = null!;

    private bool ShowSelectAllZhihengHand => IsActiveSkillCardSelectionPending &&
        HumanActiveSkillAction?.ProgramSkillId is "classic:zhiheng" or "boundary:zhiheng";

    private int[] SelectableZhihengHandIds => ShowSelectAllZhihengHand && HumanActiveSkillAction is { } action
        ? _snapshot.Players.Single(player => player.Seat == _snapshot.HumanSeat).Hand
            .Where(card => action.SelectableCardIds.Contains(card.Id)).Select(card => card.Id).ToArray()
        : [];

    private bool CanSelectAllZhihengHand
    {
        get
        {
            var handIds = SelectableZhihengHandIds;
            return HumanActiveSkillAction is { } action && handIds.Any(id => !_selectedActiveSkillCardIds.Contains(id)) &&
                _selectedActiveSkillCardIds.Union(handIds).Count() <= action.MaxCardCount;
        }
    }

    private void SelectAllZhihengHand()
    {
        if (!CanSelectAllZhihengHand || HumanActiveSkillAction is not { } action) return;
        _selectedActiveSkillCardIds.UnionWith(SelectableZhihengHandIds);
        RefreshActiveSkillCardSelection(action);
    }
}
