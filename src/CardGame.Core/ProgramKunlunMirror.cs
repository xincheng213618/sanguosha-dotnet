using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record KunlunMirrorDamageOrigin(long AttackResolutionId, long? CardActionId, int? NativeCardId,
    CardKind EffectiveKind, string CardName, int CardUserSeat, int? SourceSeat, int TargetSeat,
    bool ChainPropagation, bool DelayedJudgment);
public sealed record KunlunMirrorReceipt(int InstructionIndex, long WindowFrameId, long? DamageFrameId,
    CardConversionSource Source, string GameplayHash, KunlunMirrorDamageOrigin Origin, int Amount, bool Learning);
public sealed record KunlunMirrorTrickLearnedEvent(long ProgramFrameId, KunlunMirrorReceipt Receipt) : IGameEvent;
public sealed record KunlunMirrorPreventionIssuedEvent(long ProgramFrameId, KunlunMirrorReceipt Receipt) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public KunlunMirrorReceipt? KunlunMirror { get; init; }
}

internal interface IKunlunMirrorHost { void Resolve(ProgramSkillFrame frame); }
internal sealed class ResolveKunlunMirrorDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveKunlunMirror;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.BeforeDamageApplied, SkillProgramTriggerWindow.DamageAppliedBeforeDying])];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveKunlunMirrorHandler();
}
internal sealed class ResolveKunlunMirrorHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveKunlunMirror;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    { host.GetCapability<IKunlunMirrorHost>().Resolve(frame); return SkillProgramStepOutcome.Continue; }
}

internal static class KunlunMirrorContract
{
    [ProgramCatalogValidation(62056)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveKunlunMirror)) &&
            !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveKunlunMirror))) return;
        if (program.Id != "shanhe:kunlun-mirror-14080" || program.Activations.Count != 0 || program.ViewAs.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 || program.Triggers is not [
                { Id: "remember-real-trick-damage", Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying,
                  Subject: SkillProgramTriggerSubject.Owner, Optional: false, DamageOccurrence: SkillProgramDamageOccurrence.PerDamage,
                  Condition.Kind: SkillProgramTriggerConditionKind.Always,
                  Effects: [{ Op: SkillProgramEffectOp.ResolveKunlunMirror, Target: SkillProgramEffectTarget.Owner }] },
                { Id: "prevent-remembered-trick", Window: SkillProgramTriggerWindow.BeforeDamageApplied,
                  Subject: SkillProgramTriggerSubject.DamageTarget, Optional: false, Condition.Kind: SkillProgramTriggerConditionKind.Always,
                  Effects: [{ Op: SkillProgramEffectOp.ResolveKunlunMirror, Target: SkillProgramEffectTarget.Owner },
                    { Op: SkillProgramEffectOp.PreventCurrentDamage, Target: SkillProgramEffectTarget.Owner }] }
            ])
            throw new InvalidOperationException(path + ": Kunlun Mirror requires its exact real-damage learning and native same-name prevention branches.");
    }
}
