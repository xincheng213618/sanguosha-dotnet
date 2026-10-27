using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheShennuReceivedReceipt(long DamageWindowId, long DamageFrameId, int InstructionIndex,
    int? SourceSeat, int SourceHp, int DamageAmount, int NuBefore, int NuAfter);
public sealed record ShanheShennuOutgoingReceipt(long AttackResolutionId, CardUseEffectSource Source,
    string GameplayHash, int TargetSeat, int DamageAmount, bool Cleared = false);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheShennuReceivedReceipt? ShanheShennuReceived { get; init; }
}
public sealed partial record DamageFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheShennuOutgoingReceipt? ShanheShennuOutgoing { get; init; }
}
public sealed record ShanheShennuReceivedEvent(long ProgramFrameId, CardConversionSource Source,
    ShanheShennuReceivedReceipt Receipt) : IGameEvent;
public sealed record ShanheShennuOutgoingIssuedEvent(long DamageFrameId, ShanheShennuOutgoingReceipt Receipt) : IGameEvent;
public sealed record ShanheShennuClearedEvent(long DamageFrameId, long AttackResolutionId, int OwnerSeat, int RemovedNu) : IGameEvent;

internal interface IShanheSourceShennuHost { void ReceiveDamage(ProgramSkillFrame frame); }
internal sealed class ResolveShanheSourceShennuDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceShennu;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.DamageAppliedBeforeDying)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheSourceShennuHandler();
}
internal sealed class ResolveShanheSourceShennuHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceShennu;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IShanheSourceShennuHost>().ReceiveDamage(frame); return SkillProgramStepOutcome.Continue; }
}
internal static class ShanheSourceShennuContract
{
    [ProgramCatalogValidation(62048)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceShennu)) &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceShennu))) return;
        if (program.Id != "shanhe:source-shennu-14214" || program.Activations.Count != 0 || program.ViewAs.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 || program.Triggers is not
            [{ Id: "actual-damage-received", Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying,
                Subject: SkillProgramTriggerSubject.Owner, DamageOccurrence: SkillProgramDamageOccurrence.PerDamage,
                Optional: false, Condition.Kind: SkillProgramTriggerConditionKind.Always, Effects: [{ Op: SkillProgramEffectOp.ResolveShanheSourceShennu,
                    Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": source Shennu requires its one locked per-damage owner instruction.");
    }
}
