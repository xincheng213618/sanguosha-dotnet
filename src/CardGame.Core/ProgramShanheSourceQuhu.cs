namespace CardGame.Core;

internal interface IShanheSourceQuhuHost
{
    SkillProgramStepOutcome DamageOpponentMaximumHp(ProgramSkillFrame frame, int targetSeat);
}

internal sealed class DamageShanheQuhuOpponentMaximumHpDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp;
    public override ISkillProgramEffectHandler Handler { get; } = new DamageShanheQuhuOpponentMaximumHpHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage,
        static (effect, context) => context.Damage(effect));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceRef", "skipIfNoTarget", "condition");
        var target = reader.RequiredEnum<SkillProgramEffectTarget>("target");
        var source = reader.RequiredParticipantReference("sourceRef");
        var condition = reader.Condition();
        if (target != SkillProgramEffectTarget.SelectedTarget ||
            source != new ProgramParticipantReference(ProgramParticipantRef.ResultOpponent, "__active-pindian-result") ||
            !reader.RequiredBool("skipIfNoTarget") || condition.Kind != SkillProgramConditionKind.PindianWon ||
            condition.SourceBind != "__active-pindian-result")
            throw new InvalidOperationException(reader.Path + ": source Quhu requires its winning original opponent and selected range target.");
        return new(Op, target, 5, condition, actorReference: source, skipIfNoTarget: true);
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadSelectedTarget(), new ReadPindianResult("__active-pindian-result")];
}

internal sealed class DamageShanheQuhuOpponentMaximumHpHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IShanheSourceQuhuHost>().DamageOpponentMaximumHp(frame, targetSeat);
}

internal static class ShanheSourceQuhuContract
{
    [ProgramCatalogValidation(62050)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp)) &&
            !program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp))) return;
        if (program.Id != "shanhe:source-quhu-14190" || program.Triggers.Count != 0 || program.ViewAs.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.Activations is not [{ Id: "contest", MinCards: 1, MaxCards: 1, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.OtherLivingWithHandHpGreaterThanOwner, UsesPerTurn: null, UsesPerPhase: 1,
                Condition.Kind: SkillProgramConditionKind.Always, Effects: [
                    { Op: SkillProgramEffectOp.Pindian, Target: SkillProgramEffectTarget.SelectedTarget },
                    { Op: SkillProgramEffectOp.SelectTarget, Target: SkillProgramEffectTarget.Owner,
                        TargetKind: SkillProgramTargetKind.OtherLivingInBoundParticipantAttackRange, SkipIfNoTarget: true,
                        Condition.Kind: SkillProgramConditionKind.PindianWon, Condition.SourceBind: "__active-pindian-result",
                        ActorReference.Kind: ProgramParticipantRef.ResultOpponent, ActorReference.ResultBind: "__active-pindian-result" },
                    { Op: SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp },
                    { Op: SkillProgramEffectOp.Damage, Target: SkillProgramEffectTarget.Owner, Amount: 1,
                        ActorReference.Kind: ProgramParticipantRef.ResultOpponent, ActorReference.ResultBind: "__active-pindian-result",
                        Condition.Kind: SkillProgramConditionKind.PindianNotWon, Condition.SourceBind: "__active-pindian-result" }
                ] }])
            throw new InvalidOperationException(path + ": source Quhu requires its native once-per-Play contest, opponent-range choice and two outcome branches.");
    }
}
