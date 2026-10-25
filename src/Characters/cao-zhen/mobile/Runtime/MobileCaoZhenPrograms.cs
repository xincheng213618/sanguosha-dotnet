namespace CardGame.Core;

public sealed record MobileCaoZhenSidiAppliedEvent(long ProgramFrameId, CardConversionSource Source, string GameplayHash,
    int ActualTurnNumber, int ActualTurnOwnerSeat, int PlayPhaseInstanceId, int AffectedSeat, int PaidCardId, long PaymentSequence) : IGameEvent;
internal interface IMobileCaoZhenSidiHost { SkillProgramStepOutcome Consume(ProgramSkillFrame frame, string sourceBind); }
internal sealed class MobileCaoZhenSidiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeMobileCaoZhenSidi;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileCaoZhenSidiHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(6));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition(), sourceBind: reader.RequiredIdentifier("sourceBind"));
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayPhaseStarting), new ReadSingleCardSet(effect.SourceBind!),
            new MoveCardSet(effect.SourceBind!, null, SkillProgramCardDestination.DiscardPile)];
}
internal sealed class MobileCaoZhenSidiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeMobileCaoZhenSidi;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileCaoZhenSidiHost>().Consume(frame, effect.SourceBind!);
}
internal static class MobileCaoZhenSidiComposition
{
    [ProgramCatalogValidation(74010)]
    private static void Validate(string path, SkillProgram program)
    {
        foreach (var trigger in program.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ConsumeMobileCaoZhenSidi)))
            if (program.Id != "mobile:sidi" || trigger.Window != SkillProgramTriggerWindow.PlayPhaseStarting || trigger.Subject != SkillProgramTriggerSubject.Owner ||
                trigger.TurnOwnerScope != SkillProgramTurnOwnerScope.OtherLiving || !trigger.Optional || trigger.Effects is not [
                    { Op: SkillProgramEffectOp.SelectSourceCard, Target: SkillProgramEffectTarget.Owner, CardSource: SkillProgramCardSource.Owner,
                        Zones: [CardZoneKind.Authority], ResultBind: "spent-qian", Condition.Kind: SkillProgramConditionKind.Always },
                    { Op: SkillProgramEffectOp.ConsumeMobileCaoZhenSidi, Target: SkillProgramEffectTarget.Owner, SourceBind: "spent-qian", Condition.Kind: SkillProgramConditionKind.Always }])
                throw new InvalidOperationException(path + ": mobile Sidi consumes one selected original Qian only on its other-actor Play starting binding.");
        if (program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ConsumeMobileCaoZhenSidi)))
            throw new InvalidOperationException(path + ": mobile Sidi payment requires the original phase trigger.");
    }
}
