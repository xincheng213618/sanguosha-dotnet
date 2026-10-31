#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCommittedResponseUsesRuntime
{
    internal bool TryBeginCommittedResponseUsePrograms(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (_coreResponseUseCompletionOperations.IsCompletedResponseUse(action, continuation) && (continuation != ProgramCardContinuation.Dodge || attack is not null && attack.EffectiveCardKind is { } incomingGuqu && CoreNativeCardClassificationsRuntime.IsSlashCard(incomingGuqu) && attack.TargetSeat == action.ActorSeat))
        {
            var guquParent = continuation == ProgramCardContinuation.NullificationResponse ? (ResolutionFrame? )_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow : attack is null ? null : _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
            if (guquParent is not null)
                _characterZhouYuOlMouOperations.CaptureMouGuquResponseUse(guquParent, action, continuation);
        }

        if (_characterCaoChunOlOperations.TryBeginCaoChunPaidResponseDebt(attack, action, continuation))
            return true;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreResponseUseCompletionOperations.IsCompletedResponseUse(action, continuation) || continuation == ProgramCardContinuation.Dodge && (attack is null || attack.EffectiveCardKind is not { } incoming || !CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) || attack.TargetSeat != action.ActorSeat))
            return false;
        if (!_coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && (_coreNationalSkillsOperations.GetSkillBindingShard(owner)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted) ?? []).Any(binding => binding.Trigger.IncludeResponseUses)) && !_characterSunHaoOlOperations.HasSunHaoCanshiResponseObligation(action, continuation))
            return false;
        return _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, [], continuation: null, completedResponseReturn: new(continuation == ProgramCardContinuation.Dodge ? ProgramCompletedResponseKind.Dodge : ProgramCompletedResponseKind.Nullification, IsCommitted: true));
    }

    internal void ContinueCommittedResponseUse(NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame frame)
    {
        var typed = frame.CompletedResponseReturn ?? throw new InvalidOperationException("Committed response lost typed continuation.");
        var continuation = typed.Kind == ProgramCompletedResponseKind.Dodge ? ProgramCardContinuation.Dodge : ProgramCardContinuation.NullificationResponse;
        if (!typed.IsCommitted || !_coreResponseUseCompletionOperations.IsCompletedResponseUse(frame.Action, continuation))
            throw new InvalidOperationException("Committed response actor or kind changed.");
        if (continuation == ProgramCardContinuation.NullificationResponse)
        {
            if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, frame.Action, SkillProgramTriggerWindow.CardResponseAccepted, [], continuation))
                return;
            _coreResponseUseCompletionOperations.ContinueNullificationAfterResponseUse(frame.Action);
        }
        else
        {
            if (attack is null)
                throw new InvalidOperationException("Committed defense lost its Slash.");
            if (_coreCardActionsOperations.TryBeginProgramCardWindow(attack, frame.Action, SkillProgramTriggerWindow.CardResponseAccepted, [frame.Action.OpponentSeat!.Value], continuation))
                return;
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, frame.Action, continuation);
        }
    }
}
