#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuChuBoundaryMobileRuntime
{
    void IBoundaryXuChuHost.SubstituteBoundaryXuChuDraw(ProgramSkillFrame f) => SubstituteBoundaryXuChuDraw(f);
    internal ProgramLifecycleTriggerWindowFrame BoundaryXuChuDrawParent(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        if (f.SkillId != BoundaryXuChuMobile.Skill || index < 1 || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Draw || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != f.OwnerSeat || parent.Window != context.Window || parent.NormalDrawReplaced)
            throw new InvalidOperationException("Mobile Luoyi lost its original actual owner Draw window.");
        return parent;
    }

    internal void SubstituteBoundaryXuChuDraw(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var parent = BoundaryXuChuDrawParent(f);
        var gained = _coreProgramLifecycleOperations.GetProgramCardSet(f, BoundaryXuChuMobile.Gained);
        if (parent.ActualPhaseSubstitution is not null || f.InstructionIndex != 4 || gained.CardIds.Count == 0 || gained.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing) || f.ChoiceBindings.SingleOrDefault(b => b.Name == BoundaryXuChuMobile.Choice)?.OptionId != BoundaryXuChuMobile.Skip)
            throw new InvalidOperationException("Mobile Luoyi can substitute Draw only once after choosing its actual nonempty public gain.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, SkillProgramTurnPhase.Draw) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, parent.Window, SkillProgramTurnPhase.Draw));
    }

    internal bool BoundaryXuChuCommittedDraw(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryXuChuMobile.Skill || f.InstructionIndex is < 4 or > 8 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || f.ChoiceBindings.SingleOrDefault(b => b.Name == BoundaryXuChuMobile.Choice)?.OptionId != BoundaryXuChuMobile.Skip)
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(p => p.Id == context.ParentFrameId && p.OwnerSeat == f.OwnerSeat && p.ActualPhaseSubstitution is { } receipt && receipt.ParentFrameId == p.Id && receipt.ProducerFrameId == f.Id && receipt.SkillId == f.SkillId && receipt.SkillInstanceId == f.SkillInstanceId && receipt.OwnerSeat == f.OwnerSeat && receipt.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && receipt.SkippedPhase == SkillProgramTurnPhase.Draw);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59900)]
    internal bool ContinueBoundaryXuChuCommittedDraw(ProgramSkillFrame f) => BoundaryXuChuCommittedDraw(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 59900)]
    internal bool ReturnBoundaryXuChuCommittedMovement(ProgramSkillFrame f)
    {
        if (!BoundaryXuChuCommittedDraw(f) || f.InstructionIndex is not (5 or 6) || _coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return false;
        if (f.PendingMovementContinuation is not { SubjectSeat: var owner, BeforeCount: 0, CoverageResultBind: null } || owner != f.OwnerSeat)
            throw new InvalidOperationException("Mobile Luoyi lost its original paid gain/cleanup movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool TrySelectBoundaryXuChuAiOption(PendingDecision decision, ProgramSkillFrame f, out PromptChoice choice)
    {
        choice = null !;
        if (f.SkillId != BoundaryXuChuMobile.Skill || f.InstructionIndex != 3 || decision.PlayerSeat != f.OwnerSeat)
            return false;
        var normal = decision.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == BoundaryXuChuMobile.Normal);
        var skip = decision.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == BoundaryXuChuMobile.Skip);
        if (skip is null)
        {
            choice = normal;
            return true;
        }

        var parent = BoundaryXuChuDrawParent(f);
        var gained = _coreProgramLifecycleOperations.GetProgramCardSet(f, BoundaryXuChuMobile.Gained).CardIds.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var normalCount = Math.Max(0, checked((parent.FrozenBaseDrawCount ?? _coreActualTurnsOperations.GetTurnDrawCount(owner)) + parent.NormalDrawAdjustment));
        var attack = gained.Any(c => CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) || c.Kind == CardKind.Duel);
        var opportunity = attack ? 11d : _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 ? 6d : 0d;
        choice = gained.Length * 8d + opportunity > normalCount * 8d ? skip : normal;
        return true;
    }
}
