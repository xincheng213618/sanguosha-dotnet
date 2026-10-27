using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HuxinMirrorPaymentReceipt(int InstructionIndex, long WindowFrameId, long AttackFrameId,
    CardConversionSource Source, string GameplayHash, int CardId, int Amount, int MovementSequence);
public sealed record HuxinMirrorPaidEvent(long ProgramFrameId, HuxinMirrorPaymentReceipt Receipt) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HuxinMirrorPaymentReceipt? HuxinMirrorPayment { get; init; }
}
public sealed record HuxinMirrorPlacementFrame(long Id, int ActorSeat, int TargetSeat, int CardId,
    CardLocation From, int SequenceBefore, int SequenceAfter = 0)
    : ResolutionFrame(Id, ResolutionFrameKind.HuxinMirrorPlacement, ResolutionFrameStep.ResolvingEffect);
public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeHuxinMirrorPlacementFrameId { get; init; }
}
internal interface IHuxinMirrorHost
{
    SkillProgramStepOutcome PayAndPrevent(ProgramSkillFrame frame);
}
internal sealed class DiscardHuxinMirrorAndPreventDamageDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(5));
    public override ISkillProgramEffectHandler Handler { get; } = new DiscardHuxinMirrorAndPreventDamageHandler();
}
internal sealed class DiscardHuxinMirrorAndPreventDamageHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IHuxinMirrorHost>().PayAndPrevent(frame);
}
internal static class HuxinMirrorContract
{
    internal const string SkillId = "shanhe:huxin-mirror-2055";
    [ProgramCatalogValidation(62058)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage)) &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage))) return;
        if (program.Id != SkillId || program.Activations.Count != 0 || program.ViewAs.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.Triggers is not [{ Id: "pay-real-armor-to-prevent", Window: SkillProgramTriggerWindow.BeforeDamageApplied,
                Subject: SkillProgramTriggerSubject.DamageTarget, Optional: true,
                Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path + ": Huxin Mirror requires its optional exact native armor-payment prevention.");
    }
}
