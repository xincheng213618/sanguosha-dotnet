using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ShanheYingxiangStage { Cost, CostChildren, Option, Recipient, EffectChildren, Draw, DrawWaiting, DrawChildren, Complete }
public sealed record ShanheYingxiangReceipt(CardConversionSource Source, string GameplayHash, long ParentWindowId,
    ShanheYingxiangStage Stage, int UserSeat = -1, long ActionId = 0)
{
    private IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
    public int OptionsRemaining { get; init; }
    public int OptionsChosen { get; init; }
    public int DrawIndex { get; init; }
    public NativeDrawOutcome? DrawOutcome { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheYingxiangReceipt? ShanheYingxiang { get; init; }
}

// A native movement sequence identifies the physical card without publishing a hidden hand identity.
public sealed record ShanheYingxiangTaggedEvent(int OwnerSeat, long MovementSequence) : IGameEvent;
public sealed record ShanheYingxiangQingjueStartedEvent(long FrameId, int OwnerSeat, int RoundNumber, long BatchId) : IGameEvent;
public sealed record ShanheYingxiangUsePaidEvent(long FrameId, int OwnerSeat, int UserSeat, long ActionId) : IGameEvent;

internal interface IShanheYingxiangHost
{ SkillProgramStepOutcome ResolveYingxiang(ProgramSkillFrame frame, bool used); }

internal abstract class ShanheYingxiangDescriptor : ProgramOperationDescriptorBase
{
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeTrueUseProcessingEntry);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(2));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(Op == SkillProgramEffectOp.ResolveShanheYingxiangUse
            ? SkillProgramTriggerWindow.CardUseCompleted : SkillProgramTriggerWindow.CardsMoved)];
    public override ISkillProgramEffectHandler Handler => new ShanheYingxiangHandler(Op);
}
internal sealed class ResolveShanheYingxiangMovementDescriptor : ShanheYingxiangDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheYingxiangMovement; }
internal sealed class ResolveShanheYingxiangUseDescriptor : ShanheYingxiangDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheYingxiangUse; }
internal sealed class ShanheYingxiangHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheYingxiangHost>().ResolveYingxiang(frame, op == SkillProgramEffectOp.ResolveShanheYingxiangUse);
}

internal static class ShanheYingxiangComposition
{
    [ProgramCatalogValidation(5406)]
    internal static void Validate(string path, SkillProgram program)
    {
        var triggers = program.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op is
            SkillProgramEffectOp.ResolveShanheYingxiangMovement or SkillProgramEffectOp.ResolveShanheYingxiangUse)).ToArray();
        if (triggers.Length == 0) return;
        if (program.Id != "shanhe:companion-yingxiang" || program.Activations.Count != 0 || triggers.Length != 2 ||
            triggers.Any(trigger => trigger.Optional || trigger.Effects.Count != 1) ||
            !triggers.Any(trigger => trigger.Window == SkillProgramTriggerWindow.CardsMoved &&
                trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerBatch && trigger.Effects[0].Op == SkillProgramEffectOp.ResolveShanheYingxiangMovement) ||
            !triggers.Any(trigger => trigger.Window == SkillProgramTriggerWindow.CardUseCompleted &&
                trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Observer && trigger.Effects[0].Op == SkillProgramEffectOp.ResolveShanheYingxiangUse))
            throw new InvalidOperationException(path + ": Yingxiang requires its mandatory global movement and completed-use entries.");
    }
}
