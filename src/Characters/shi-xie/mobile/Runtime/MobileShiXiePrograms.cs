namespace CardGame.Core;

public sealed record MobileShiXieIncomingDistanceChangedEvent(long ProgramFrameId, int InstructionIndex,
    CardUseEffectSource Source, string GameplayHash, int ActualTurnNumber, int ActualTurnOwnerSeat, int Amount) : IGameEvent;
public sealed record MobileShiXieCourtesyDrawIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardUseEffectSource Source, string GameplayHash, int RecipientSeat, int ActualCount, long MovementSequence) : IGameEvent;

internal static class MobileShiXieComposition
{
    internal const string Biluan = "mobile:biluan", Lixia = "mobile:lixia", Benefit = "lixia-benefit";
    internal static bool Handles(SkillProgramEffectOp op) => op is SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions or SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance;
    [ProgramCatalogValidation(78900)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => Handles(e.Op))) return;
        bool Plain(SkillProgramEffect e, SkillProgramEffectOp op) => e.Op == op && e.Target == SkillProgramEffectTarget.Owner && e.Condition.Kind == SkillProgramConditionKind.Always && e.TargetReference is null;
        var valid = p.Id switch
        {
            Biluan => p.Triggers is [{ Id: "skip-normal-draw-for-distance", Window: SkillProgramTriggerWindow.DrawPhaseStarting, Subject: SkillProgramTriggerSubject.Owner,
                TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true, DrawPhaseMode: SkillProgramDrawPhaseMode.Replacement, UsageScope: null, UsageLimit: null, Effects: [var increase] }] &&
                Plain(increase, SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions),
            Lixia => p.Triggers is [{ Id: "one-other-ending-benefit", Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner,
                TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Optional: false, UsageScope: null, UsageLimit: null,
                Effects: [{ Op: SkillProgramEffectOp.ChooseOption, Target: SkillProgramEffectTarget.Owner, ResultBind: Benefit, Options: [{ Id: "self" }, { Id: "current" }] },
                    var decrease] }] && Plain(decrease, SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance),
            _ => false
        };
        if (!valid || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.Contributions.Count != 0 || p.CardPolicies.Count != 0 || p.BooleanStates.Count != 0)
            throw new InvalidOperationException(path + ": mobile Shi Xie requires its optional complete native Draw replacement and one mandatory mutually exclusive one-card Ending benefit before permanent incoming-distance settlement.");
    }
}
internal interface IMobileShiXieHost { SkillProgramStepOutcome Change(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class MobileShiXieDistanceDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new MobileShiXieDistanceHandler(Op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(Op == SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions ? SkillProgramTriggerWindow.DrawPhaseStarting : SkillProgramTriggerWindow.TurnEnding)];
}
internal sealed class IncreaseMobileShiXieDistanceDescriptor : MobileShiXieDistanceDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions; }
internal sealed class DecreaseMobileShiXieDistanceDescriptor : MobileShiXieDistanceDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class MobileShiXieDistanceHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host)
        => host.GetCapability<IMobileShiXieHost>().Change(f, op);
}
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class MobileShiXieHost(GameEngine engine) : IMobileShiXieHost
    { public SkillProgramStepOutcome Change(ProgramSkillFrame frame, SkillProgramEffectOp op) => engine.ChangeMobileShiXieDistance(frame, op); }

    private bool MobileShiXieEndingParent(ProgramSkillFrame f) => f.SkillId == MobileShiXieComposition.Lixia && f.TriggerId == "one-other-ending-benefit" &&
        f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && c.OwnerSeat == f.OwnerSeat && c.SourceSeat == _currentSeat && c.TargetSeat == _currentSeat && f.OwnerSeat != _currentSeat &&
        _resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId) is { } parent && parent.OwnerSeat == _currentSeat && parent.TurnNumber == _turnNumber &&
        parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } candidate && MountObserverCandidateMatches(f, candidate) && c.OccurrenceIndex == candidate.OccurrenceIndex;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 78900)]
    private bool CanOfferMobileShiXie(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c)
    {
        if (!t.Effects.Any(e => MobileShiXieComposition.Handles(e.Op))) return true;
        if (!_players[candidate.OwnerSeat].IsAlive || !_players[_currentSeat].IsAlive || c.SourceSeat != _currentSeat || c.TargetSeat != _currentSeat) return false;
        if (candidate.SkillId == MobileShiXieComposition.Biluan)
            return c.Window == SkillProgramTriggerWindow.DrawPhaseStarting && candidate.OwnerSeat == _currentSeat &&
                _players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat && GetCombatDistance(p.Seat, candidate.OwnerSeat) == 1);
        return candidate.SkillId == MobileShiXieComposition.Lixia && c.Window == SkillProgramTriggerWindow.TurnEnding && candidate.OwnerSeat != _currentSeat && !IsWithinAttackRange(_currentSeat, candidate.OwnerSeat);
    }
    private SkillProgramStepOutcome ChangeMobileShiXieDistance(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (GetPausedPrivateOfferEffect(f)?.Op != op || ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("Mobile Shi Xie cannot reissue another instruction's permanent distance change.");
        var increase = op == SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions;
        if (increase ? f.SkillId != MobileShiXieComposition.Biluan || f.TriggerId != "skip-normal-draw-for-distance" || f.InstructionIndex != 1 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || f.OwnerSeat != _currentSeat ||
            _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId) is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase } ||
            !_players[f.OwnerSeat].IsAlive || !_players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && GetCombatDistance(p.Seat, f.OwnerSeat) == 1)
            : !MobileShiXieCourtesySelected(f) || f.NativeDraw is not null || ProgramEventHistory<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Mobile Shi Xie lost its native Draw replacement or already-issued exact Ending benefit.");
        if (_winner != Winner.None) return SkillProgramStepOutcome.Continue;
        if (!increase) return BeginOperationProgramDraw(f, MobileShiXieCourtesyRecipient(f), 1, new("skill-program.mobile:lixia.Draw"));
        IssueMobileShiXieDistance(f, GetLivingFactionCount());
        return SkillProgramStepOutcome.Continue;
    }
    private void IssueMobileShiXieDistance(ProgramSkillFrame f, int amount)
    {
        if (ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Mobile Shi Xie cannot settle the original distance instruction twice.");
        AdvanceEventRulesAndQueueFact(new MobileShiXieIncomingDistanceChangedEvent(f.Id, f.InstructionIndex,
            CreateProgramTurnEffectSource(f), f.GameplayHash, _turnNumber, _turnProgression.OwnerSeat, amount));
    }
    [ProgramDistanceRule(ProgramDistanceStage.IncomingContributions, 78900)]
    private IEnumerable<RuleQueryContribution> MobileShiXieIncomingDistance(int target) =>
        ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Where(e => e.Source.OwnerSeat == target)
            .Select(e => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-shi-xie:{e.ProgramFrameId}:{e.InstructionIndex}", SkillRuleOperation.Add, e.Amount));

    private bool MobileShiXieCourtesySelected(ProgramSkillFrame f)
    {
        if (!MobileShiXieEndingParent(f) || f.ChoiceBindings.SingleOrDefault(b => b.Name == MobileShiXieComposition.Benefit) is not { ChooserSeat: var chooser, OptionId: "self" or "current" } choice || chooser != f.OwnerSeat) return false;
        return f.InstructionIndex == 2;
    }
    private int MobileShiXieCourtesyRecipient(ProgramSkillFrame f) =>
        f.ChoiceBindings.Single(b => b.Name == MobileShiXieComposition.Benefit).OptionId == "self" ? f.OwnerSeat : _currentSeat;
    [ProgramNativeDrawReturn(SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance)]
    private void ReturnMobileShiXieCourtesyDraw(ProgramSkillFrame supplied, NativeDrawOutcome drawn)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (!MobileShiXieCourtesySelected(f) || f.NativeDraw is not null || drawn.RecipientSeat != MobileShiXieCourtesyRecipient(f) ||
            drawn.RequestedCount != 1 || drawn.ActualDrawIds.Count > 1 || drawn.SequenceBefore > drawn.SequenceAfter || drawn.SequenceAfter > _movementSequence ||
            ProgramEventHistory<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Lixia lost the original chosen one-card native draw return.");
        AdvanceEventRulesAndQueueFact(new MobileShiXieCourtesyDrawIssuedEvent(f.Id, f.InstructionIndex,
            CreateProgramTurnEffectSource(f), f.GameplayHash, drawn.RecipientSeat, drawn.ActualDrawIds.Count, drawn.SequenceAfter));
        AdvanceRuntimeProgram(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 78900)]
    private bool SettleMobileShiXieCourtesy(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame f || f.Id != id || f.NativeDraw is not null ||
            !MobileShiXieCourtesySelected(f) || !MobileShiXieCourtesyIssued(f) ||
            ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Any(e => e.ProgramFrameId == f.Id)) return false;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) ||
            TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(f.Id) || TryBeginAdvancedSkillsChanged(f.Id)) return true;
        if (_winner != Winner.None) return false;
        IssueMobileShiXieDistance(GetActiveProgramFrame(id), -1);
        AdvanceRuntimeProgram(id); return true;
    }
    private bool MobileShiXieCourtesyIssued(ProgramSkillFrame f)
    {
        if (!MobileShiXieCourtesySelected(f)) return false;
        var recipient = MobileShiXieCourtesyRecipient(f);
        return f.NativeDraw is { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, RequestedCount: 1, Operation: SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance, InstructionIndex: 2 } native &&
                native.Recipients.SequenceEqual([recipient]) ||
            ProgramEventHistory<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == 2 &&
                e.Source == CreateProgramTurnEffectSource(f) && e.GameplayHash == f.GameplayHash && e.RecipientSeat == recipient && e.ActualCount is >= 0 and <= 1 && e.MovementSequence <= _movementSequence);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 78900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 78900)]
    private bool KeepMobileShiXieIssuedCourtesy(ProgramSkillFrame f) => MobileShiXieCourtesyIssued(f);
}
