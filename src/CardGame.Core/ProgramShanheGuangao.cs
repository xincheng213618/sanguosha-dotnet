using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheGuangaoDraft
{
    private IReadOnlyList<int> _originalTargets = [];
    private IReadOnlyList<PromptChoice> _choices = [];
    public long CardUseFrameId { get; init; }
    public long ActionId { get; init; }
    public string Stage { get; init; } = "target";
    public int? AddedTarget { get; init; }
    public int? HandCountBeforeDraw { get; init; }
    public NativeDrawOutcome? DrawOutcome { get; init; }
    public IReadOnlyList<int> OriginalTargets { get => _originalTargets; init => _originalTargets = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<PromptChoice> Choices { get => _choices; init => _choices = Array.AsReadOnly(value.Select(PromptChoiceProjection.Freeze).ToArray()); }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ShanheGuangaoDraft? ShanheGuangao { get; init; }
}

internal interface IShanheGuangaoHost
{
    SkillProgramStepOutcome Resolve(ProgramSkillFrame frame);
}

internal sealed class ResolveShanheGuangaoDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheGuangao;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseTargetsFinalized),
         new RequireCardActionRelation(SkillProgramCardActionOwnerRelation.Observer, [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash])];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(5));
    public override ISkillProgramEffectHandler Handler { get; } = new ShanheGuangaoHandler();
}

internal sealed class ShanheGuangaoHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheGuangao;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheGuangaoHost>().Resolve(frame);
}
