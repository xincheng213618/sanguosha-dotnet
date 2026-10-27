namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool ShanheAlliedHarvestProhibited(GroupCardHandle group, int pickerSeat) =>
        GroupNullificationCardKind(group) == CardKind.FiveGrains &&
        HasCardPolicy(_players[group.SourceSeat], SkillProgramCardPolicyKind.AlliedHarvestOnly, CardKind.FiveGrains) &&
        !IsShanheFriendlySeat(group.SourceSeat, pickerSeat);

    private int ShanheAlliedPeachGardenRecovery(GroupCardHandle group, int targetSeat, int original) =>
        GroupNullificationCardKind(group) == CardKind.PeachGarden &&
        HasCardPolicy(_players[group.SourceSeat], SkillProgramCardPolicyKind.AlliedPeachGardenDoubleRecovery, CardKind.PeachGarden) &&
        IsShanheFriendlySeat(group.SourceSeat, targetSeat) ? checked(original * 2) : original;

    private bool RefreshShanheAlliedHarvestSelection()
    {
        if (_pendingDecision is not { Kind: DecisionKind.SelectHarvestCard } pending ||
            ActiveGroupCard is not { Effect: GroupCardEffect.PublicDraft } group ||
            _resolutionStack.LastOrDefault()?.Id != group.ResolutionId ||
            group.TargetIndex < 0 || group.TargetIndex >= group.TargetSeats.Count ||
            group.TargetSeats[group.TargetIndex] != pending.PlayerSeat ||
            !ShanheAlliedHarvestProhibited(group, pending.PlayerSeat)) return false;
        ClearPendingDecision();
        _status = EngineStatus.Running;
        BeginHarvestSelection(group);
        return true;
    }
}
