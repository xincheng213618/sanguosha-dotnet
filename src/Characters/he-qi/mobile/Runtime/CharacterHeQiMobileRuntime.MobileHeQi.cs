#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiMobileRuntime
{
    internal const string MobileHeQiAction = "mobile-he-qi-view";
    internal static CardConversionSource MobileHeQiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IMobileHeQiHost.View(ProgramSkillFrame f) => BeginMobileHeQiView(f);
    internal bool MobileHeQiOriginal(ProgramSkillFrame f) => f.SkillId == MobileHeQiComposition.Skill && f.TriggerId is null && f.ActivationId == MobileHeQiComposition.Activation && f.InstructionIndex == 3 && f.SelectedCardIds.Count == 1 && f.SelectedTargetSeats is [var target] && target != f.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.ResolveMobileHeQiShanxiView && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
    internal NativeLogicalDiscardMaterialCompletedEvent? MobileHeQiDiscard(ProgramSkillFrame f, out NativeLogicalDiscardInvocationRecordedEvent? invoice)
    {
        invoice = null;
        if (!MobileHeQiOriginal(f))
            return null;
        var selected = f.CardSetBindings.SingleOrDefault(b => b.Name == MobileHeQiComposition.DiscardBind);
        if (selected?.CardIds is not [var id])
            return null;
        var invocations = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(i => i.ParentFrameId == f.Id && i.InitiatorSeat == f.OwnerSeat && i.ExecutorSeat == f.OwnerSeat && i.ActualCount == 1 && i.Reason.Value == "skill-program." + f.SkillId + ".SelectAndMoveOwnedCard").ToArray();
        if (invocations is not [var exact])
            return null;
        var material = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == exact.InvocationId && e.CardId == id && e.From.OwnerSeat == f.SelectedTargetSeats[0] && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment).ToArray();
        if (material is not [var paid] || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == paid.CardId && m.Sequence == paid.CompletionSequence && m.Sequence > exact.SequenceBefore && m.Sequence <= exact.SequenceAfter))
            return null;
        invoice = exact;
        return paid;
    }

    internal SkillProgramStepOutcome BeginMobileHeQiView(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MobileHeQiOriginal(f) || f.MobileHeQiView is not null)
            throw new InvalidOperationException("Mobile Shanxi requires its original completed native payment and selected target.");
        if (MobileHeQiDiscard(f, out var invoice)is not { } paid)
            return SkillProgramStepOutcome.Continue;
        var target = f.SelectedTargetSeats[0];
        var kind = _coreAdvancedLifecycleOperations.GetAdvancedCard(paid.CardId).Kind;
        var dodge = kind == CardKind.Dodge;
        var viewer = dodge ? f.OwnerSeat : target;
        var holder = dodge ? target : f.OwnerSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[viewer].IsAlive || !_coreMatchParticipantsPlayersAccess._players[holder].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var r = new MobileHeQiViewReceipt(MobileHeQiSource(f), f.GameplayHash, target, invoice!.InvocationId, paid.CardId, paid.From, kind, viewer, holder)
        {
            Hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Select(c => c.Id).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHeQiView = r, Step = ResolutionFrameStep.AwaitingResponse });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileHeQiHandViewedEvent(f.Id, r.Source, r.InvocationId, r.CardId, r.DiscardedKind, r.Viewer, r.Holder, r.Hand.Count));
        PublishMobileHeQiView(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MobileHeQiConfirm(ProgramSkillFrame f) => new(new("mobile-he-qi.confirm." + f.Id), "确认已观看手牌", [], [], new Dictionary<string, string> { ["program-action"] = MobileHeQiAction });
    internal void PublishMobileHeQiView(ProgramSkillFrame f)
    {
        var r = f.MobileHeQiView!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.Viewer, "闪袭：观看该角色的当前手牌。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Holder,
            Choices = Array.AsReadOnly(new[] { MobileHeQiConfirm(f) }),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.Viewer].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool ValidMobileHeQiView(ProgramSkillFrame f)
    {
        if (f.MobileHeQiView is not { } r || !MobileHeQiOriginal(f) || r.Source != MobileHeQiSource(f) || r.Hash != f.GameplayHash || r.Target != f.SelectedTargetSeats[0] || MobileHeQiDiscard(f, out var invoice)is not { } paid || r.InvocationId != invoice!.InvocationId || r.CardId != paid.CardId || r.From != paid.From || r.DiscardedKind != _coreAdvancedLifecycleOperations.GetAdvancedCard(paid.CardId).Kind || r.Viewer != (r.DiscardedKind == CardKind.Dodge ? f.OwnerSeat : r.Target) || r.Holder != (r.DiscardedKind == CardKind.Dodge ? r.Target : f.OwnerSeat) || r.Hand.Distinct().Count() != r.Hand.Count || !r.Hand.SequenceEqual(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Holder]).Select(c => c.Id)))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MobileHeQiHandViewedEvent>().Count(e => e == new MobileHeQiHandViewedEvent(f.Id, r.Source, r.InvocationId, r.CardId, r.DiscardedKind, r.Viewer, r.Holder, r.Hand.Count)) == 1;
    }

    internal bool HasMobileHeQiPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MobileHeQiView: { } r, Step: ResolutionFrameStep.AwaitingResponse } f && ValidMobileHeQiView(f) && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && p.PlayerSeat == r.Viewer && p.SourceSeat == f.OwnerSeat && p.TargetSeat == r.Holder && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, [MobileHeQiConfirm(f)]);
    [ProgramAnswer(MobileHeQiAction)]
    internal void AnswerMobileHeQiView(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileHeQiView: { } r } f || !HasMobileHeQiPrompt() || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [MobileHeQiConfirm(f)]))
            throw new InvalidOperationException("Mobile Shanxi lost its exact private viewer or hand confirmation.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHeQiView = null, Step = ResolutionFrameStep.ResolvingEffect });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileHeQiViewConfirmedEvent(f.Id, r.Viewer));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileHeQiShanxiView)]
    internal PromptChoice MobileHeQiAi(PendingDecision p) => p.Choices.Single();
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 50100)]
    internal CardSnapshot[] MobileHeQiPrivateHand(int viewer) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MobileHeQiView: { } r } f && r.Viewer == viewer && ValidMobileHeQiView(f) ? r.Hand.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray() : [];
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 50100)]
    internal void AssertMobileHeQiView(ProgramSkillFrame f)
    {
        if (f.MobileHeQiView is not null && (!ValidMobileHeQiView(f) || !HasMobileHeQiPrompt()))
            throw new InvalidOperationException("Mobile Shanxi changed its paid discard, private hand viewer or owning frame.");
    }
}
