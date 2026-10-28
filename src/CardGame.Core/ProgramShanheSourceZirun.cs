using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheSourceZirunReceipt(long PreparationWindowId, int ActualTurn)
{
    private readonly IReadOnlyList<int> _recipients = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Recipients
    { get => _recipients; init => _recipients = Array.AsReadOnly(value.ToArray()); }
    public int Index { get; init; }
    public int RequestedCount { get; init; }
    public bool ReturningChildren { get; init; }
}
public sealed record ShanheSourceZirunStartedEvent(long FrameId, long PreparationWindowId,
    int OwnerSeat, int ActualTurn, int RecipientCount) : IGameEvent;
public sealed record ShanheSourceZirunRequestedEvent(long FrameId, int Index, int RecipientSeat,
    int EquipmentCount, int RequestedCount) : IGameEvent;
public sealed record ShanheSourceZirunReturnedEvent(long FrameId, int Index, int RecipientSeat,
    int RequestedCount, int ActualCount, long SequenceBefore, long SequenceAfter, bool Replaced) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceZirunReceipt? ShanheSourceZirun { get; init; }
}
public interface IShanheSourceZirunHost
{ SkillProgramStepOutcome Begin(ProgramSkillFrame frame); }
internal sealed class ResolveShanheSourceZirunDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceZirun;
    public override ISkillProgramEffectHandler Handler { get; } = new ShanheSourceZirunHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, c) => c.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.Owner, 1, new(SkillProgramConditionKind.Always, 0, []))));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class ShanheSourceZirunHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceZirun;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSourceZirunHost>().Begin(frame);
}
internal static class ShanheSourceZirunComposition
{
    internal const string SkillId = "shanhe:source-zirun-1172";
    [ProgramCatalogValidation(24922)]
    internal static void Validate(string path, SkillProgram program)
    {
        foreach (var trigger in program.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceZirun)))
            if (program.Id != SkillId || trigger.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow ||
                trigger.Subject != SkillProgramTriggerSubject.Owner || trigger.Optional ||
                trigger.Effects is not [{ Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] ||
                trigger.Condition.Kind != SkillProgramTriggerConditionKind.Always)
                throw new InvalidOperationException(path + ": source1172 requires one compulsory owner Preparation draw cursor.");
    }
}
