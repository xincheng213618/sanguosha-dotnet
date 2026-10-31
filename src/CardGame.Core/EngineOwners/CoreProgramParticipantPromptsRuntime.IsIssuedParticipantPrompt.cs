#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramParticipantPromptsRuntime
{
    internal bool IsIssuedParticipantPrompt(PendingDecision issued) => issued.ParticipantFrameId is> 0 && issued.Kind == DecisionKind.ProgramTrigger && issued.PromptId.Value > 0 && issued.IsPrivate && issued.SkillPrompt is not null && issued.ValidCardIds.Count == 0 && issued.Choices.Count == 0 && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Step: ResolutionFrameStep.AwaitingResponse } owner && owner.Id == issued.ParticipantFrameId;
    internal PendingDecision? GetPendingDecisionForActor(int actorSeat)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } issued)
            return null;
        if (issued.ParticipantFrameId is null)
            return issued.PlayerSeat == actorSeat ? issued : null;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !IsIssuedParticipantPrompt(issued))
            return null;
        var decision = _programPipelineHooksOperations._programPipelineHooks.ParticipantPrompt(issued, actorSeat);
        if (decision is null)
            return null;
        if (decision.PlayerSeat != actorSeat || decision.Kind != issued.Kind || decision.ParticipantFrameId != issued.ParticipantFrameId || decision.PromptId != issued.PromptId || decision.Revision != issued.Revision || decision.SourceSeat != issued.SourceSeat || decision.SkillPrompt != issued.SkillPrompt || !decision.IsPrivate)
            throw new InvalidOperationException("A participant projection must retain its issued prompt identity and actor privacy.");
        return global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime.CloneDecision(decision);
    }

    internal bool HasParticipantInputBoundary => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } issued && IsIssuedParticipantPrompt(issued);

    internal bool IsParticipantInputBoundary(ParticipantPromptHostKey key) => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } issued && IsIssuedParticipantPrompt(issued) && issued.ParticipantFrameId == key.FrameId && issued.PromptId == key.PromptId;
    internal ParticipantPromptHostState? CaptureParticipantHostState()
    {
        var state = _programPipelineHooksOperations._programPipelineHooks.ParticipantHostState();
        if (state is null)
            return null;
        var key = state.Key;
        var owner = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == key.FrameId);
        if (owner is null || key.PromptId.Value <= 0 || key.Source != new CardConversionSource(owner.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(owner), owner.OwnerSeat, owner.SkillInstanceId) || key.GameplayHash != owner.GameplayHash || state.RecipientSeat != owner.OwnerSeat || state.PendingNpcSeats.Distinct().Count() != state.PendingNpcSeats.Count || state.PendingNpcSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || _coreMatchParticipantsPlayersAccess._players[seat].IsHuman || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
            throw new InvalidOperationException("Participant host state must retain its live owner and distinct pending NPCs.");
        var accepting = state.CanAcceptInput && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } issued && IsIssuedParticipantPrompt(issued) && issued.ParticipantFrameId == key.FrameId && issued.PromptId == key.PromptId;
        return state with
        {
            PendingNpcSeats = Array.AsReadOnly(state.PendingNpcSeats.ToArray()),
            CanAcceptInput = accepting
        };
    }

    internal ParticipantGiftWork? CaptureParticipantGiftWork(ParticipantPromptHostState state, int actor)
    {
        if (!state.CanAcceptInput || !state.PendingNpcSeats.Contains(actor))
            return null;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(actor);
        if (view.PendingDecision is not { } prompt || prompt.ParticipantFrameId != state.Key.FrameId || prompt.PromptId != state.Key.PromptId || prompt.PlayerSeat != actor)
            return null;
        if (prompt.Choices.Any(choice => choice.Targets.Count != 0 || choice.Cards.Count > 1))
            throw new InvalidOperationException("The participant gift host accepts only published optional single-entity gifts.");
        var decline = prompt.Choices.Single(choice => choice.Cards.Count == 0).Id;
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[actor].ForkParticipantDecision();
        var recipient = state.RecipientSeat;
        // Only this detached brain and frozen actor view enter the worker.
        return new(state.Key, actor, view.Revision, () => brain.ChooseOptionalOwnedCardGift(view, recipient, prompt.Choices, decline));
    }
}
