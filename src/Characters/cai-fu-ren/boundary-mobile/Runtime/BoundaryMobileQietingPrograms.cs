using System.Text.Json.Serialization;

namespace CardGame.Core;

internal sealed class ResolveBoundaryMobileQietingDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryMobileQieting;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded)];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(1));
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryMobileQietingHandler();
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
}
internal interface IBoundaryMobileQietingHost { SkillProgramStepOutcome Begin(ProgramSkillFrame frame); }
internal sealed class BoundaryMobileQietingHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryMobileQieting;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryMobileQietingHost>().Begin(frame);
}

public sealed record BoundaryMobileQietingSelection(uint RandomBefore, uint RandomAfter)
{
    private readonly IReadOnlyList<int> _hand = Array.AsReadOnly(Array.Empty<int>()),_viewed = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Hand { get => _hand; init => _hand = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> Viewed { get => _viewed; init => _viewed = Array.AsReadOnly(value.ToArray()); }
    public int? ReplacedCardId { get; init; }
    public CardKind? ReplacedKind { get; init; }
}
public sealed partial record OrdinaryQietingReceipt
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryMobileQietingSelection? MobileSelection { get; init; }
}
public sealed record BoundaryMobileQietingViewedEvent(long FrameId, int ViewerSeat, int SourceSeat, int Count) : IGameEvent;

internal static class BoundaryMobileQietingComposition
{
    [ProgramCatalogValidation(56600)]
    private static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(trigger => trigger.Effects).Any(effect => effect.Op == SkillProgramEffectOp.ResolveBoundaryMobileQieting)) return;
        if (program.Id != "boundary:qieting-mobile" || program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.Modifiers.Count != 0 ||
            program.Triggers is not [{ Id: "mobile-turn-end", Window: SkillProgramTriggerWindow.AfterTurnEnded, Subject: SkillProgramTriggerSubject.Owner,
                TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Optional: true, UsageLimit: null, UsageScope: null, MarkerCost: null,
                Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.ResolveBoundaryMobileQieting, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": mobile Qieting requires its original actual-turn damage qualification and one three-way option.");
    }
}
