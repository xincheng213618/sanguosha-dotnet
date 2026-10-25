namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class BoundaryXuChuCapability(GameEngine engine) : IBoundaryXuChuHost
    { public void SubstituteBoundaryXuChuDraw(ProgramSkillFrame f) => engine.SubstituteBoundaryXuChuDraw(f); }

    private ProgramLifecycleTriggerWindowFrame BoundaryXuChuDrawParent(ProgramSkillFrame f)
    {
        var index = _resolutionStack.FindIndex(item => item.Id == f.Id);
        if (f.SkillId != BoundaryXuChuMobile.Skill || index < 1 || f.OwnerSeat != _currentSeat || _phase != TurnPhase.Draw ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context ||
            _resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId ||
            parent.OwnerSeat != f.OwnerSeat || parent.Window != context.Window || parent.NormalDrawReplaced)
            throw new InvalidOperationException("Mobile Luoyi lost its original actual owner Draw window.");
        return parent;
    }
    private void SubstituteBoundaryXuChuDraw(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var parent = BoundaryXuChuDrawParent(f);
        var gained = GetProgramCardSet(f, BoundaryXuChuMobile.Gained);
        if (parent.ActualPhaseSubstitution is not null || f.InstructionIndex != 4 || gained.CardIds.Count == 0 ||
            gained.CardIds.Any(id => _cardZones.GetLocation(id) != CardLocation.Processing) ||
            f.ChoiceBindings.SingleOrDefault(b => b.Name == BoundaryXuChuMobile.Choice)?.OptionId != BoundaryXuChuMobile.Skip)
            throw new InvalidOperationException("Mobile Luoyi can substitute Draw only once after choosing its actual nonempty public gain.");
        ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, f.Id, f.OwnerSeat,
            f.SkillId, f.SkillInstanceId, _turnNumber, SkillProgramTurnPhase.Draw) });
        AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(f.Id, f.SkillId, GetProgramBindingId(f),
            f.SkillInstanceId, f.OwnerSeat, _turnNumber, parent.Window, SkillProgramTurnPhase.Draw));
    }

    private bool BoundaryXuChuCommittedDraw(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryXuChuMobile.Skill || f.InstructionIndex is < 4 or > 8 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context ||
            f.ChoiceBindings.SingleOrDefault(b => b.Name == BoundaryXuChuMobile.Choice)?.OptionId != BoundaryXuChuMobile.Skip) return false;
        return _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(p => p.Id == context.ParentFrameId &&
            p.OwnerSeat == f.OwnerSeat && p.ActualPhaseSubstitution is { } receipt && receipt.ParentFrameId == p.Id &&
            receipt.ProducerFrameId == f.Id && receipt.SkillId == f.SkillId && receipt.SkillInstanceId == f.SkillInstanceId &&
            receipt.OwnerSeat == f.OwnerSeat && receipt.ActualTurnNumber == _turnNumber && receipt.SkippedPhase == SkillProgramTurnPhase.Draw);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59900)]
    private bool ContinueBoundaryXuChuCommittedDraw(ProgramSkillFrame f) => BoundaryXuChuCommittedDraw(f);

    [ProgramContinuation(ProgramContinuationKind.Movement, 59900)]
    private bool ReturnBoundaryXuChuCommittedMovement(ProgramSkillFrame f)
    {
        if (!BoundaryXuChuCommittedDraw(f) || f.InstructionIndex is not (5 or 6) ||
            HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId)) return false;
        if (f.PendingMovementContinuation is not { SubjectSeat: var owner, BeforeCount: 0, CoverageResultBind: null } || owner != f.OwnerSeat)
            throw new InvalidOperationException("Mobile Luoyi lost its original paid gain/cleanup movement return.");
        ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        AdvanceRuntimeProgram(f.Id);
        return true;
    }

    private bool TrySelectBoundaryXuChuAiOption(PendingDecision decision, ProgramSkillFrame f, out PromptChoice choice)
    {
        choice = null!;
        if (f.SkillId != BoundaryXuChuMobile.Skill || f.InstructionIndex != 3 || decision.PlayerSeat != f.OwnerSeat) return false;
        var normal = decision.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == BoundaryXuChuMobile.Normal);
        var skip = decision.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == BoundaryXuChuMobile.Skip);
        if (skip is null) { choice = normal; return true; }
        var parent = BoundaryXuChuDrawParent(f);
        var gained = GetProgramCardSet(f, BoundaryXuChuMobile.Gained).CardIds.Select(GetAdvancedCard).ToArray();
        var owner = _players[f.OwnerSeat];
        var normalCount = Math.Max(0, checked((parent.FrozenBaseDrawCount ?? GetTurnDrawCount(owner)) + parent.NormalDrawAdjustment));
        var attack = gained.Any(c => IsSlashCard(c.Kind) || c.Kind == CardKind.Duel);
        var opportunity = attack ? 11d : GetHand(owner).Count > 0 ? 6d : 0d;
        choice = gained.Length * 8d + opportunity > normalCount * 8d ? skip : normal;
        return true;
    }
}
