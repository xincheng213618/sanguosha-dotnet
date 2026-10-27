namespace CardGame.Core;

internal interface IShanheSourcePojunHost
{
    SkillProgramStepOutcome HoldAllTargetHand(ProgramSkillFrame frame);
}

internal sealed class HoldShanheSourcePojunHandDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.HoldShanheSourcePojunHand;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.SlashBeforeResponse),
            new CreateCardSet("source-pojun-held", int.MaxValue, false)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.HoldTargetCards,
        static (_, context) => context.PublicControlValue(3));
    public override ISkillProgramEffectHandler Handler { get; } = new HoldShanheSourcePojunHandHandler();
}

internal sealed class HoldShanheSourcePojunHandHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.HoldShanheSourcePojunHand;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IShanheSourcePojunHost>().HoldAllTargetHand(frame);
}

public sealed record ShanheSourcePojunHeldEvent(long ProgramFrameId, long CardUseFrameId, long ActionId,
    int OwnerSeat, int TargetSeat, int Count, long SequenceBefore, long SequenceAfter) : IGameEvent;

internal static class ShanheSourcePojunContract
{
    [ProgramCatalogValidation(62049)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.HoldShanheSourcePojunHand)) &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.HoldShanheSourcePojunHand))) return;
        if (program.Id != "shanhe:source-pojun-14112" || program.Activations.Count != 0 || program.ViewAs.Count != 0 ||
            program.CardPolicies.Count != 0 || program.Modifiers.Count != 0 || program.Triggers is not
            [{ Id: "hold-all-target-hand", Window: SkillProgramTriggerWindow.SlashBeforeResponse,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, Optional: true,
                Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.HoldShanheSourcePojunHand, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always }] }] ||
            !program.Triggers[0].CardKinds.Order().SequenceEqual(new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash }.Order()) ||
            program.DamageModifiers is not [{ Amount: 1, Condition: SkillProgramDamageModifierCondition.SourceNotFewerHandAndEquipmentThanTarget,
                SourceScope: SkillProgramDamageModifierSourceScope.OwnerUsed }] ||
            !program.DamageModifiers[0].CardKinds.Order().SequenceEqual(new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash }.Order()))
            throw new InvalidOperationException(path + ": source14112 requires its independent optional whole-hand Slash hold and standard hand/equipment damage condition.");
    }
}
