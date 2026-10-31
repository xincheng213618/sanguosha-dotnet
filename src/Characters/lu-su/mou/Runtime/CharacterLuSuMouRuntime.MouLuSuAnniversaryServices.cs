#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMouRuntime
{
    internal MouLuSuAnniversaryRuntime MouLuSuAnniversary => _mouLuSuAnniversaryOperations.Get();
    internal bool HasMouLuSuAnniversary => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuSuMengmou);

    int IMouLuSuAnniversaryFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long IMouLuSuAnniversaryFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouLuSuAnniversaryFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouLuSuAnniversaryFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouLuSuAnniversaryFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool IMouLuSuAnniversaryFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    string IMouLuSuAnniversaryFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    int IMouLuSuAnniversaryFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    int IMouLuSuAnniversaryFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    IReadOnlyList<int> IMouLuSuAnniversaryFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    IReadOnlyList<CardSnapshot> IMouLuSuAnniversaryFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray());
    bool IMouLuSuAnniversaryFacts.InHand(int seat, int id) => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(seat);
    SkillProgram IMouLuSuAnniversaryFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> IMouLuSuAnniversaryFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    bool IMouLuSuAnniversaryFacts.Yin(int owner, string instance) => _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner, MouLuSuAnniversaryRuntime.Mengmou, instance, "yin");
    bool IMouLuSuAnniversaryFacts.Parent(ProgramSkillFrame f)
    {
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack[i - 1].Id != w.ParentFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack[i - 1] switch
        {
            CardsMovedTriggerWindowFrame p => p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            DamageTriggerWindowFrame p => _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(f, p),
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            _ => false
        };
    }

    ActualDiscardRecoveryPhaseKey? IMouLuSuAnniversaryFacts.Phase() => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
    int? IMouLuSuAnniversaryFacts.Peer(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouLuSuGainPeer(c, w);
    (int Actor, int Amount, long UseId)? IMouLuSuAnniversaryFacts.Damage(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.Amount <= 0 || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not DamageTriggerWindowFrame p || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex].ToProgramCandidate() != c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == p.ParentFrameId)is not DamageFrame damage || w.DamageFrameId != damage.Id || w.Amount != damage.Amount || w.SourceSeat != damage.SourceSeat || w.TargetSeat != damage.TargetSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(damage.ParentFrameId)is not { MouLuSuAnniversary: { } o } use || o.Source.OwnerSeat != c.OwnerSeat || o.Source.SkillInstanceId != c.SkillInstanceId || o.Source.SkillId != c.SkillId || o.GameplayHash != c.GameplayHash || damage.SourceSeat != o.Actor || !MouLuSuNativeMatches(use))
            return null;
        return (o.Actor, w.Amount, use.Id);
    }

    IReadOnlyList<LegalAction> IMouLuSuAnniversaryFacts.Uses(ProgramSkillFrame f) => MouLuSuUseActions(f);
    IReadOnlyList<MouLuSuAnniversaryPlayedPlan> IMouLuSuAnniversaryFacts.Plays(ProgramSkillFrame f) => MouLuSuPlayedPlans(f);
    bool IMouLuSuAnniversaryFacts.Declaration(MouLuSuAnniversaryPlayedPlan plan) => plan.Source is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null;
    void IMouLuSuAnniversaryNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMouLuSuAnniversaryNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool IMouLuSuAnniversaryNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouLuSuAnniversaryNative.Yin(ProgramSkillFrame f, bool yin)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.SkillId != MouLuSuAnniversaryRuntime.Mengmou)
            throw new InvalidOperationException("Mengmou state requires its accepted native branch.");
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, f.SkillId, f.SkillInstanceId, "yin", yin);
    }

    void IMouLuSuAnniversaryNative.Reveal(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.Recipient } r || r.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c) != CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("Mingshi requires its actual selected hand entities to reveal.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(r.Cards.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
        foreach (var id in r.Cards)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryShownEvent(f.Id, id));
        _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "mou-lu-su-mingshi-three");
    }

    void IMouLuSuAnniversaryNative.Gift(ProgramSkillFrame f, int card)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.GiftChildren } r || !r.Cards.Contains(card) || !_coreMatchParticipantsPlayersAccess._players[r.Other].IsAlive || _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Hand(f.OwnerSeat))
            throw new InvalidOperationException("Mingshi requires the recipient's exact publicly shown hand entity.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(r.Other), new("skill-program.mou:mingshi.give"), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            if (moves is not [var m] || m.CardId != card || m.From != CardLocation.Hand(f.OwnerSeat) || m.To != CardLocation.Hand(r.Other))
                throw new InvalidOperationException("Mingshi gift changed its physical owner or recipient.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouLuSuAnniversary = parent.MouLuSuAnniversary!with { After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryGiftEvent(f.Id, r.Other, card, m.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
    }

    void IMouLuSuAnniversaryNative.Use(ProgramSkillFrame f) => IssueMouLuSuUse(f);
    void IMouLuSuAnniversaryNative.Declare(ProgramSkillFrame f) => IssueMouLuSuPlayedDeclaration(f);
    void IMouLuSuAnniversaryNative.Play(ProgramSkillFrame f) => IssueMouLuSuPlayed(f);
    void IMouLuSuAnniversaryNative.CompletePlayed(ProgramSkillFrame f) => BeginMouLuSuPlayedCompleted(f);
    void IMouLuSuAnniversaryNative.LoseHp(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
    void IMouLuSuAnniversaryNative.Recover(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
    [PublicHandRevealReturn("mou-lu-su-mingshi-three")]
    internal void ReturnMouLuSuPublicReveal(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary?.Stage != MouLuSuAnniversaryStage.Recipient)
            throw new InvalidOperationException("Mingshi lost its actual publicly shown roster.");
        MouLuSuAnniversary.Continue(f.Id);
    }
}
