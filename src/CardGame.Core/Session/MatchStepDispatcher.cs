namespace CardGame.Core;

/// <summary>
/// Chooses one synchronous rule step. Each protocol retains its admission slot,
/// short-circuit order and state-publication boundary.
/// </summary>
[ProgramRuntimeModule]
internal sealed class MatchStepDispatcher(IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    IMatchStepFacts facts, IMatchStepProtocols protocols, IMatchStepDecisions decisions, IMatchStepTurns turns)
{
    internal bool HasUnfinishedAutomaticDecision() =>
        decisions.Pending(MatchAutomaticDecision.Response) ||
        decisions.Pending(MatchAutomaticDecision.Nullification) ||
        decisions.Pending(MatchAutomaticDecision.DyingResponse) ||
        decisions.Pending(MatchAutomaticDecision.TargetCardSelection) ||
        decisions.Pending(MatchAutomaticDecision.Harvest) ||
        decisions.Pending(MatchAutomaticDecision.FireAttack) ||
        decisions.Pending(MatchAutomaticDecision.ProgramJudgmentReplacement) ||
        decisions.Pending(MatchAutomaticDecision.ProgramJudgment) ||
        decisions.Pending(MatchAutomaticDecision.ProgramTopReorder) ||
        decisions.Pending(MatchAutomaticDecision.ProgramRepeatJudgment) ||
        decisions.Pending(MatchAutomaticDecision.Yingbo) ||
        decisions.Pending(MatchAutomaticDecision.SkipDiscardPolicy) ||
        decisions.Pending(MatchAutomaticDecision.Pindian);

