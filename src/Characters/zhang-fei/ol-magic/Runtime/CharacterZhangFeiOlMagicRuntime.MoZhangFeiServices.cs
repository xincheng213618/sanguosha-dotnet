#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiOlMagicRuntime
{
    internal NativeDrawPileSlashProof? NativeDrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => _moZhangFeiOperations.Get().GreenReplacementProof(frame, context);
    internal bool IsIssuedNativeDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => _moZhangFeiOperations.Get().IsIssuedGreenReplacement(context, replacement);
    bool global::CardGame.Core.IMoZhangFeiFacts.Enabled => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoZhangFeiZhuohun);

    bool global::CardGame.Core.IMoZhangFeiFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    Winner global::CardGame.Core.IMoZhangFeiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IMoZhangFeiFacts.ActualTurn => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IMoZhangFeiFacts.TurnOwner => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int global::CardGame.Core.IMoZhangFeiFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int global::CardGame.Core.IMoZhangFeiFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    bool global::CardGame.Core.IMoZhangFeiFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    int global::CardGame.Core.IMoZhangFeiFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int global::CardGame.Core.IMoZhangFeiFacts.MaximumHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    int global::CardGame.Core.IMoZhangFeiFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    IReadOnlyList<string> global::CardGame.Core.IMoZhangFeiFacts.ZhuohunGrantIds(int seat) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Where(g => g.SkillId == MoZhangFeiRuntime.Zhuohun && !g.IsModeRule).Select(g => g.GrantId).ToArray());
    bool global::CardGame.Core.IMoZhangFeiFacts.AnyDying => _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any();

    long global::CardGame.Core.IMoZhangFeiFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IMoZhangFeiFacts.Candidates(int seat, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], window);
    SkillProgramTrigger global::CardGame.Core.IMoZhangFeiFacts.Trigger(ProgramTriggerCandidate c) => _coreProgramLifecycleOperations.GetProgramTrigger(c);
    SkillProgram global::CardGame.Core.IMoZhangFeiFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> global::CardGame.Core.IMoZhangFeiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    CardKind? global::CardGame.Core.IMoZhangFeiFacts.DamageKind(long windowId) => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id == windowId ? _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.EffectiveCardKind ?? _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.Card?.Kind : null;
    IReadOnlyList<Card> global::CardGame.Core.IMoZhangFeiFacts.DrawPileSlashes(int maximum) => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => c.Kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash).Take(maximum).ToArray());
    void global::CardGame.Core.IMoZhangFeiOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMoZhangFeiOperations.Recover(ProgramSkillFrame f, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
    void global::CardGame.Core.IMoZhangFeiOperations.LoseHp(ProgramSkillFrame f, int seat, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, seat, amount);
    void global::CardGame.Core.IMoZhangFeiOperations.Suppress(ProgramSkillFrame f, int seat) => _coreCurrentTurnSkillSuppressionsOperations.IssueCurrentTurnNonLockedSkillSuppression(f, seat);
    void global::CardGame.Core.IMoZhangFeiOperations.ObserveSkillRevisions()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
    }

    bool global::CardGame.Core.IMoZhangFeiOperations.RemoveSkillGrant(int owner, string id) => _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.RemoveGrant(id);
    void global::CardGame.Core.IMoZhangFeiOperations.GrantModifiedZhuohun(ProgramSkillFrame f) => _coreProgramSkillChangesOperations.GrantProgramSkills(f, [MoZhangFeiRuntime.ModifiedZhuohun]);
    void global::CardGame.Core.IMoZhangFeiOperations.EnterRumo(ProgramSkillFrame f) => _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
    bool global::CardGame.Core.IMoZhangFeiOperations.BeginRecovery(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    bool global::CardGame.Core.IMoZhangFeiOperations.BeginCharacterState(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    bool global::CardGame.Core.IMoZhangFeiOperations.BeginHpChanged(long id) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    bool global::CardGame.Core.IMoZhangFeiOperations.BeginCardsMoved(long id) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    bool global::CardGame.Core.IMoZhangFeiOperations.BeginSkillsChanged(long id) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void global::CardGame.Core.IMoZhangFeiOperations.GainSlash(ProgramSkillFrame f, Card card, int recipientSeat) => _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(recipientSeat), new(MoZhangFeiRuntime.GreenReason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoZhangFeiGreenMaterialEvent(f.Id, f.MoZhangFei!.DrawAttemptId!.Value, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
    void global::CardGame.Core.IMoZhangFeiOperations.CompleteDrawPileReplacement(ProgramSkillFrame f, IReadOnlyList<int> ids, CardMoveReason reason, long before) => _coreNativeDrawAttemptsOperations.NativeDrawAttempts.CompleteDrawPileSlashReplacement(f, ids, reason, before);
}
