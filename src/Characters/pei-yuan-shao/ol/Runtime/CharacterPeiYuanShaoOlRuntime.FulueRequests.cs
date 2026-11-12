#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPeiYuanShaoOlRuntime
{
    internal bool HasIssuedFulueRequest(ProgramSkillFrame f) =>
        f.Fulue is { Stage: FulueStage.CounterRequest, Counter: false, Option: not null } r &&
        r.Source.SkillId == f.SkillId && r.Source.SkillInstanceId == f.SkillInstanceId && r.GameplayHash == f.GameplayHash &&
        r.UserSeat == f.OwnerSeat && f.SelectedTargetSeats is [var actor] && actor == r.TargetSeat &&
        f.InstructionIndex is 1 or 2;

    internal bool TryBeginFulueAssistedSlash(ProgramSkillFrame frame, int actorSeat, string bind, out SkillProgramStepOutcome outcome)
    {
        outcome = SkillProgramStepOutcome.Continue;
        if (frame.Fulue?.Stage != FulueStage.CounterRequest)
            return false;
        if (!HasIssuedFulueRequest(frame) || frame.InstructionIndex != 2 || bind != FulueCounterBind || actorSeat != frame.Fulue.TargetSeat || frame.AssistedSlashRequest is not null)
            throw new InvalidOperationException("Fulue's real counter-Slash lost its original completed card and exact requested actor.");
        if (!_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.Fulue.UserSeat].IsAlive || !_coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashTargets(actorSeat).Contains(frame.Fulue.UserSeat))
        {
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(frame.Id, bind, "declined", actorSeat, "反杀参与者已死亡或不在合法攻击范围内。");
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { AssistedSlashRequest = new(actorSeat, frame.Fulue.UserSeat, ActorChoosesTarget: true) });
        _slashRequests.Issue(frame, actorSeat, new(ProgramSlashRequestTargetSource.FrozenCandidates, [frame.Fulue.UserSeat], bind,
            UsesChoiceResult: true, RetainSourceLoss: true, RetainOwnerDeath: true, RetainAcceptedSourceLoss: true, RetainAcceptedOwnerDeath: true, FailureReturnPolicy: ProgramSlashRequestFailureReturnPolicy.ResumeOriginalInstruction));
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreAssistedPhysicalCardsOperations.PublishAssistedPhysicalSlashPrompt(frame);
        outcome = SkillProgramStepOutcome.AwaitChoice;
        return true;
    }

    internal void IssueFulueCounterUse(long useId, CardActionContext action)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId) is not { ProgramSlashRequestExecution: { } execution, FulueCounter: null } use ||
            _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == execution.ProgramFrameId) is not { Fulue: { Stage: FulueStage.CounterRequest, Option: { } option } r } parent)
            return;
        var acceptance = execution.Acceptance;
        if (parent.InstructionIndex != 2 || action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) ||
            use.Action?.ActionId != action.ActionId || !_slashRequests.OwnsAcceptance(parent, acceptance, r.TargetSeat, r.UserSeat))
            throw new InvalidOperationException("Fulue cannot attach its other option to another accepted counter card.");
        var invoice = new FulueCounterInvoice(parent.Id, r.UseId, r.ActionId, r.Source, r.GameplayHash,
            r.ActualTurn, r.TurnOwner, r.TargetSeat, r.UserSeat, OtherFulueOption(option));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { FulueCounter = invoice });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FulueCounterUseIssuedEvent(useId, action.ActionId, invoice));
    }

    internal bool ValidFulueCounterInvoice(CardUseFrame use, FulueCounterInvoice r) =>
        use.FulueCounter == r && use.Action is { Type: CardActionType.Use } action &&
        CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.ProgramSlashRequestExecution is { } execution &&
        execution.ProgramFrameId == r.ProgramFrameId &&
        _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == r.ProgramFrameId) is { Fulue: { } draft } parent &&
        draft.UseId == r.OriginalUseId && draft.ActionId == r.OriginalActionId && draft.Source == r.Source &&
        draft.GameplayHash == r.GameplayHash && draft.TargetSeat == r.ActorSeat && draft.UserSeat == r.TargetSeat &&
        _slashRequests.OwnsAcceptance(parent, execution.Acceptance, r.ActorSeat, r.TargetSeat);
}
