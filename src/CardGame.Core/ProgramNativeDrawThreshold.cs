using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record NativeDrawThresholdEntitlement(CardConversionSource Source, string GameplayHash,
    int Threshold, int RemainderBefore);
public sealed record NativeDrawThresholdProgress(CardConversionSource Source, string GameplayHash,
    int Threshold, int RemainderBefore, int ActualCount, int Crossings, int RemainderAfter);
public sealed record NativeDrawThresholdSourceCapturedEvent(long InvocationId, int OwnerSeat,
    long SequenceBefore, NativeDrawThresholdEntitlement Entitlement) : IGameEvent;
public sealed record NativeDrawThresholdProgressedEvent(long InvocationId, int OwnerSeat,
    CardConversionSource Source, string GameplayHash, int Threshold, int RemainderBefore,
    int ActualCount, int Crossings, int RemainderAfter) : IGameEvent;
public sealed record NativeDrawThresholdTargetChosenEvent(long ProgramFrameId, long InvocationId,
    int OccurrenceIndex, CardConversionSource Source, int TargetSeat) : IGameEvent;

public sealed partial record NativeDrawInvocationProof
{
    private readonly IReadOnlyList<NativeDrawThresholdProgress>? _thresholdProgress;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<NativeDrawThresholdProgress>? ThresholdProgress
    { get => _thresholdProgress; init => _thresholdProgress = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}

internal interface INativeDrawThresholdHost
{
    SkillProgramStepOutcome DamageRandomEnemyAtDrawThreshold(ProgramSkillFrame frame, int threshold, int amount);
}

internal sealed class DamageRandomEnemyAtDrawThresholdDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold;
    public override ISkillProgramEffectHandler Handler { get; } = new DamageRandomEnemyAtDrawThresholdHandler();
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeDrawInvocation);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage,
        static (effect, context) => context.Damage(new(SkillProgramEffectOp.Damage,
            SkillProgramEffectTarget.SelectedTarget, effect.Amount, effect.Condition)));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "threshold", "amount", "condition");
        var threshold = reader.RequiredInt("threshold");
        if (threshold <= 0) throw new InvalidOperationException(reader.Path + ".threshold requires a positive material count.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader),
            DrawProgramOperationDescriptor.Amount(reader, 20), reader.Condition(), minimumValue: threshold);
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsGained)];
}

internal sealed class DamageRandomEnemyAtDrawThresholdHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<INativeDrawThresholdHost>().DamageRandomEnemyAtDrawThreshold(frame, effect.MinimumValue, effect.Amount);
}
