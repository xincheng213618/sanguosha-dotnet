#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouSmNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, MouSmFactsFactory createMouSmFacts, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries> getCoreGameEngineDamagePreventionProgramsRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<TurnCardUseEffectStore> getTurnCardUseEffectStore, EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IMouSimaYiNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Give(ProgramSkillFrame f)
    {
        if (!getCharacterSimaYiMouRuntime().MouSimaYi.Valid(f) || f.MouSimaYi is not { Stage: MouSmStage.GiveChildren, Materials: [var c] } r || !createMouSmFacts().Gifts(f, r.Target).Contains(c))
            throw new InvalidOperationException("Quanmou requires the giver's current real entity.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([c.Id], CardLocation.Hand(f.OwnerSeat), new(MouSimaYiRuntime.GiveReason), (_, moves) =>
        {
            if (moves is not [var m] || m.CardId != c.Id || m.CardKind != c.Kind || m.From != c.From || m.Reason.Value != MouSimaYiRuntime.GiveReason || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && !getCharacterSimaYiMouRuntime().IsMouSmShopEquipmentGivePayment(f, r, c, m) || getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<MouSmGivePaidEvent>().Any(p => p.FrameId == f.Id))
                throw new InvalidOperationException("Quanmou lost the exact native Give movement.");
            var own = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, own with { MouSimaYi = own.MouSimaYi!with { After = m.Sequence } });
            advanceEventRulesAndQueueFact(new MouSmGivePaidEvent(f.Id, r.Target, c.Id, m.Sequence));
            getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, r.Target, f.OwnerSeat, [c.Id], r.Before);
        });
    }

    public void Prevent(ProgramSkillFrame f) => getCoreGameEngineDamagePreventionProgramsRuntime().PreventProgramCurrentDamage(f);
    public void Damage(ProgramSkillFrame f, int target) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, target, 1);
    public void Disclose(ProgramSkillFrame f) => getCharacterSimaYiMouRuntime().DiscloseMouSmTargets(f);
    public void ProhibitHand(ProgramSkillFrame f, int target)
    {
        if (!getCharacterSimaYiMouRuntime().MouSimaYi.Valid(f) || f.MouSimaYi is not { Operation: SkillProgramEffectOp.ResolveMouSimaYiPingliao } r || !r.Targets.Contains(target) || r.PlayedSeats.Contains(target) || target == f.OwnerSeat || !getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(target) || !matchParticipantsState.ReferenceToPlayers[target].IsAlive || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(r.UseId)?.MouSimaYiSecret is not { Released: true })
            throw new InvalidOperationException("Pingliao prohibits only the exact original living target which did not Play a red Basic.");
        getCoreGameEngineCardUseModulesRuntime().ValidateProgramTurnEffectGrant(f);
        foreach (var red in new[]
        {
            false,
            true
        }

        )
        {
            var grant = getTurnCardUseEffectStore().GrantHandColorRestriction(actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat, f.Id, -2 * target - (red ? 2 : 1), EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), target, red);
            advanceEventRulesAndQueueFact(new HandCardColorRestrictionGrantedEvent(grant));
        }
    }

    public void Declare(ProgramSkillFrame f) => getCharacterSimaYiMouRuntime().IssueMouSmPlayedDeclaration(f);
    public void Play(ProgramSkillFrame f) => getCharacterSimaYiMouRuntime().IssueMouSmPlayed(f);
    public void CompletePlayed(ProgramSkillFrame f) => getCharacterSimaYiMouRuntime().BeginMouSmPlayedCompleted(f);
}
