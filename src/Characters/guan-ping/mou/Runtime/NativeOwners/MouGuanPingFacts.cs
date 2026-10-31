#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuanPingFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::CardGame.Core.EngineStates.CoreGameEngineSkillProgramsState gameEngineSkillProgramsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries> getCoreGameEngineCardConversionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries> getCoreNativeTargetLegalityRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanHaiBridgesEntries> getCoreGameEngineGuanHaiBridgesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore) : IMouGuanPingFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToCurrentSeat;
    public int PhaseId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public SkillProgram Program() => getContentRegistry().GetSkill(MouGuanPingComposition.Skill).Program!;
    public int PhaseUsage(int owner) => gameEngineSkillProgramsState.ReferenceToProgramPhaseUses.GetValueOrDefault((owner, MouGuanPingComposition.Skill, MouGuanPingComposition.Activation));
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool WholeColor(int owner, IReadOnlyList<int> ids)
    {
        if (ids.Count == 0 || ids.Distinct().Count() != ids.Count || !Alive(owner))
            return false;
        var hand = getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[owner]);
        var cards = hand.Where(c => ids.Contains(c.Id)).ToArray();
        if (cards.Length != ids.Count || global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[owner], cards[0]))is not { } red)
            return false;
        return hand.Where(c => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[owner], c)) == red).Select(c => c.Id).Order().SequenceEqual(ids.Order());
    }

    public IReadOnlyList<ProgramMultiCardViewAsSelection> Groups(int owner)
    {
        if (!Alive(owner) || !getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(matchParticipantsState.ReferenceToPlayers[owner], MouGuanPingComposition.Skill))
            return[];
        var actor = matchParticipantsState.ReferenceToPlayers[owner];
        var source = new CardConversionSource(MouGuanPingComposition.Skill, MouGuanPingComposition.Binding, owner, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(actor, MouGuanPingComposition.Skill));
        return Array.AsReadOnly(new[] { true, false }.Select(red => getCorePhysicalOwnedCardQueriesRuntime().GetHand(actor).Where(c => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(getCoreNativeCardAppearanceRuntime().EffectiveSuit(actor, c)) == red).Select(c => c.Id).Order().ToArray()).Where(ids => ids.Length > 0).Select(ids => getCoreGameEngineCardConversionsRuntime().FindProgramMultiCardViewAsSelection(actor, ids, CardKind.Slash, false, source)).Where(s => s is not null).Select(s => ProgramSelectionProjection.Freeze(s)!).ToArray());
    }

    public IReadOnlyList<int> Targets(int owner, IReadOnlyList<Card> cards)
    {
        var actor = matchParticipantsState.ReferenceToPlayers[owner];
        var selection = new ProgramMultiCardViewAsSelection(cards, new(MouGuanPingComposition.Skill, MouGuanPingComposition.Binding, owner, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(actor, MouGuanPingComposition.Skill)), CardKind.Slash);
        return Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(t => getCoreNativeTargetLegalityRuntime().CanUseVirtualSlashTarget(actor, t, physicalSuit: getCoreGameEngineGuanHaiBridgesRuntime().ProgramMultiCardSuit(actor, selection), effectiveColor: getCoreGameEngineGuanHaiBridgesRuntime().ProgramMultiCardColor(actor, selection), physicalCardIds: cards.Select(c => c.Id).ToArray(), ignoreDistance: true, conversionSource: selection.Source, countsTowardLimit: false)).Select(t => t.Seat).ToArray());
    }

    public CardUseFrame? Use(long id) => getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(id);
    public CardUseFrame? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouGuanPingComposition.Skill || c.BindingId != MouGuanPingComposition.Trigger || w.Window != SkillProgramTriggerWindow.CardUseCommitted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != info.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        return getFrameStore().OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == info.CardActionId && u.MouGuanPing is { } r && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillInstanceId == c.SkillInstanceId && r.GameplayHash == c.GameplayHash);
    }
}
