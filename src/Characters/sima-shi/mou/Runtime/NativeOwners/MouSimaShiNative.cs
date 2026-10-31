#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouSimaShiNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries> getCharacterSimaShiMouRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IMouSimaShiNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public (int Id, ulong Before, ulong After) Random(IReadOnlyList<MouSimaShiDeckCard> cards)
    {
        var before = getDeterministicRandom().State;
        var selected = cards[getDeterministicRandom().Next(cards.Count)];
        return (selected.Id, before, getDeterministicRandom().State);
    }

    public void Move(ProgramSkillFrame f, MouSimaShiCard card, CardLocation destination)
    {
        if (!getCharacterSimaShiMouRuntime().MouSimaShiRules.Valid(f) || f.MouSimaShi is not { Stage: MouSimaShiStage.MovementChildren } r || r.Index < 1 || r.Cards[r.Index - 1] != card || getCardZoneStore().GetLocation(card.Id) != card.From)
            throw new InvalidOperationException("Mou Sima Shi needs his exact current physical movement.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([card.Id], destination, new("skill-program.mou:sima-shi." + r.Operation), (_, moves) => getCharacterSimaShiMouRuntime().MouSimaShiRules.MovesPaid(f.Id, moves), disclosure: card.From.Zone is CardZoneKind.DrawPile or CardZoneKind.Hand || destination == CardLocation.DrawPile || destination == CardLocation.OutsideGame ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
    }

    public void Damage(ProgramSkillFrame f)
    {
        if (!getCharacterSimaShiMouRuntime().MouSimaShiRules.Valid(f) || f.MouSimaShi is not { Stage: MouSimaShiStage.Damage, Operation: SkillProgramEffectOp.ResolveMouSimaShiZhenrao } r || !matchParticipantsState.ReferenceToPlayers[r.Target].IsAlive)
            throw new InvalidOperationException("Zhenrao damage requires the accepted actual-use target.");
        getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, r.Target, 1, explicitSourceSeat: f.OwnerSeat);
    }

    public void Shuffle(ProgramSkillFrame f)
    {
        if (!getCharacterSimaShiMouRuntime().MouSimaShiRules.Valid(f) || f.MouSimaShi is not { Stage: MouSimaShiStage.Shuffle, Operation: SkillProgramEffectOp.ReturnMouSimaShiChenlve } || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouSimaShiShuffledEvent>().Any(s => s.FrameId == f.Id))
            throw new InvalidOperationException("Chenlve shuffles only after its actual death return movements.");
        var before = getDeterministicRandom().State;
        getCardZoneStore().Shuffle(CardLocation.DrawPile, getDeterministicRandom());
        advanceEventRulesAndQueueFact(new MouSimaShiShuffledEvent(f.Id, getCardZoneStore().Count(CardLocation.DrawPile), before, getDeterministicRandom().State));
    }
}
