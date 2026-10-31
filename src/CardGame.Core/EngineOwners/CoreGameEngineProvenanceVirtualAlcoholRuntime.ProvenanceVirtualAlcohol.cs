#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProvenanceVirtualAlcoholRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 30, SkillProgramEffectOp.UseVirtualAlcohol)]
    internal bool CanStartProvenanceAlcohol(CharacterState owner) => !owner.IsFaceDown && _coreVirtualBasicOffersOperations.VirtualBasicOptions2(owner.Seat).Contains((CardKind.Alcohol, owner.Seat));
    internal SkillProgramStepOutcome BeginProvenanceVirtualAlcohol(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ProvenanceAlcohol is not null || f.VirtualBasicDraft is not null)
            throw new InvalidOperationException("A virtual Alcohol instruction cannot pay twice.");
        if (!CanStartProvenanceAlcohol(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "当前不能翻背使用酒，未支付成本。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceAlcohol = new(f.InstructionIndex, ProgramProvenanceAlcoholStage.FaceCost) });
        _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, f.OwnerSeat, true);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 640)]
    internal bool ResumeProvenanceAlcohol(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.ProvenanceAlcohol is not { } receipt)
            return false;
        AssertProvenanceAlcohol(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        if (receipt.Stage == ProgramProvenanceAlcoholStage.WineUse)
        {
            if (f.VirtualBasicDraft is not null)
                throw new InvalidOperationException("The virtual Wine child returned without consuming its exact typed return.");
            var completed = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Any(e => e.ResolutionId == receipt.ChildFrameId && e.CardId == 0 && e.CardKind == CardKind.Alcohol);
            if (!completed)
                throw new InvalidOperationException("A virtual Wine return has no actual zero-entity completion.");
            var finished = f with
            {
                ProvenanceAlcohol = null
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(finished);
            _coreSkillProgramsOperations.FinishProgramSkill(finished, completed: _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive);
            return true;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!_coreVirtualBasicOffersOperations.VirtualBasicOptions2(actor.Seat).Contains((CardKind.Alcohol, actor.Seat)))
        {
            var cancelled = f with
            {
                ProvenanceAlcohol = null
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(cancelled);
            _coreSkillProgramsOperations.FinishProgramSkill(cancelled, completed: false);
            return true;
        }

        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceAlcohol = receipt with { Stage = ProgramProvenanceAlcoholStage.WineUse, ChildFrameId = useId }, VirtualBasicDraft = new(f.InstructionIndex, useId) });
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Alcohol, [actor.Seat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Alcohol, [actor.Seat], PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(f.Id, f.InstructionIndex), FirstOwnPlayUseDistanceUnlimited = _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(actor) });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Alcohol, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, [actor.Seat]));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor.Seat, CardKind.Alcohol);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Alcohol);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, new(0, SimpleCardUseEffect.Alcohol));
        return true;
    }
    internal void AssertProvenanceAlcohol(ProgramSkillFrame f)
    {
        if (f.ProvenanceAlcohol is not { } receipt)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions;
    }
}
