namespace CardGame.Core;

public sealed record MobileShiXieIncomingDistanceChangedEvent(long ProgramFrameId, int InstructionIndex,
    CardUseEffectSource Source, string GameplayHash, int ActualTurnNumber, int ActualTurnOwnerSeat, int Amount) : IGameEvent;
public sealed record MobileShiXieCourtesyDrawIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardUseEffectSource Source, string GameplayHash, int RecipientSeat, int ActualCount, long MovementSequence) : IGameEvent;

internal static class MobileShiXieComposition
{
    internal const string Biluan = "mobile:biluan",Lixia = "mobile:lixia",Benefit = "lixia-benefit";
    internal static bool Handles(SkillProgramEffectOp op) => op is SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions or SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance;
    [ProgramCatalogValidation(78900)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => Handles(e.Op))) return;
        bool Plain(SkillProgramEffect e, SkillProgramEffectOp op) => e.Op == op && e.Target == SkillProgramEffectTarget.Owner && e.Condition.Kind == SkillProgramConditionKind.Always && e.TargetReference is null;
        var valid = p.Id switch
        {
            Biluan => p.Triggers is [{ Id: "skip-normal-draw-for-distance", Window: SkillProgramTriggerWindow.DrawPhaseStarting, Subject: SkillProgramTriggerSubject.Owner,
                TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true, DrawPhaseMode: SkillProgramDrawPhaseMode.Replacement, UsageScope: null, UsageLimit: null, Effects: [var increase] }] &&
                Plain(increase, SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions),
            Lixia => p.Triggers is [{ Id: "one-other-ending-benefit", Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner,
                TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Optional: false, UsageScope: null, UsageLimit: null,
                Effects: [{ Op: SkillProgramEffectOp.ChooseOption, Target: SkillProgramEffectTarget.Owner, ResultBind: Benefit, Options: [{ Id: "self" }, { Id: "current" }] },
                    var decrease] }] && Plain(decrease, SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance),
            _ => false
        };
        if (!valid || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.Contributions.Count != 0 || p.CardPolicies.Count != 0 || p.BooleanStates.Count != 0)
            throw new InvalidOperationException(path + ": mobile Shi Xie requires its optional complete native Draw replacement and one mandatory mutually exclusive one-card Ending benefit before permanent incoming-distance settlement.");
    }
}
internal interface IMobileShiXieHost { SkillProgramStepOutcome Change(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class MobileShiXieDistanceDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new MobileShiXieDistanceHandler(Op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(Op == SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions ? SkillProgramTriggerWindow.DrawPhaseStarting : SkillProgramTriggerWindow.TurnEnding)];
}
internal sealed class IncreaseMobileShiXieDistanceDescriptor : MobileShiXieDistanceDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions; }
internal sealed class DecreaseMobileShiXieDistanceDescriptor : MobileShiXieDistanceDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class MobileShiXieDistanceHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host)
        => host.GetCapability<IMobileShiXieHost>().Change(f, op);
}
