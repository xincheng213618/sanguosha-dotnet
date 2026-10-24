namespace CardGame.Core;

internal static class BoundaryCaoZhiComposition
{
    internal const string Luoying = "boundary:luoying-mobile", Jiushi = "boundary:jiushi-mobile", Upgraded = "boundary:jiushi-mobile-upgraded", Chengzhang = "boundary:chengzhang-mobile";
    internal const string Damage = "face-up-after-original-face-down-damage", Turnover = "trick-after-any-turnover", Awakening = "seven-damage-awakening";
    [ProgramCatalogValidation(56700)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Any(e => e.Op is SkillProgramEffectOp.SelectBoundaryCaoZhiTrick or SkillProgramEffectOp.UpgradeBoundaryCaoZhiJiushi)) return;
        if (p.Id == Chengzhang)
        {
            if (p.Activations.Count != 0 || p.Triggers is not [{ Id: Awakening, Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Subject: SkillProgramTriggerSubject.Owner,
                    TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: false, UsageScope: SkillUsageScope.Game, UsageLimit: 1, Condition.Kind: SkillProgramTriggerConditionKind.Always,
                    Effects: [{ Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.UpgradeBoundaryCaoZhiJiushi, Target: SkillProgramEffectTarget.Owner }] }])
                throw new InvalidOperationException(path + ": Chengzhang requires its once-game native recovery/draw before the shared permanent Jiushi upgrade.");
            return;
        }
        if (p.Id is not (Jiushi or Upgraded) || p.Triggers.Count != (p.Id == Jiushi ? 2 : 3) || p.Activations is not
            [{ Effects: [{ Op: SkillProgramEffectOp.UseVirtualAlcohol, Target: SkillProgramEffectTarget.Owner }], MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0 }])
            throw new InvalidOperationException(path + ": mobile Jiushi needs the exact original native zero-material Wine activation and configured damage/turnover branches.");
        foreach (var trigger in p.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.SelectBoundaryCaoZhiTrick)))
        {
            var pair = trigger.Effects.TakeLast(2).ToArray();
            if (pair is not [{ Op: SkillProgramEffectOp.SelectBoundaryCaoZhiTrick, Target: SkillProgramEffectTarget.Owner, ResultBind: { } bind },
                    { Op: SkillProgramEffectOp.MoveBoundCards, Target: SkillProgramEffectTarget.Owner, Destination: SkillProgramCardDestination.OwnerHand, AwaitMovementTriggers: true } move] ||
                move.SourceBind != bind || pair.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always) ||
                !(p.Id == Jiushi && trigger.Id == Damage && trigger.Window == SkillProgramTriggerWindow.AfterDamageApplied && trigger.Effects.Count == 3 && trigger.Effects[0].Op == SkillProgramEffectOp.TurnOver ||
                  p.Id == Upgraded && trigger.Id == Turnover && trigger.Window == SkillProgramTriggerWindow.CharacterTurnedOver && trigger.Effects.Count == 2))
                throw new InvalidOperationException(path + ": a random Jiushi trick must stay on its exact native damage/turnover frame and use one awaited bound-card gain.");
        }
    }
}

internal interface IBoundaryCaoZhiHost
{
    void SelectTrick(ProgramSkillFrame frame, string bind);
    void Upgrade(ProgramSkillFrame frame);
}
internal sealed class SelectBoundaryCaoZhiTrickDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectBoundaryCaoZhiTrick;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryCaoZhiHandler(SkillProgramEffectOp.SelectBoundaryCaoZhiTrick);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind"));
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.AfterDamageApplied, SkillProgramTriggerWindow.CharacterTurnedOver]), new CaptureSourceCard(e.ResultBind!, 1)];
}
internal sealed class UpgradeBoundaryCaoZhiJiushiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.UpgradeBoundaryCaoZhiJiushi;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryCaoZhiHandler(SkillProgramEffectOp.UpgradeBoundaryCaoZhiJiushi);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantSkills, static (_, c) => c.PublicControlValue(3));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class BoundaryCaoZhiHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host)
    {
        var capability = host.GetCapability<IBoundaryCaoZhiHost>();
        if (op == SkillProgramEffectOp.SelectBoundaryCaoZhiTrick) capability.SelectTrick(frame, effect.ResultBind!); else capability.Upgrade(frame);
        return SkillProgramStepOutcome.Continue;
    }
}

public sealed record BoundaryCaoZhiDamageFaceEvent(long DamageFrameId, long NativeAttemptId, int OwnerSeat, bool WasFaceDown) : IGameEvent;
public sealed record BoundaryCaoZhiTrickSelectedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    int InstructionIndex, int PoolCount, int? RandomIndex, int? CardId, uint RandomBefore, uint RandomAfter) : IGameEvent;
public sealed record BoundaryCaoZhiJiushiUpgradedEvent(long FrameId, CardConversionSource Source, int GameDamageTotal) : IGameEvent;
