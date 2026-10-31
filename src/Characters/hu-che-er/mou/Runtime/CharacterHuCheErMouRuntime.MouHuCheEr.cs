#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    internal MouHuCheErRuntime MouHuCheErRuntime => _mouHuCheErOperations.Get();
    internal bool HasMouHuCheEr => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouHuCheErKongwu);

    internal bool ValidMouHuCheEr(ProgramSkillFrame f) => MouHuCheErRuntime.Valid(f);
    internal void ContinueMouHuCheEr(long id) => MouHuCheErRuntime.Continue(id);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 35800, SkillProgramEffectOp.ResolveMouHuCheErKongwu)]
    internal bool CanStartMouHuCheEr(CharacterState owner) => MouHuCheErRuntime.CanStart(owner.Seat);
    int IMouHuCheErFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber;

    long IMouHuCheErFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouHuCheErFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouHuCheErFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouHuCheErFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool IMouHuCheErFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    bool IMouHuCheErFacts.HasHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    int IMouHuCheErFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int IMouHuCheErFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    int IMouHuCheErFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    string IMouHuCheErFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string IMouHuCheErFacts.SkillName(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Name;
    string IMouHuCheErFacts.SkillDescription(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Description;
    SkillProgram IMouHuCheErFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    bool IMouHuCheErFacts.Owns(ProgramSkillFrame f) => _coreProgramLifecycleOperations.CanInvokeProgramSkill(f);
    ActualDiscardRecoveryPhaseKey? IMouHuCheErFacts.Phase() => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
    bool IMouHuCheErFacts.Recorded(ActualDiscardRecoveryPhaseKey p) => _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p);
    IReadOnlyList<int> IMouHuCheErFacts.Targets(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && t.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(t).Count > 0).Select(t => t.Seat).ToArray());
    IReadOnlyList<MouHuCheErHand> IMouHuCheErFacts.Hand(int target) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select((c, i) => new MouHuCheErHand(c.Id, i, c.Kind, _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[target], c))).ToArray());
    bool IMouHuCheErFacts.InHand(int target, int id) => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(target);
    SkillProgramCardCategory IMouHuCheErFacts.Category(CardKind kind) => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
    IReadOnlyList<int> IMouHuCheErFacts.SameSuitDiscards(int target, Suit suit) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Where(c => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[target], c) == suit && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(target, c, CardLocation.Hand(target), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray());
    int IMouHuCheErFacts.CategoryCount(int target, SkillProgramCardCategory category) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(target)).Count(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == category);
    IEnumerable<T> IMouHuCheErFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    MouHuCheErDamageEvent? IMouHuCheErFacts.Damage(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not NativeOwners.CardAttackHandle a || a.IsChainPropagation || _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId)is not { MouHuCheErSlash: { } o } use || !IsIssuedMouHuCheErUse(use) || applied.TargetSeat != o.Target || a.TargetSeat != o.Target || !a.DamageWasApplied)
            return null;
        return new(o.FrameId, use.Id, applied.TargetSeat, applied.Amount);
    }

    void IMouHuCheErNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMouHuCheErNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool IMouHuCheErNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouHuCheErNative.Reveal(ProgramSkillFrame f)
    {
        if (!ValidMouHuCheEr(f) || f.MouHuCheEr is not { Stage: MouHuCheErStage.DiscardChildren } r || _cardZonesOperations._cardZones.GetLocation(r.Shown) != CardLocation.Hand(r.Target))
            throw new InvalidOperationException("Kongwu reveal requires its actual selected Hand entity.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.Shown))]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuCheErShownEvent(f.Id, r.Target, r.Shown, r.Suit, r.Category));
    }

    void IMouHuCheErNative.Discard(ProgramSkillFrame f)
    {
        if (!ValidMouHuCheEr(f) || f.MouHuCheEr is not { Stage: MouHuCheErStage.DiscardChildren } r || !r.Discards.SequenceEqual(_characterHuCheErMouOperations.CreateMouHuCheErFacts().SameSuitDiscards(r.Target, r.Suit)))
            throw new InvalidOperationException("Kongwu discards require its actual payable whole same-suit Hand group.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, r.Discards, new(MouHuCheErRuntime.MouHuCheErDiscardReason), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = parent.MouHuCheEr!;
            if (!moves.Select(m => m.CardId).SequenceEqual(r.Discards) || moves.Any(m => m.From != CardLocation.Hand(r.Target) || m.Sequence <= receipt.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Kongwu lost its real same-suit discard entities.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouHuCheEr = receipt with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuCheErDiscardedEvent(f.Id, r.Target, m.CardId, m.Sequence));
        });
    }

    bool IMouHuCheErNative.CanSlash(ProgramSkillFrame f) => CanMouHuCheErSlash(f);
    void IMouHuCheErNative.Slash(ProgramSkillFrame f) => IssueMouHuCheErSlash(f);
}
