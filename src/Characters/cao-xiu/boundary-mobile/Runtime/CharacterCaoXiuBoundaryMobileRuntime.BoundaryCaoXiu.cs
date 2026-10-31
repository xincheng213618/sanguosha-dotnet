#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiuBoundaryMobileRuntime
{
    SkillProgramStepOutcome IBoundaryCaoXiuHost.BeginQingxi(ProgramSkillFrame frame) => BeginBoundaryCaoXiuQingxi(frame);
    internal SkillProgramStepOutcome BeginBoundaryCaoXiuQingxi(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.BoundaryCaoXiuQingxi is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target } context || context.SourceSeat != frame.OwnerSeat || target == frame.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not BeforeDamageProgramWindowFrame window || window.Prevented || window.RedirectedTargetSeat is not null || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageWasApplied: false } attack || attack.SourceSeat != frame.OwnerSeat || attack.TargetSeat != target)
            throw new InvalidOperationException("Boundary Qingxi requires the owner's original pending damage to another role.");
        var distance = _coreNativeRuleQueriesOperations.GetCombatDistance(frame.OwnerSeat, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryCaoXiuQingxi = new(window.Id, attack.ResolutionId, target, distance, Math.Max(1, 4 - distance)) });
        PublishBoundaryCaoXiuQingxi(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishBoundaryCaoXiuQingxi(ProgramSkillFrame frame)
    {
        var draft = frame.BoundaryCaoXiuQingxi!;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]);
        var choices = new List<PromptChoice>();
        choices.AddRange(hand.Where(card => draft.SelectedIds.Contains(card.Id)).Select(card => new PromptChoice(new ChoiceId("boundary-qingxi.unpick-" + card.Id), "取消选择【" + card.DisplayName + "】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "boundary-qingxi", ["branch"] = "unpick" })));
        if (hand.Count >= draft.RequiredCount)
        {
            if (draft.SelectedIds.Count < draft.RequiredCount)
                choices.AddRange(hand.Where(card => !draft.SelectedIds.Contains(card.Id)).Select(card => new PromptChoice(new ChoiceId("boundary-qingxi.card-" + card.Id), "选择弃置【" + card.DisplayName + "】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "boundary-qingxi", ["branch"] = "card" })));
            else
                choices.Add(new(new ChoiceId("boundary-qingxi.discard"), $"弃置所选{(draft.RequiredCount)}张手牌", [], [], new Dictionary<string, string> { ["program-action"] = "boundary-qingxi", ["branch"] = "discard" }));
        }

        choices.Add(new(new ChoiceId("boundary-qingxi.bonus"), "令此伤害+1", [], [], new Dictionary<string, string> { ["program-action"] = "boundary-qingxi", ["branch"] = "bonus" }));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var decision = new PendingDecision(DecisionKind.ProgramTrigger, draft.TargetSeat, $"【{(skill.Name)}】弃置{(draft.RequiredCount)}张手牌，或令此伤害+1。", choices.SelectMany(c => c.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices.ToArray(),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        var remaining = draft.RequiredCount - draft.SelectedIds.Count;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(decision, frame, "qingxi-hand", "branch", ["card"], ["discard"], draft.SelectedIds, remaining, remaining);
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("boundary-qingxi")]
    internal void ResolveBoundaryCaoXiuQingxi(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Qingxi lost its owning program frame.");
        var draft = frame.BoundaryCaoXiuQingxi ?? throw new InvalidOperationException("Qingxi lost its original target choice.");
        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != draft.TargetSeat || choice.Parameters.GetValueOrDefault("program-action") != "boundary-qingxi" || choice.Targets.Count != 0 || branch is not ("card" or "unpick" or "discard" or "bonus"))
            throw new InvalidOperationException("Invalid Qingxi target choice.");
        if (branch == "unpick")
        {
            if (choice.Cards is not [var card] || !draft.SelectedIds.Contains(card) || _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Hand(draft.TargetSeat))
                throw new InvalidOperationException("Qingxi can undo only its staged unspent hand entity.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryCaoXiuQingxi = draft with { SelectedIds = draft.SelectedIds.Where(id => id != card).ToArray() } });
            PublishBoundaryCaoXiuQingxi(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        if (branch == "card")
        {
            if (choice.Cards is not [var card] || draft.SelectedIds.Count >= draft.RequiredCount || draft.SelectedIds.Contains(card) || _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Hand(draft.TargetSeat))
                throw new InvalidOperationException("Qingxi can stage only a distinct original hand entity.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryCaoXiuQingxi = draft with { SelectedIds = [..draft.SelectedIds, card] } });
            PublishBoundaryCaoXiuQingxi(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        if (choice.Cards.Count != 0 || branch == "discard" && draft.SelectedIds.Count != draft.RequiredCount)
            throw new InvalidOperationException("Qingxi requires its entire frozen hand-card payment.");
        var window = (BeforeDamageProgramWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == draft.DamageWindowId);
        var attack = _coreProgramAttackAttemptOperations.CurrentDamageAttempt!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryCaoXiuQingxi = null });
        var after = window.Amount;
        if (branch == "bonus")
        {
            attack.IncreaseFinalizedDamageAmount(1);
            after = checked(window.Amount + 1);
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Amount = after });
        }
        else
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, draft.TargetSeat, draft.SelectedIds, new CardMoveReason("skill-program.boundary-qingxi.discard"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryCaoXiuQingxiResolvedEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), draft.AttackId, draft.TargetSeat, draft.Distance, draft.RequiredCount, branch == "discard", window.Amount, after));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryCaoXiuQingxi)]
    internal static PromptChoice SelectAiBoundaryCaoXiuQingxi(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? decision.Choices.Single();
}
