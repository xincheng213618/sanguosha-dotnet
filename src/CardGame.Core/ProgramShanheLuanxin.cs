namespace CardGame.Core;

public sealed record ShanheLuanxinNextDrawIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardConversionSource Source, string GameplayHash, long MovementBatchId, int TargetSeat) : IGameEvent;
public sealed record ShanheLuanxinNextDrawAssignedEvent(long ProgramFrameId, int InstructionIndex,
    ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;

internal static class ShanheLuanxinProgramContract
{
    [ProgramCatalogValidation(81600)]
    internal static void Validate(string path, SkillProgram program)
    {
        var operation = SkillProgramEffectOp.QueueShanheLuanxinNextDraw;
        var triggered = program.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op == operation)).ToArray();
        if (triggered.Length == 0 && !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == operation))) return;
        if (program.Id != "shanhe:companion-luanxin" || program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == operation)) ||
            triggered is not [{ Id: "other-hand-gain", Window: SkillProgramTriggerWindow.CardsGained,
                Subject: SkillProgramTriggerSubject.Owner, MovementOccurrence: SkillProgramMovementOccurrence.PerBatch,
                Optional: false, Effects: [{ Op: SkillProgramEffectOp.QueueShanheLuanxinNextDraw }] } trigger] ||
            !trigger.DestinationZones.SequenceEqual([CardZoneKind.Hand]))
            throw new InvalidOperationException(path + ": Luanxin's future Draw benefit requires its mandatory owner hand-gain batch trigger.");
    }
}

internal interface IShanheLuanxinProgramHost { void QueueNextDraw(ProgramSkillFrame frame); }
internal sealed class QueueShanheLuanxinNextDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.QueueShanheLuanxinNextDraw;
    public override ISkillProgramEffectHandler Handler { get; } = new QueueShanheLuanxinNextDrawHandler();
    public override ProgramFactRequirements FactRequirements => new(Triggered:
        ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier,
        static (_, context) => context.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 1, reader.Condition());
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}
internal sealed class QueueShanheLuanxinNextDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.QueueShanheLuanxinNextDraw;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IShanheLuanxinProgramHost>().QueueNextDraw(frame); return SkillProgramStepOutcome.Continue; }
}

public sealed partial class GameEngine
{
}
