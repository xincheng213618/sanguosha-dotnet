#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineResponseUseCompletionRuntime
{
    internal bool IsCompletedResponseUse(CardActionContext action, ProgramCardContinuation continuation) => RuntimeSkills.RuntimeRules_Core_ResponseUseCompletion.IsCompletedResponseUse(action, continuation, _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action));
    internal bool IsCompletedResponseParentConsistent(ProgramCardTriggerWindowFrame window)
    {
        if (window.CompletedResponseReturn?.Kind == ProgramCompletedResponseKind.Nullification)
        {
            var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow;
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == window.ParentFrameId)is NullificationWindowFrame suspended && _characterLiuChengMouMobileOperations.IsMouLiuChengSuspendedCounterspell(suspended))
                pending = suspended;
            if (pending is null || window.ParentFrameId != pending.Id || window.Action.OpponentSeat != pending.SourceSeat)
                return false;
            return _coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(pending) && window.Action.ParentActionId == _coreDelayedNullificationOperations.Value.NullificationParentActionId(pending);
        }

        if (window.CompletedResponseReturn?.Kind != ProgramCompletedResponseKind.Dodge || window.AttackOwnerFrameId is not { } ownerId || window.ParentFrameId != ownerId)
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == ownerId);
        return use?.CardAttack is { } attack && window.Action.ActorSeat == attack.TargetSeat && window.Action.OpponentSeat == attack.SourceSeat && window.Action.ParentActionId == use.Action?.ActionId;
    }

    internal bool IsCompletedResponseUse(CardActionContext action, ProgramCompletedResponseKind kind) => RuntimeSkills.RuntimeRules_Core_ResponseUseCompletion.IsCompletedResponseUse(action, kind, _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action));
    internal bool HasResponseUseCompletionObserver(CardActionContext action, ProgramCardContinuation continuation) => IsCompletedResponseUse(action, continuation) && (_programPipelineHooksOperations._programPipelineHooks.ObservesResponseUseCompletion(action) || _characterLiFengOlOperations.HasJiyunResponseCompletion(action, continuation) || _coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && (_coreNationalSkillsOperations.GetSkillBindingShard(owner)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCompleted) ?? []).Any(binding => binding.Trigger.IncludeResponseUses)));
    internal bool TryBeginCompletedResponseUsePrograms(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        _characterCaoXiOlOperations.CompleteCaoXiNativeCounterspellBeforeUseCompleted(action, continuation);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !HasResponseUseCompletionObserver(action, continuation))
            return false;
        // Arrow Barrage Dodge and supplied faction cards are response-only. A Dodge use
        // must be the responder's own defense against the actual Slash parent.
        if (continuation == ProgramCardContinuation.Dodge && (attack is null || attack.EffectiveCardKind is not { } incomingKind || !CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind) || attack.TargetSeat != action.ActorSeat))
            return false;
        var kind = continuation == ProgramCardContinuation.Dodge ? ProgramCompletedResponseKind.Dodge : ProgramCompletedResponseKind.Nullification;
        return _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCompleted, [], continuation: null, cardUseCausedDamage: false, completedResponseReturn: new(kind));
    }

    internal Suit? FreezeCompletedResponseUseSuit(CharacterState owner, IReadOnlyList<Card> cards, CardKind kind)
    {
        _characterHuangGaiMouMobileOperations.PrepareMouZhaJiangResponse(owner, cards.Select(c => c.Id).ToArray(), kind);
        if (!_characterMaChaoAnniversaryGodOperations.HasHengwuCapability && !_characterLiFengOlOperations.HasLiFengCapability && !_characterMaMiDiOlOperations.HasMaMiDiCapability && kind is not (CardKind.Dodge or CardKind.Nullification) || !(_characterMaChaoAnniversaryGodOperations.HasHengwuCapability || _characterZouShiLeOperations.HasLeZouShiCapability || _characterMiHengLeOperations.HasSiruiCapability || _characterLiFengOlOperations.HasLiFengCapability || _characterCuiYanOlOperations.HasCuiYanCapability || _characterMaMiDiOlOperations.HasMaMiDiCapability || _characterWuAnGuoOlOperations.HasWuAnGuoCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.CapturesCompletedResponseSuit || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RevealRecordedSuitSlash) || _coreCurrentTurnUseKindsOperations.TracksCurrentTurnUseKinds && kind == CardKind.Nullification || (_coreCurrentTurnUseKindsOperations.TracksActualTurnUseOrdinal || _coreWholeActualTurnUsesOperations.TracksWholeActualTurnUseOrdinal) && kind == CardKind.Dodge))
            return null;
        var suits = cards.Select(card => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)).Distinct().ToArray();
        return suits.Length == 1 ? suits[0] : null;
    }

    internal void ContinueNullificationAfterResponseUse(CardActionContext action)
    {
        if (_coreResponseCompletionOperations.TryBeginCardResponseCompleted(null, action, ProgramCardContinuation.NullificationResponse))
            return;
        if (!TryBeginCompletedResponseUsePrograms(null, action, ProgramCardContinuation.NullificationResponse))
            _coreNativeNullificationOperations.ContinueNullificationWindow(_coreTieredRoundZeroResponsesOperations.ClearTieredRoundCounterspellResponse(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("The completed counterspell lost its original chain.")));
    }

    internal void ContinueCompletedResponseUse(NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame frame)
    {
        var completed = frame.CompletedResponseReturn ?? throw new InvalidOperationException("The completed response use lost its typed continuation.");
        if (!IsCompletedResponseUse(frame.Action, completed.Kind))
            throw new InvalidOperationException("A completed response use changed its actor or card kind.");
        if (completed.Kind == ProgramCompletedResponseKind.Nullification)
            _coreNativeNullificationOperations.ContinueNullificationWindow(_coreTieredRoundZeroResponsesOperations.ClearTieredRoundCounterspellResponse(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("The completed counterspell lost its original chain.")));
        else
            _coreNativeDodgeOperations.ContinueFinishedCardResponse(attack ?? throw new InvalidOperationException("The completed Dodge lost its original Slash."), frame.Action, ProgramCardContinuation.Dodge);
    }
}
