namespace CardGame.Core;

internal interface IMobileBuLianShiAnxuHost { void Prepare(ProgramSkillFrame frame); }
internal sealed class PrepareMobileBuLianShiAnxuDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileBuLianShiAnxu;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileBuLianShiAnxuHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadTargetSet(2, 2), new ReadCardSet("mobile-anxu-gift"), new CreateChoiceResult("mobile-anxu-equality", ["equal", "unequal"])];
}
internal sealed class MobileBuLianShiAnxuHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileBuLianShiAnxu;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host)
    { host.GetCapability<IMobileBuLianShiAnxuHost>().Prepare(frame); return SkillProgramStepOutcome.Continue; }
}
public sealed record MobileBuLianShiAnxuComparedEvent(long FrameId, int RecipientSeat, int DonorSeat, int RecipientHand, int DonorHand, bool Equal) : IGameEvent;
public sealed record MobileBuLianShiAnxuDeliveredEvent(long FrameId, string SkillInstanceId, string GameplayHash, int RecipientSeat, int DonorSeat, int CardId, long BatchId, long MovementSequence) : IGameEvent;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class MobileBuLianShiAnxuHost(GameEngine engine) : IMobileBuLianShiAnxuHost
    { public void Prepare(ProgramSkillFrame frame) => engine.PrepareMobileBuLianShiAnxu(frame); }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 72400)]
    private void RecordMobileBuLianShiAnxuDelivery(CardMovedEvent fact)
    {
        if (fact.Reason.Value != "skill-program.mobile:anxu.SelectAndMoveOwnedCard" ||
            _resolutionStack.LastOrDefault() is not ProgramSkillFrame { SkillId: "mobile:anxu", InstructionIndex: 2, SelectedTargetSeats: [var recipient, var donor] } f ||
            fact.To != CardLocation.Hand(recipient) || fact.From != CardLocation.Hand(donor) && fact.From != CardLocation.Processing ||
            _cardMovements.LastOrDefault() is not { } movement || movement.CardId != fact.CardId || movement.From != fact.From || movement.To != fact.To || movement.Reason != fact.Reason ||
            !_activeCardMovementBatchIds.TryPeek(out var batch)) return;
        AdvanceEventRulesAndQueueFact(new MobileBuLianShiAnxuDeliveredEvent(f.Id, f.SkillInstanceId, f.GameplayHash, recipient, donor, movement.CardId, batch, movement.Sequence));
    }
    private bool MobileBuLianShiAnxuPaid(ProgramSkillFrame f) => f.SkillId == "mobile:anxu" && f.TriggerId is null && f.ActivationId == "donor-gives-to-lower-hand" &&
        f.InstructionIndex >= 2 && f.SelectedTargetSeats is [var recipient, var donor] &&
        f.CardSetBindings.SingleOrDefault(b => b.Name == "mobile-anxu-gift") is { CardIds: [var card] } &&
        ProgramEventHistory<MobileBuLianShiAnxuDeliveredEvent>().Count(e => e.FrameId == f.Id && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash &&
            e.RecipientSeat == recipient && e.DonorSeat == donor && e.CardId == card && e.BatchId > f.Id &&
            _cardMovements.Any(m => m.Sequence == e.MovementSequence && m.CardId == card && (m.From == CardLocation.Hand(donor) || m.From == CardLocation.Processing) &&
                m.To == CardLocation.Hand(recipient) && m.Reason.Value == "skill-program.mobile:anxu.SelectAndMoveOwnedCard")) == 1;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 72400)]
    private bool KeepMobileBuLianShiAnxuPaid(ProgramSkillFrame f) => MobileBuLianShiAnxuPaid(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 72400)]
    private bool ReturnMobileBuLianShiAnxuPaid(ProgramSkillFrame f)
    {
        if (f.InstructionIndex != 2 || !MobileBuLianShiAnxuPaid(f) || f.SelectedTargetSeats is not [_, var donor] ||
            f.PendingMovementContinuation != new ProgramMovementContinuation(donor, 0, null)) return false;
        ReplaceRuntimeTop(f with { PendingMovementContinuation = null }); AdvanceRuntimeProgram(f.Id); return true;
    }
    private bool IsMobileBuLianShiAnxuEqualityResult(ProgramSkillFrame f, ProgramChoiceResultBinding binding) =>
        f.InstructionIndex >= 3 && binding.Name == "mobile-anxu-equality" && binding.ChooserSeat == f.OwnerSeat &&
        MobileBuLianShiAnxuPaid(f) && f.SelectedTargetSeats is [var recipient, var donor] &&
        ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat &&
            e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1 &&
        ProgramEventHistory<MobileBuLianShiAnxuComparedEvent>().Where(e => e.FrameId == f.Id).ToArray() is [var compared] &&
        compared.RecipientSeat == recipient && compared.DonorSeat == donor && compared.RecipientHand >= 0 && compared.DonorHand >= 0 &&
        (!compared.Equal || compared.RecipientHand == compared.DonorHand) && binding.OptionId == (compared.Equal ? "equal" : "unequal");
    private void PrepareMobileBuLianShiAnxu(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "mobile:anxu" || f.TriggerId is not null || f.InstructionIndex != 3 || f.SelectedTargetSeats is not [var recipient, var donor] ||
            recipient == donor || recipient == f.OwnerSeat || donor == f.OwnerSeat ||
            GetProgramCardSet(f, "mobile-anxu-gift") is not { CardIds.Count: 1, SourceLocations: [var boundLocation] } || boundLocation != CardLocation.Hand(recipient) ||
            !MobileBuLianShiAnxuPaid(f) || f.ChoiceBindings.Any(b => b.Name == "mobile-anxu-equality"))
            throw new InvalidOperationException("Mobile Anxu compares only its original unequal pair after the actual donor's native gift children return.");
        var first = GetHand(_players[recipient]).Count; var second = GetHand(_players[donor]).Count;
        var equal = _players[recipient].IsAlive && _players[donor].IsAlive && first == second;
        ReplaceRuntimeTop(f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("mobile-anxu-equality", equal ? "equal" : "unequal", f.OwnerSeat)).ToArray()) });
        AdvanceEventRulesAndQueueFact(new MobileBuLianShiAnxuComparedEvent(f.Id, recipient, donor, first, second, equal));
    }
}
