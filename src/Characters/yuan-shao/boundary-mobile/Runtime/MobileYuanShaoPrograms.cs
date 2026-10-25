namespace CardGame.Core;

internal interface IMobileYuanShaoProgramHost { SkillProgramStepOutcome Draw(ProgramSkillFrame frame, bool responder); }
internal interface IMobileYuanShaoSelectionQueries : IProgramSelectionQuery { bool SuitAvailable(int owner, Suit suit); }
internal sealed class DrawMobileLuanjiResponderDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawMobileLuanjiResponder;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileYuanShaoDrawHandler(SkillProgramEffectOp.DrawMobileLuanjiResponder);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardResponseCompleted)];
}
internal sealed class DrawMobileLuanjiNoDamageDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawMobileLuanjiNoDamage;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileYuanShaoDrawHandler(SkillProgramEffectOp.DrawMobileLuanjiNoDamage);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCompleted)];
}
internal sealed class MobileYuanShaoDrawHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => host.GetCapability<IMobileYuanShaoProgramHost>().Draw(frame, op == SkillProgramEffectOp.DrawMobileLuanjiResponder);
}
[ProgramSelectionModule(ProgramSelectionStage.CardAfterDeclaration, 66001, SkillProgramEffectOp.UseSelectedCardsAs)]
internal sealed class MobileYuanShaoSuitFilter : IProgramSelectionCardFilter
{
    public bool Allows(ProgramSelectionContext context, Card card, IProgramSelectionQueries queries) => context.SkillId != "boundary:luanji-mobile" || queries.Get<IMobileYuanShaoSelectionQueries>().SuitAvailable(context.OwnerSeat, card.Suit);
}
internal static class MobileYuanShaoComposition
{
    [ProgramCatalogValidation(66009)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "boundary:luanji-mobile" && !program.Triggers.SelectMany(t => t.Effects).Any(e => e.Op is SkillProgramEffectOp.DrawMobileLuanjiResponder or SkillProgramEffectOp.DrawMobileLuanjiNoDamage)) return;
        var valid = program.Id == "boundary:luanji-mobile" && program.Modifiers.Count == 0 && program.CardPolicies.Count == 0 && program.Activations is
            [{ Id: "unused-suit-pair-as-arrow-barrage", MinCards: 2, MaxCards: 2, MinTargets: 0, MaxTargets: 0, UsesPerTurn: null, UsesPerPhase: null, UsesPerGame: null,
                Effects: [{ Op: SkillProgramEffectOp.UseSelectedCardsAs, SourceBind: "unused-suit-pair-as-arrow-barrage", OutputKind: CardKind.ArrowBarrage }] }] &&
            program.ViewAs is [{ Id: "unused-suit-pair-as-arrow-barrage", InputCount: 2, SameSuit: false, OutputKind: CardKind.ArrowBarrage, ForPlay: true, ForResponse: false }] &&
            program.Triggers is [
                { Id: "actual-arrow-dodge-draw", Window: SkillProgramTriggerWindow.CardResponseCompleted, OwnerRelation: SkillProgramCardActionOwnerRelation.Observer, Optional: false,
                    Effects: [{ Op: SkillProgramEffectOp.DrawMobileLuanjiResponder, Target: SkillProgramEffectTarget.Owner }] },
                { Id: "undamaged-arrow-draw", Window: SkillProgramTriggerWindow.CardUseCompleted, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, SingleActionInstance: true, Optional: false,
                    Effects: [{ Op: SkillProgramEffectOp.DrawMobileLuanjiNoDamage, Target: SkillProgramEffectTarget.Owner }] }];
        if (!valid) throw new InvalidOperationException(path + ": mobile Luanji requires an arbitrary unused-suit hand pair, actual completed Dodge reward and whole-use no-damage reward.");
    }
}
