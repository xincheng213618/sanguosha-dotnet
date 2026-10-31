namespace CardGame.Core;

internal interface IBoundaryXuChuHost
{ void SubstituteBoundaryXuChuDraw(ProgramSkillFrame frame); }

internal sealed class SubstituteBoundaryXuChuDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SubstituteBoundaryXuChuDraw;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryXuChuDrawHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.DrawPlan;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.AdjustNormalDraw,
        static (_, context) => context.BoundaryXuChuSkipDraw());
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition());
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseStarting), new ReadCardSet(BoundaryXuChuMobile.Gained)];
}
internal sealed class BoundaryXuChuDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.SubstituteBoundaryXuChuDraw;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IBoundaryXuChuHost>().SubstituteBoundaryXuChuDraw(f);
        return SkillProgramStepOutcome.Continue;
    }
}

internal static class BoundaryXuChuMobile
{
    internal const string Skill = "boundary:luoyi-mobile",Reveal = "luoyi-revealed",Gained = "luoyi-gained",Choice = "luoyi-draw-choice";
    internal const string Skip = "skip-draw",Normal = "normal-draw";
    [ProgramCatalogValidation(59900)]
    private static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects))
            .Any(e => e.Op == SkillProgramEffectOp.SubstituteBoundaryXuChuDraw)) return;
        if (p.Id != Skill || p.Activations.Count != 0 || p.Triggers is not [{ Id: "reveal-three-then-choose",
            Window: SkillProgramTriggerWindow.DrawPhaseStarting, Subject: SkillProgramTriggerSubject.Owner, Optional: false,
            DrawPhaseMode: SkillProgramDrawPhaseMode.Additive, Effects: [var reveal, var filter, var choice, var skip, var gain, var rest, var decline, var bonus] }] ||
            reveal.Op != SkillProgramEffectOp.RevealTopCards || reveal.Amount != 3 || reveal.ResultBind != Reveal || reveal.Visibility != SkillProgramCardSetVisibility.Public ||
            filter.Op != SkillProgramEffectOp.FilterBoundCards || filter.SourceBind != Reveal || filter.ResultBind != Gained ||
            !filter.CardCategories.SequenceEqual([SkillProgramCardCategory.Basic]) || !filter.EquipmentSlots.SequenceEqual([EquipmentSlot.Weapon]) ||
            !filter.CardKinds.SequenceEqual([CardKind.Duel]) || filter.Suits.Count != 0 || filter.TargetReference is not null ||
            choice.Op != SkillProgramEffectOp.ChooseOption || choice.ResultBind != Choice || choice.ChooserRef is not null and not { Kind: ProgramParticipantRef.Owner } ||
            choice.Options is not [{ Id: Skip, Condition: { Kind: SkillProgramConditionKind.BoundCardCountAtLeast, SourceBind: Gained, Value: 1 } }, { Id: Normal, Condition.Kind: SkillProgramConditionKind.Always }] ||
            skip.Op != SkillProgramEffectOp.SubstituteBoundaryXuChuDraw || !Branch(skip, Skip) ||
            gain.Op != SkillProgramEffectOp.MoveBoundCards || gain.SourceBind != Gained || gain.ExceptBind is not null || gain.Destination != SkillProgramCardDestination.OwnerHand || !Branch(gain, Skip) ||
            rest.Op != SkillProgramEffectOp.MoveBoundCards || rest.SourceBind != Reveal || rest.ExceptBind != Gained || rest.Destination != SkillProgramCardDestination.DiscardPile || !Branch(rest, Skip) ||
            decline.Op != SkillProgramEffectOp.MoveBoundCards || decline.SourceBind != Reveal || decline.ExceptBind is not null || decline.Destination != SkillProgramCardDestination.DiscardPile || !Branch(decline, Normal) ||
            bonus.Op != SkillProgramEffectOp.GrantTurnCardDamageModifier || bonus.Amount != 1 || !ProgramCardKindMatch.IsFullSlashFamily(bonus.CardKinds.Where(k => k != CardKind.Duel).ToArray()) || !bonus.CardKinds.Contains(CardKind.Duel) ||
            bonus.DamageModifierExpiration != SkillProgramDamageModifierExpiration.NextOwnerTurnStart || bonus.DamageModifierSourceScope != SkillProgramDamageModifierSourceScope.DamageSource || !Branch(bonus, Skip) ||
            new[] { reveal, filter, choice }.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always) ||
            new[] { gain, rest, decline }.Any(e => !e.AwaitMovementTriggers) || p.Triggers[0].Effects.Any(e => e.Target != SkillProgramEffectTarget.Owner))
            throw new InvalidOperationException(path + ": mobile Luoyi requires mandatory public three-card reveal, optional nonempty basic/weapon/Duel gain, actual draw substitution and native source-damage bonus.");
    }
    private static bool Branch(SkillProgramEffect e, string option) => e.Condition is
        { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: Choice, OptionId: var selected } && selected == option;
}

internal sealed partial class ProgramAiEstimateContext
{
    internal void BoundaryXuChuSkipDraw() => _ownerDraw -= Math.Max(0, _publicContext.NormalDrawCount);
}