    internal void Run()
    {
        if (protocols.TryAdvanceAutomatic(NativeFrameAutomaticPhase.BeforeOpening)) return;
        if (prompts.Current is null && facts.FrameCount == 0 && protocols.TryBeginOpening())
        { protocols.PublishState(); return; }
        if (protocols.TryAdvanceAutomatic(NativeFrameAutomaticPhase.AfterOpening)) return;
        if (frames.Top is RecoveryReplacementFrame recoveryReplacement)
        {
            if (prompts.Current is { Kind: DecisionKind.RecoveryReplacement } decision)
            {
                if (!facts.IsHuman(decision.PlayerSeat)) decisions.Resolve(MatchAutomaticDecision.RecoveryReplacement);
            }
            else { frames.Advance(recoveryReplacement.Id); protocols.PublishState(); }
            return;
        }
        // These pre-existing request families retain their own native AI rule
        // implementations until their module-owned registration is migrated.
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.Huanpei)) return;
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.Zonghu)) return;
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.BoundaryLiuFeng)) return;
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.Weijie)) return;
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.RequestedDeckBasic)) return;
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.DrawFundedDistinctBasic)) return;
        if (decisions.TryAdvanceRequest(MatchSpecialRequest.ChainedStateBasic)) return;
        if (protocols.TryAdvanceHook(ProgramRuntimePhase.BeforeDeclaration)) return;
        if (frames.Top is CardDeclarationChallengeFrame challenge)
        {
            if (prompts.Current is null) decisions.ContinueDeclaration(challenge.Id);
            else if (!facts.IsHuman(prompts.Current.PlayerSeat))
                decisions.AnswerDeclaration(prompts.Current.Choices.Single(choice =>
                    choice.Parameters.GetValueOrDefault("declaration-answer") == "pass"));
            protocols.PublishState();
            return;
        }
        if (protocols.TryAdvanceAutomatic(NativeFrameAutomaticPhase.AfterDeclarationChallenge)) return;
        if (prompts.Current is { Kind: DecisionKind.ProgramTrigger } triggerDecision)
        {
            if (!facts.IsHuman(triggerDecision.PlayerSeat)) decisions.Resolve(MatchAutomaticDecision.ProgramTrigger);
            return;
        }

        if (frames.Top is ProgramSkillFrame { TopReorder.Population: not null } &&
            decisions.Pending(MatchAutomaticDecision.ProgramTopReorder))
        {
            decisions.Resolve(MatchAutomaticDecision.ProgramTopReorder);
            return;
        }
        if (protocols.TryAdvanceAutomatic(NativeFrameAutomaticPhase.BeforeJudgmentDecision)) return;
        if (prompts.Current is { Kind: DecisionKind.ProgramJudgmentTrigger })
        {
            if (decisions.Pending(MatchAutomaticDecision.ProgramJudgment))
                decisions.Resolve(MatchAutomaticDecision.ProgramJudgment);
            return;
        }
        if (TryResolve(MatchAutomaticDecision.ProgramTopReorder)) return;
        if (TryResolve(MatchAutomaticDecision.ProgramRepeatJudgment)) return;
        if (TryResolve(MatchAutomaticDecision.ProgramJudgmentReplacement)) return;
        if (TryResolve(MatchAutomaticDecision.Yingbo)) return;
        if (TryResolve(MatchAutomaticDecision.SkipDiscardPolicy)) return;
        if (prompts.Current?.SkillPrompt is not null)
        {
            if (decisions.Pending(MatchAutomaticDecision.Pindian)) decisions.Resolve(MatchAutomaticDecision.Pindian);
            return;
        }

        if (protocols.TryAdvanceHook(ProgramRuntimePhase.BeforeResponseHealth)) return;
        if (prompts.Current is null &&
            frames.Top is (ProgramSkillFrame or HpChangedTriggerWindowFrame or CardsMovedTriggerWindowFrame or ProgramLifecycleTriggerWindowFrame) &&
            (protocols.HasTypedResponseCompletionHealthCursor() || protocols.HasTypedCommittedSlashPaymentCursor()))
        {
            frames.Advance(frames.Top!.Id);
            protocols.PublishState();
            return;
        }
        if (protocols.TryAdvanceHook(ProgramRuntimePhase.AfterResponseHealth)) return;

        if (protocols.IsCurrentWenhePoisonDying() && facts.ActiveDying is { } poisonDying &&
            frames.Top is ProgramSkillFrame { WindowContext: { } poisonContext } poisonResponse &&
            poisonContext.ParentFrameId == poisonDying.Id && poisonContext.TargetSeat == poisonDying.VictimSeat &&
            (poisonContext.Window == SkillProgramTriggerWindow.DyingResponse && poisonResponse.OwnerSeat == poisonDying.ResponderSeat ||
             poisonContext.Window == SkillProgramTriggerWindow.SelfDyingResponse && poisonResponse.OwnerSeat == poisonDying.VictimSeat))
        {
            frames.AdvanceProgram(poisonResponse.Id);
            protocols.PublishState();
            return;
        }

        // The direct recovery protocol proves its producer, return edges and
        // exact Dying before the capability-specific subtree proofs are read.
        if (facts.ActiveDying is { } recoveryDying &&
            frames.Top is ProgramSkillFrame recoveryResponse &&
            recoveryResponse.WindowContext?.ParentFrameId == recoveryDying.Id && protocols.HasTypedRecoveryProgramDying())
        {
            frames.AdvanceProgram(recoveryResponse.Id);
            protocols.PublishState();
            return;
        }
        if (facts.ActiveDying is { } paidDying &&
            frames.Top is ProgramSkillFrame { WindowContext: { } paidContext } paidProgram &&
            facts.FrameCount >= 2 && frames.ParentOf(paidProgram.Id) is DyingFrame paidParent && paidParent.Id == paidDying.Id &&
            paidContext.ParentFrameId == paidDying.Id && paidContext.TargetSeat == paidDying.VictimSeat &&
            paidContext.OwnerSeat == paidProgram.OwnerSeat &&
            (paidContext.Window == SkillProgramTriggerWindow.DyingResponse && paidProgram.OwnerSeat == paidDying.ResponderSeat ||
             paidContext.Window == SkillProgramTriggerWindow.SelfDyingResponse && paidProgram.OwnerSeat == paidDying.VictimSeat) &&
            protocols.HasRuntimeDyingProof())
        {
            frames.AdvanceProgram(paidProgram.Id);
            protocols.PublishState();
            return;
        }
        if (facts.ActiveDying is not null)
        {
            protocols.RunDyingStep();
            return;
        }
        if (TryResolve(MatchAutomaticDecision.TargetCardSelection)) return;
        if (TryResolve(MatchAutomaticDecision.FireAttack)) return;
        if (facts.HasDamageTrigger)
        {
            protocols.RunDamageTriggerStep();
            protocols.PublishState();
            return;
        }
        if (TryResolve(MatchAutomaticDecision.Nullification)) return;
        if (TryResolve(MatchAutomaticDecision.Response)) return;
        if (TryResolve(MatchAutomaticDecision.Harvest)) return;
        if (facts.HasGroupRecovery)
        {
            protocols.RunGroupRecoveryStep();
            return;
        }
        if (!facts.SetupComplete)
        {
            protocols.RunSetupStep();
            return;
        }
        if (!facts.GameStartingProgramsResolved && facts.FrameCount == 0 && protocols.BeginGameStartingPrograms()) return;
        if (frames.Top is ProgramSkillFrame programFrame)
        {
            frames.AdvanceProgram(programFrame.Id);
            protocols.PublishState();
            return;
        }
        if (facts.Phase is TurnPhase.NotStarted or TurnPhase.Finished)
        {
            turns.BeginTurn();
            return;
        }
        if (facts.Phase == TurnPhase.Play)
        {
            if (facts.StrategicEndPlayTurn == facts.TurnNumber)
            {
                turns.ClearStrategicEndPlay();
                turns.CompletePlayPhase();
                return;
            }
            var seat = facts.CurrentSeat;
            var current = facts.Player(seat);
            if (!current.Alive) { turns.EndTurn(); return; }
            if (current.Human) { turns.RequestHumanPlay(); return; }
            turns.RunAiPlayDecision(seat);
            return;
        }
        if (facts.Phase == TurnPhase.Discard)
        {
            var seat = facts.CurrentSeat;
            var current = facts.Player(seat);
            if (facts.UseInteractiveDiscard && current.Human && current.Alive && turns.RequiredDiscard(seat) > 0)
            {
                turns.RequestHumanDiscard(seat);
                return;
            }
            if (turns.AutoDiscardAndTryBeginEndedWindow(seat)) return;
            turns.EndTurn();
            return;
        }
        throw new InvalidOperationException(
            $"No continuation for phase {facts.Phase}; seed {facts.Seed}, turn {facts.TurnNumber}, seat {facts.CurrentSeat}, " +
            $"general {facts.GeneralId(facts.CurrentSeat)}, winner {facts.Winner}, " +
            $"frames [{string.Join(", ", frames.OfType<ResolutionFrame>().Select(frame => $"{frame.Kind}:{frame.Id}"))}], " +
            $"decision {prompts.Current?.Kind}.");
    }

    private bool TryResolve(MatchAutomaticDecision kind)
    {
        if (!decisions.Pending(kind)) return false;
        decisions.Resolve(kind);
        return true;
    }
}

