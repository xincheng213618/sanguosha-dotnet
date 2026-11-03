using System.Collections.Frozen;

namespace CardGame.Core;

/// <summary>A definition-only relation between a target, its payment and the native consumer.</summary>
internal sealed record ProgramTargetPaymentPlan(
    int SelectionIndex, int PaymentIndex, int RedirectIndex, SkillProgramEffect Payment);

internal sealed class ProgramTargetPaymentPlans
{
    private static readonly ProgramTargetPaymentPlans Empty = new([]);
    private readonly FrozenDictionary<int, ProgramTargetPaymentPlan> _selections;
    private readonly FrozenDictionary<int, ProgramTargetPaymentPlan> _payments;
    private readonly FrozenDictionary<int, ProgramTargetPaymentPlan> _redirects;

    private ProgramTargetPaymentPlans(IReadOnlyList<ProgramTargetPaymentPlan> plans)
    {
        _selections = plans.ToFrozenDictionary(plan => plan.SelectionIndex);
        _payments = plans.ToFrozenDictionary(plan => plan.PaymentIndex);
        _redirects = plans.ToFrozenDictionary(plan => plan.RedirectIndex);
    }

    // All indices are zero-based instruction identities. A paused original frame
    // has already committed its cursor, so callers pass frame.InstructionIndex - 1.
    internal ProgramTargetPaymentPlan? ForSelection(int instructionIndex) => _selections.GetValueOrDefault(instructionIndex);
    internal ProgramTargetPaymentPlan? ForPayment(int instructionIndex) => _payments.GetValueOrDefault(instructionIndex);
    internal ProgramTargetPaymentPlan? ForRedirect(int instructionIndex) => _redirects.GetValueOrDefault(instructionIndex);

    internal static void Validate(string path, IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerWindow? window)
    {
        if (!effects.Any(effect => effect.SlashRedirectPostCostRange is not null)) return;
        if (window != SkillProgramTriggerWindow.SlashTargetRedirecting)
            throw Error(path, "post-cost Slash redirect requires its native redirecting window");
        _ = Compile(effects, path);
    }

    internal static ProgramTargetPaymentPlans Compile(IReadOnlyList<SkillProgramEffect> effects,
        string path = "instruction binding")
    {
        if (!effects.Any(effect => effect.SlashRedirectPostCostRange is not null)) return Empty;
        var plans = new List<ProgramTargetPaymentPlan>();
        var selections = new HashSet<int>();
        var redirects = new HashSet<int>();
        for (var paymentIndex = 0; paymentIndex < effects.Count; paymentIndex++)
        {
            var payment = effects[paymentIndex];
            if (payment.SlashRedirectPostCostRange is null) continue;
            if (payment is not
                { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Target: SkillProgramEffectTarget.Owner,
                  Amount: 1, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true,
                  SlashRedirectPostCostRange: true, ChooserRef.Kind: ProgramParticipantRef.Owner,
                  CardOwnerRef.Kind: ProgramParticipantRef.Owner, Condition.Kind: SkillProgramConditionKind.Always } ||
                payment.CountExpression is not null || payment.ExceptBind is not null || payment.SuitFrom is not null ||
                payment.Zones.Count != 2 ||
                !payment.Zones.Contains(CardZoneKind.Hand) || !payment.Zones.Contains(CardZoneKind.Equipment) ||
                payment.SkipIfNoCards || payment.AllowDecline || payment.AllowFewerWhenInsufficient)
                throw Error(path, "post-cost Slash redirect requires one unconditional awaited own HE discard");

            // Eligibility may query this payment before the binding begins.
            // Only the initial pair can rely on the current material domain.
            var selectionIndex = paymentIndex - 1;
            if (selectionIndex != 0 || effects[selectionIndex] is not
                { Op: SkillProgramEffectOp.SelectTarget, Target: SkillProgramEffectTarget.Owner,
                  TargetKind: SkillProgramTargetKind.SlashRedirectable,
                  Condition.Kind: SkillProgramConditionKind.Always, SkipIfNoTarget: false,
                  Zones.Count: 0, TargetSetExpression: null, ActorReference: null, Marker: null })
                throw Error(path, "post-cost Slash redirect requires an unambiguous preceding Slash target selection");

            var redirectIndex = paymentIndex + 1;
            while (redirectIndex < effects.Count && effects[redirectIndex].Op != SkillProgramEffectOp.RedirectCurrentAttack)
            {
                if (ChangesSelection(effects[redirectIndex]) ||
                    effects[redirectIndex].SlashRedirectPostCostRange is not null)
                    break;
                redirectIndex++;
            }
            if (redirectIndex >= effects.Count || effects[redirectIndex] is not
                { Op: SkillProgramEffectOp.RedirectCurrentAttack, Target: SkillProgramEffectTarget.SelectedTarget,
                  Condition.Kind: SkillProgramConditionKind.Always })
                throw Error(path, "post-cost Slash redirect requires its original selected-target consumer");
            if (!selections.Add(selectionIndex) || !redirects.Add(redirectIndex))
                throw Error(path, "a Slash target selection and redirect cannot share different payments");
            plans.Add(new(selectionIndex, paymentIndex, redirectIndex, payment));
        }
        return new(plans);
    }

    private static bool ChangesSelection(SkillProgramEffect effect)
    {
        var descriptor = ProgramOperationCatalog.Default.Resolve(effect.Op);
        return descriptor.RequiredCapabilities.HasFlag(ProgramContextCapability.PhaseInsertion) ||
            descriptor.Resources(effect).Any(resource => resource is SelectSingleTarget or ReplaceSingleTarget or
                SelectTargetSet or ReplaceSingleTargetWithSet or NarrowTargetSetToSingle or
                ConsumeTargetSet or ReleaseTargetSet);
    }

    private static InvalidOperationException Error(string path, string message) =>
        new($"Invalid skill program at {path}: {message}.");
}
