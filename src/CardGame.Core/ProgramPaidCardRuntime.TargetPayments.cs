namespace CardGame.Core;

internal interface IProgramTargetPaymentQueries : IProgramSelectionQuery
{
    bool HasSlashRedirectPayment(ProgramSkillFrame frame, int targetSeat);
    bool HasSlashRedirectPayment(int ownerSeat, string skillId, string skillInstanceId,
        ProgramSkillWindowContext? context, ProgramTargetPaymentPlan plan, int targetSeat);
}

internal sealed partial class ProgramPaidCardRuntime
{
    private readonly record struct TargetPayment(ProgramSkillWindowContext? Context, int? TargetSeat);

    public bool HasSlashRedirectPayment(ProgramSkillFrame frame, int targetSeat)
    {
        var plans = ProgramInstructionResolver.Default.Resolve(frame,
            _facts.Skill(frame.SkillId).Program!).Features.TargetPayments;
        var index = frame.InstructionIndex - 1;
        var plan = plans.ForSelection(index) ?? plans.ForPayment(index);
        return plan is not null && HasSlashRedirectPayment(frame.OwnerSeat, frame.SkillId,
            frame.SkillInstanceId, frame.WindowContext, plan, targetSeat);
    }

    public bool HasSlashRedirectPayment(int ownerSeat, string skillId, string skillInstanceId,
        ProgramSkillWindowContext? context, ProgramTargetPaymentPlan plan, int targetSeat)
    {
        var payment = plan.Payment;
        return OwnedCardPaymentCandidates(ownerSeat, skillId, skillInstanceId, ownerSeat, ownerSeat,
            payment.Zones, OwnedCardMoveIntent.Discard, payment.CardCategories, payment.CardKinds,
            payment.Suits, targetPayment: new(context, targetSeat)).Any();
    }

    private TargetPayment? TargetPaymentFor(ProgramSkillFrame frame) =>
        ProgramInstructionResolver.Default.Resolve(frame, _facts.Skill(frame.SkillId).Program!).Features
            .TargetPayments.ForPayment(frame.InstructionIndex - 1) is null
            ? null : new(frame.WindowContext, frame.SelectedTargetSeats is [var seat] ? seat : null);

    private bool AllowsTargetPayment(ProgramSkillFrame frame, CardLocation source, Card card) =>
        TargetPaymentFor(frame) is not { } payment || AllowsTargetPayment(frame.OwnerSeat, payment, source, card);

    private bool AllowsTargetPayment(int ownerSeat, TargetPayment payment, CardLocation source, Card card) =>
        payment.TargetSeat is { } targetSeat && source.OwnerSeat == ownerSeat &&
        source.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
        _facts.Location(card.Id) == source &&
        !_facts.SelfDiscardForbidden(ownerSeat, card, source, OwnedCardMoveIntent.Discard) &&
        _slashRedirectTargets.IsEligible(ownerSeat, targetSeat, payment.Context,
            source.Zone == CardZoneKind.Equipment ? card.Id : null);
}