internal readonly record struct MatchStepPlayer(bool Alive, bool Human);

internal enum MatchAutomaticDecision
{
    RecoveryReplacement, ProgramTrigger, ProgramTopReorder, ProgramJudgment,
    ProgramRepeatJudgment, ProgramJudgmentReplacement, Yingbo, SkipDiscardPolicy,
    Pindian, TargetCardSelection, FireAttack, Nullification, Response, Harvest, DyingResponse
}

internal enum MatchSpecialRequest
{
    Huanpei, Zonghu, BoundaryLiuFeng, Weijie, RequestedDeckBasic, DrawFundedDistinctBasic, ChainedStateBasic
}

internal interface IMatchStepFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    DyingFrame? ActiveDying { get; }
    bool HasDamageTrigger { get; }
    bool HasGroupRecovery { get; }
    bool SetupComplete { get; }
    bool GameStartingProgramsResolved { get; }
    TurnPhase Phase { get; }
    int CurrentSeat { get; }
    int TurnNumber { get; }
    int StrategicEndPlayTurn { get; }
    bool UseInteractiveDiscard { get; }
    int Seed { get; }
    Winner Winner { get; }
    bool IsHuman(int seat);
    MatchStepPlayer Player(int seat);
    string GeneralId(int seat);
}

internal interface IMatchStepProtocols : IProgramRuntimeOperations
{
    bool TryAdvanceAutomatic(NativeFrameAutomaticPhase phase);
    bool TryBeginOpening();
    bool TryAdvanceHook(ProgramRuntimePhase phase);
    bool HasTypedResponseCompletionHealthCursor();
    bool HasTypedCommittedSlashPaymentCursor();
    bool IsCurrentWenhePoisonDying();
    bool HasTypedRecoveryProgramDying();
    bool HasRuntimeDyingProof();
    void PublishState();
    void RunDyingStep();
    void RunDamageTriggerStep();
    void RunGroupRecoveryStep();
    void RunSetupStep();
    bool BeginGameStartingPrograms();
}

internal interface IMatchStepDecisions : IProgramRuntimeOperations
{
    bool Pending(MatchAutomaticDecision kind);
    void Resolve(MatchAutomaticDecision kind);
    bool TryAdvanceRequest(MatchSpecialRequest kind);
    void ContinueDeclaration(long frameId);
    void AnswerDeclaration(PromptChoice choice);
}

internal interface IMatchStepTurns : IProgramRuntimeOperations
{
    void ClearStrategicEndPlay();
    void CompletePlayPhase();
    void BeginTurn();
    void EndTurn();
    void RequestHumanPlay();
    void RunAiPlayDecision(int seat);
    int RequiredDiscard(int seat);
    void RequestHumanDiscard(int seat);
    bool AutoDiscardAndTryBeginEndedWindow(int seat);
}
