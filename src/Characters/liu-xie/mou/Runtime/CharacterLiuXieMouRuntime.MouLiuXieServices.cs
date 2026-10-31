#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuXieMouRuntime
{
    internal MouLiuXieRuntime MouLiuXie => _mouLiuXieOperations.Get();

    int global::CardGame.Core.IMouLiuXieFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IMouLiuXieFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IMouLiuXieFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    int global::CardGame.Core.IMouLiuXieFacts.TableStepSeat(int from, int offset) => _coreTableOrderOperations.TableStepSeat(from, offset);
    long global::CardGame.Core.IMouLiuXieFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouLiuXieFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    ActualDiscardRecoveryPhaseKey? global::CardGame.Core.IMouLiuXieFacts.Phase => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();

    bool global::CardGame.Core.IMouLiuXieFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouLiuXieFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool global::CardGame.Core.IMouLiuXieFacts.Qun(int seat) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]) == "qun";
    bool global::CardGame.Core.IMouLiuXieFacts.LordQualified(int seat) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], MouLiuXieComposition.Tiancheng) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[seat], MouLiuXieComposition.Tiancheng, null, Role.Lord);
    bool global::CardGame.Core.IMouLiuXieFacts.Friendly(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    string global::CardGame.Core.IMouLiuXieFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string global::CardGame.Core.IMouLiuXieFacts.CardLabel(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName;
    int global::CardGame.Core.IMouLiuXieFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    IReadOnlyList<int> global::CardGame.Core.IMouLiuXieFacts.Discardable(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray());
    int global::CardGame.Core.IMouLiuXieFacts.KeepValue(int seat, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[seat]);
    SkillProgram? global::CardGame.Core.IMouLiuXieFacts.Program(string skill) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(skill)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouLiuXieFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouLiuXieFacts.ExactStart(ProgramSkillFrame f) => f.SkillId == MouLiuXieComposition.Zhanban && f.ActivationId == "adjust-all-hands" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && f.InstructionIndex == 1 && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.ResolveMouLiuXieZhanban && ((global::CardGame.Core.IMouLiuXieFacts)this).Program(f.SkillId)?.GameplayHash == f.GameplayHash && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && ((global::CardGame.Core.IMouLiuXieFacts)this).Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && !((global::CardGame.Core.IMouLiuXieFacts)this).History<MouLiuXieStartedEvent>().Any(x => x.Source.OwnerSeat == f.OwnerSeat && x.Phase == p);
    bool global::CardGame.Core.IMouLiuXieFacts.ChenshengParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouLiuXieComposition.Chensheng || f.TriggerId != "other-actual-turn-end" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } w || w.OwnerSeat != f.OwnerSeat || ((global::CardGame.Core.IMouLiuXieFacts)this).Program(f.SkillId)?.GameplayHash != f.GameplayHash || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat == f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is DeferredTurnEndFrame parent && parent.Id == w.ParentFrameId && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && w.SourceSeat == parent.OwnerSeat && w.TargetSeat == parent.OwnerSeat && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent));
    }

    void global::CardGame.Core.IMouLiuXieNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IMouLiuXieNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool global::CardGame.Core.IMouLiuXieNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouLiuXieNative.Discard(ProgramSkillFrame f)
    {
        if (!MouLiuXie.Valid(f) || f.MouLiuXie is not { Stage: MouLiuXieStage.DiscardChildren } r || r.Paying.Count == 0 || r.Paying.Any(id => !_characterLiuXieMouOperations.CreateMouLiuXieFacts().Discardable(r.Payer).Contains(id)))
            throw new InvalidOperationException("Zhanban must discard this payer's exact legal Hand entities.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Payer, r.Paying, new(MouLiuXieRuntime.DiscardReason), (_, records) =>
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
            if (parent?.MouLiuXie is not { Stage: MouLiuXieStage.DiscardChildren } paid || !MouLiuXie.Valid(parent) || paid.Payer != r.Payer || !paid.Paying.SequenceEqual(r.Paying) || records.Count != r.Paying.Count || records.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || !r.Paying.Contains(m.CardId) || m.From != CardLocation.Hand(r.Payer) || m.Sequence <= paid.Before || m.Reason.Value != MouLiuXieRuntime.DiscardReason || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Zhanban's native discard lost its original parent or primary movement receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent = parent with { MouLiuXie = paid with { After = records.Max(m => m.Sequence) } });
            foreach (var moved in records)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuXieDiscardPaidEvent(f.Id, paid.Payer, moved.CardId, moved.Sequence, paid.IsAdjustment));
        });
    }

    void global::CardGame.Core.IMouLiuXieNative.Damage(ProgramSkillFrame f, int target) => _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
}
