#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiOlMagicRuntime
{
    internal NativeDrawPileSlashProof? NativeDrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => _moZhangFeiOperations.Get().GreenReplacementProof(frame, context);
    internal bool IsIssuedNativeDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => _moZhangFeiOperations.Get().IsIssuedGreenReplacement(context, replacement);
    bool IMoZhangFeiFacts.Enabled => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoZhangFeiZhuohun);

    bool IMoZhangFeiFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    Winner IMoZhangFeiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IMoZhangFeiFacts.ActualTurn => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int IMoZhangFeiFacts.TurnOwner => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int IMoZhangFeiFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int IMoZhangFeiFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    bool IMoZhangFeiFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    int IMoZhangFeiFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int IMoZhangFeiFacts.MaximumHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    int IMoZhangFeiFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    IReadOnlyList<string> IMoZhangFeiFacts.ZhuohunGrantIds(int seat) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Where(g => g.SkillId == MoZhangFeiRuntime.Zhuohun && !g.IsModeRule).Select(g => g.GrantId).ToArray());
    bool IMoZhangFeiFacts.AnyDying => _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any();

    long IMoZhangFeiFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    IReadOnlyList<ProgramTriggerCandidate> IMoZhangFeiFacts.Candidates(int seat, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], window);
    SkillProgramTrigger IMoZhangFeiFacts.Trigger(ProgramTriggerCandidate c) => _coreProgramLifecycleOperations.GetProgramTrigger2(c);
    SkillProgram IMoZhangFeiFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> IMoZhangFeiFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    CardKind? IMoZhangFeiFacts.DamageKind(long windowId) => _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger?.Id == windowId ? _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.EffectiveCardKind ?? _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.Card?.Kind : null;
    IReadOnlyList<Card> IMoZhangFeiFacts.DrawPileSlashes(int maximum) => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => c.Kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash).Take(maximum).ToArray());
    void IMoZhangFeiOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMoZhangFeiOperations.Recover(ProgramSkillFrame f, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
    void IMoZhangFeiOperations.LoseHp(ProgramSkillFrame f, int seat, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, seat, amount);
    void IMoZhangFeiOperations.Suppress(ProgramSkillFrame f, int seat) => _coreProgramSkillChangesOperations.ApplyProgramSkillState(f, seat,
        new(SkillStateAction.Suppress, SkillStateScope.NonLocked, SkillStateDuration.CurrentTurn) { CharacterOnly = false }, []);
    void IMoZhangFeiOperations.ObserveSkillRevisions()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
    }

    bool IMoZhangFeiOperations.RemoveSkillGrant(int owner, string id) => _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.RemoveGrant(id);
    void IMoZhangFeiOperations.GrantModifiedZhuohun(ProgramSkillFrame f) => _coreProgramSkillChangesOperations.GrantProgramSkills(f, [MoZhangFeiRuntime.ModifiedZhuohun]);
    void IMoZhangFeiOperations.EnterRumo(ProgramSkillFrame f) => _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
    bool IMoZhangFeiOperations.BeginRecovery(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    bool IMoZhangFeiOperations.BeginCharacterState(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    bool IMoZhangFeiOperations.BeginHpChanged(long id) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    bool IMoZhangFeiOperations.BeginCardsMoved(long id) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    bool IMoZhangFeiOperations.BeginSkillsChanged(long id) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void IMoZhangFeiOperations.GainSlash(ProgramSkillFrame f, Card card, int recipientSeat) => _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(recipientSeat), new(MoZhangFeiRuntime.GreenReason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoZhangFeiGreenMaterialEvent(f.Id, f.MoZhangFei!.DrawAttemptId!.Value, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
    void IMoZhangFeiOperations.CompleteDrawPileReplacement(ProgramSkillFrame f, IReadOnlyList<int> ids, CardMoveReason reason, long before) => _coreNativeDrawAttemptsOperations.NativeDrawAttempts.CompleteDrawPileSlashReplacement(f, ids, reason, before);
}
