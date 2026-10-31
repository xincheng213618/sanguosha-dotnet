#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuLieNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterHuLieMouEntries> getCharacterHuLieMouRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, MouHuLieFactsFactory createMouHuLieFacts, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime) : IMouHuLieNative
{
    public void Publish() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Use(ProgramSkillFrame f) => getCharacterHuLieMouRuntime().IssueMouHuLieSlash(f);
    public void Give(ProgramSkillFrame f)
    {
        if (!getCharacterHuLieMouRuntime().MouHuLieRules.Valid(f) || f.MouHuLie is not { Stage: MouHuLieStage.GiveChildren } r || !matchParticipantsState.ReferenceToPlayers[r.Target].IsAlive)
            throw new InvalidOperationException("Chuanyu/Yitou needs its accepted real recipient.");
        var initial = r.Operation == SkillProgramEffectOp.StartMouHuLieChuanyu;
        if (initial && r.Target == f.OwnerSeat)
        {
            if (r.Cards is not [var card] || !createMouHuLieFacts().Owned(f.OwnerSeat, false).Contains(card))
                throw new InvalidOperationException("Self Chuanyu marks only its exact unchanged own HE entity.");
            getCharacterHuLieMouRuntime().MouHuLieRules.Given(f.Id, [], true);
            return;
        }

        if (r.Cards.Count == 0 || r.Cards.Any(c => getCardZoneStore().GetLocation(c.Id) != c.From) || (initial || r.Operation == SkillProgramEffectOp.ResolveMouHuLieYitou) && r.Cards.Any(c => !createMouHuLieFacts().Owned(f.OwnerSeat, r.Operation == SkillProgramEffectOp.ResolveMouHuLieYitou).Contains(c)) || r.Operation == SkillProgramEffectOp.RegiveMouHuLieChuanyu && (r.Yu is null || r.Cards is not [var used] || used.Id != r.Yu.CardId || used.From != CardLocation.DiscardPile))
            throw new InvalidOperationException("Chuanyu/Yitou lost its exact current physical Give material.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(r.Cards.Select(c => c.Id).ToArray(), CardLocation.Hand(r.Target), new("skill-program.mou:chuanyu-or-yitou.give"), (_, moves) =>
        {
            getCharacterHuLieMouRuntime().MouHuLieRules.Given(f.Id, moves);
            if (r.Cards.All(c => c.From.OwnerSeat == f.OwnerSeat) && r.Target != f.OwnerSeat)
                getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, r.Cards.Select(c => c.Id).ToArray(), r.Before);
        }, disclosure: CardMovementDisclosure.FaceUp);
    }

    public void Clear(ProgramSkillFrame f, MouHuLieCard card)
    {
        if (!getCharacterHuLieMouRuntime().MouHuLieRules.Valid(f) || f.MouHuLie is not { Stage: MouHuLieStage.ClearChildren } r || r.Yu?.CardId != card.Id || r.Index < 1 || r.Cards[r.Index - 1] != card || getCardZoneStore().GetLocation(card.Id) != card.From || card.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.DrawPile))
            throw new InvalidOperationException("Chuanyu may discard only its own marked actual reachable entity.");
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves) => getCharacterHuLieMouRuntime().MouHuLieRules.ClearedMoves(f.Id, moves);
        if (card.From.OwnerSeat is { } holder)
            getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, holder, [card.Id], new("skill-program.mou:chuanyu.round-discard"), Paid);
        else
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([card.Id], CardLocation.DiscardPile, new("skill-program.mou:chuanyu.round-deck-discard"), Paid, disclosure: CardMovementDisclosure.FaceUp);
    }
}
