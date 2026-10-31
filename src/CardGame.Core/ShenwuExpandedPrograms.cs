using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IShenwuExpandedProgramHost
{
    SkillProgramStepOutcome ExecuteShenwuExpanded(ProgramSkillFrame frame, SkillProgramEffectOp op);
}
internal abstract class ShenwuExpandedOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new ShenwuExpandedEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
}
internal sealed class AddShenwuDamagePoisonDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.AddShenwuDamagePoison;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
}
internal sealed class ResolveShenwuPoisonDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShenwuPoison;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class ResolveShenwuXunyouDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShenwuXunyou;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class ResolveShenwuHonglianDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShenwuHonglian;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class ResolveShenwuXushiDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShenwuXushi;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CharacterTurnedFaceUp)];
}
internal sealed class BindShenwuTurnOwnerDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindShenwuTurnOwner;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow), new SelectSingleTarget()];
}
internal sealed class GateShenwuYanyuDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GateShenwuYanyu;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class BindShenwuEndedTurnOwnerDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindShenwuEndedTurnOwner;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded), new SelectSingleTarget()];
}
internal sealed class RecoverShenwu2020RebirthDescriptor : ShenwuExpandedOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecoverShenwu2020Rebirth;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.SelfDyingResponse)];
}
internal sealed class ShenwuExpandedEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShenwuExpandedProgramHost>().ExecuteShenwuExpanded(frame, op);
}

public sealed record ShenwuExpandedDamage(int Seat, int Amount);
public sealed record ShenwuExpandedResolution(string Stage, int Amount = 0, int Cursor = 0, int? CardId = null,
    IReadOnlyList<int>? SelectedIds = null, IReadOnlyList<ShenwuExpandedDamage>? Damage = null);
public sealed record ShenwuExpandedRandomEvent(long ProgramFrameId, string SkillId, int Roll,
    int ObtainedCount, IReadOnlyList<ShenwuExpandedDamage> Damage) : IGameEvent;
public sealed record ShenwuTurnEndHealthEvent(int TurnNumber, int OwnerSeat, int Hp) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShenwuExpandedResolution? ShenwuExpanded { get; init; }
}

public sealed partial class GameEngine
{
}
