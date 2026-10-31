
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_CurrentTurnUseKinds
{
    internal static CurrentTurnCardUseKindsRecordedEvent[] ActualTurnBasicUses(
        IEnumerable<IGameEvent> history, int turn, int turnOwner)
    {
        var events = history.ToArray();
        var start = Array.FindLastIndex(events, e => e is TurnStartedEvent t && t.TurnNumber == turn && t.ActorSeat == turnOwner);
        if (turn <= 0 || start < 0) return [];
        var prefix = events.Skip(start + 1).TakeWhile(e => e is not TurnEndedEvent t || t.TurnNumber != turn || t.ActorSeat != turnOwner).ToArray();
        return prefix.OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(f => f.TurnNumber == turn && f.TurnOwnerSeat == turnOwner &&
            f.CardCategoryMask == 1 << (int)SkillProgramCardCategory.Basic && f.EffectiveCardKind is not null &&
            (f.RoundActionType == CardActionType.Use || f.RoundActionType == CardActionType.Response &&
                f.EffectiveCardKind == CardKind.Dodge && f.RoundProviderSeat == f.ActorSeat)).Select(f =>
            EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(f.EffectiveCardKind!.Value) &&
                prefix.OfType<ProgramCurrentSlashFireChangedEvent>().LastOrDefault(c => c.CardUseFrameId == f.OriginFrameId && c.ActionId == f.CardActionId) is { } changed
                    ? f with { EffectiveCardKind = changed.FinalKind } : f).ToArray();
    }

    internal static int ActualTurnSuitBit(Suit? suit) => suit switch
    {
        Suit.Spade => 1, Suit.Heart => 2, Suit.Club => 4, Suit.Diamond => 8, _ => 0
    };
}
