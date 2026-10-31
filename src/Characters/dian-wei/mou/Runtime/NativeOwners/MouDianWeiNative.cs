#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDianWeiNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterDianWeiMouEntries> getCharacterDianWeiMouRuntime, Func<RuntimeEntries.CoreGameEngineNextSlashProgramsEntries> getCoreGameEngineNextSlashProgramsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime, MouDianWeiFactsFactory createMouDianWeiFacts, Func<RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEnginePindianEntries> getCoreGameEnginePindianRuntime, Func<RuntimeEntries.CoreGameEngineForeignTurnContestsEntries> getCoreGameEngineForeignTurnContestsRuntime, Func<RuntimeEntries.CoreActiveNativeAttackQueriesEntries> getCoreActiveNativeAttackQueriesRuntime, Func<RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, Func<RuntimeEntries.CoreProgramFactionActionRewardEntries> getCoreProgramFactionActionRewardRuntime, EngineStates.CoreGameEngineCardActionsState gameEngineCardActionsState, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries> getCoreGameEngineAlternativeViewAsCostsRuntime, Func<RuntimeEntries.CoreNativeOwnedCardQueriesEntries> getCoreNativeOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries> getCoreGameEngineActualPlayPhaseCardUsesRuntime, Func<RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries> getCoreGameEngineDirectedCardPolicyQueriesRuntime, CardAttackHandleFactory2 createCardAttackHandle, Func<RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries> getCoreGameEngineAlcoholSlashSuppressionsRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineCardActionsEntries> getCoreGameEngineCardActionsRuntime, Func<RuntimeEntries.CoreNativeSlashUseEntries> getCoreNativeSlashUseRuntime) : IMouDianWeiNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Recover(ProgramSkillFrame f)
    {
        if (!getCharacterDianWeiMouRuntime().MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.RecoveryChildren })
            throw new InvalidOperationException("Kangyong recovery requires its exact actual-turn-start parent.");
        getCoreGameEngineNextSlashProgramsRuntime().RecoverProgramToMaximum(f);
    }

    public void Lose(ProgramSkillFrame f, int amount)
    {
        if (!getCharacterDianWeiMouRuntime().MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.LossChildren } r || amount != r.ActualLoss || amount < 0 || amount > Math.Max(0, matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].Hp - 1))
            throw new InvalidOperationException("Kangyong repays only actual recovered HP while preserving at least one HP.");
        getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, amount);
    }

    public void Pindian(ProgramSkillFrame f)
    {
        if (!getCharacterDianWeiMouRuntime().MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.Pindian } r || !createMouDianWeiFacts().Opponents(f.OwnerSeat).Contains(r.Opponent))
            throw new InvalidOperationException("Kuangzhan requires one currently legal actual Pindian opponent.");
        var id = getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence + 1;
        var bind = MouDianWeiRuntime.Bind(r);
        var skill = getContentRegistry().GetSkill(f.SkillId);
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, f = f with { MouDianWei = r with { ContestId = id, Before = cardMovementJournalState.ReferenceToMovementSequence, After = cardMovementJournalState.ReferenceToMovementSequence } });
        advanceEventRulesAndQueueFact(new MouDianWeiContestIssuedEvent(f.Id, r.Index, id, r.Opponent, bind));
        getCoreGameEnginePindianRuntime().BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), f.OwnerSeat, r.Opponent, programResultBind: bind, programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    public void Slash(ProgramSkillFrame f, int actor, IReadOnlyList<int> targets)
    {
        if (!getCharacterDianWeiMouRuntime().MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.Slash, Slash: null } r || targets.Count == 0 || targets.Distinct().Count() != targets.Count || targets.Any(s => !getCoreGameEngineForeignTurnContestsRuntime().CanUseForeignTurnContestSlash(actor, s)) || getCoreActiveNativeAttackQueriesRuntime().ActiveCardAttack is not null || getCoreNativeCardContinuationCursorsRuntime().ActiveDuel is not null)
            throw new InvalidOperationException("Kuangzhan must issue its own original comparison's legal native Slash.");
        var result = createMouDianWeiFacts().Result(f);
        var expected = (result.SourceWon ? createMouDianWeiFacts().NotWon(f.OwnerSeat) : new[]
        {
            f.OwnerSeat
        }

        ).Where(s => getCoreGameEngineForeignTurnContestsRuntime().CanUseForeignTurnContestSlash(actor, s)).ToArray();
        if (actor != (result.SourceWon ? f.OwnerSeat : result.OpponentSeat) || !targets.SequenceEqual(expected))
            throw new InvalidOperationException("Kuangzhan cannot substitute its real winner/loser or frozen target set.");
        var id = ++getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence;
        var source = matchParticipantsState.ReferenceToPlayers[actor];
        var action = getCoreProgramFactionActionRewardRuntime().CaptureFactionAction(new CardActionContext(++gameEngineCardActionsState.ReferenceToCardActionSequence, getFrameStore().OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor, actor, null, null, null, CardKind.Slash, targets, [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var ret = new MouDianWeiSlash(f.Id, r.Source, f.GameplayHash, r.Index, r.ContestId, id, action.ActionId, actor)
        {
            Targets = targets
        };
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, f with { MouDianWei = r with { Slash = ret } });
        getCoreGameEngineRuntimeRuntime().PushRuntimeFrame(new CardUseFrame(id, actor, 0, CardKind.Slash, ret.Targets, PhysicalCardIds: []) { Action = action, MouDianWei = ret });
        foreach (var(target, index)in ret.Targets.Select((target, index) => (target, index)))
            advanceEventRulesAndQueueFact(new MouDianWeiSlashIssuedEvent(f.Id, r.Index, r.ContestId, id, action.ActionId, actor, target, index));
        if (getCoreGameEnginePlayCardHistoryRuntime().TracksPlayCardHistory)
            advanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        advanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Slash, actor));
        advanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, ret.Targets));
        getCharacterLuMengMouRuntime().RecordYingboCardUse(id, actor, CardKind.Slash);
        getCoreGameEngineAlternativeViewAsCostsRuntime().RecordProgramUsedBasicCard(actor, CardKind.Slash);
        getCoreNativeOwnedCardQueriesRuntime().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor, CardKind.Slash);
        getCoreGameEngineActualPlayPhaseCardUsesRuntime().RecordActualPlayPhaseUse(action);
        var attack = createCardAttackHandle(id, actor, ret.Targets[0], card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: getCoreGameEngineDirectedCardPolicyQueriesRuntime().HasCardArmorBypass(source, matchParticipantsState.ReferenceToPlayers[ret.Targets[0]], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        getCoreGameEngineAlcoholSlashSuppressionsRuntime().CaptureProgramAlcoholConsumption(id, source);
        source.HasAlcoholEffect = false;
        getCoreActiveNativeAttackQueriesRuntime().ActiveCardAttack = attack;
        advanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor, ret.Targets[0]));
        getCoreGameEngineCardUseLifecycleRuntime().TryMarkProgramUseCommitted(id);
        if (!getCoreGameEngineCardActionsRuntime().TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, ret.Targets, ProgramCardContinuation.CommittedSlash))
            getCoreNativeSlashUseRuntime().BeginSlashTargetResolution(attack);
    }
}
