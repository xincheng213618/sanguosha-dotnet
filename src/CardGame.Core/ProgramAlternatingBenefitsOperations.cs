namespace CardGame.Core;
public enum SkillProgramAlternatingHistoryScope { PreviousOwnerTurn = 1 }
public enum SkillProgramAlternatingDrawTiming { NextDrawPhase = 1 }
public sealed partial class SkillProgramEffect
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public SkillProgramAlternatingHistoryScope? AlternatingHistoryScope { get; internal init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public SkillProgramAlternatingDrawTiming? AlternatingDrawTiming { get; internal init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public bool? PersistAlternatingTargetsAfterSkillLoss { get; internal init; }
}
internal interface IAlternatingBenefitsProgramHost
{
    void ApplyAlternatingChoiceBenefit(ProgramSkillFrame frame, SkillProgramEffect effect);
    SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame frame, int targetSeat, SkillProgramEffect effect);
}
internal sealed record RequireOwnTurnBoundary : ProgramResourceOperation;
internal sealed record RequireChoiceOptions(string Name, IReadOnlyList<string> Options) : ProgramResourceOperation;
internal sealed class ApplyAlternatingChoiceBenefitDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyAlternatingChoiceBenefit;
    public override ISkillProgramEffectHandler Handler { get; } = new ApplyAlternatingChoiceBenefitHandler();
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (e,c)=>c.GrantTurnRuleModifier(e));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op","target","sourceBind","stateId","condition","historyScope","drawTiming","persistTargetsAfterSkillLoss");
        var effect=new SkillProgramEffect(Op,FilterBoundCardsProgramOperationDescriptor.Owner(r),0,r.Condition(),sourceBind:r.RequiredIdentifier("sourceBind"),stateId:r.RequiredIdentifier("stateId"))
        {
            AlternatingHistoryScope = r.Has("historyScope") ? r.RequiredEnum<SkillProgramAlternatingHistoryScope>("historyScope") : null,
            AlternatingDrawTiming = r.Has("drawTiming") ? r.RequiredEnum<SkillProgramAlternatingDrawTiming>("drawTiming") : null,
            PersistAlternatingTargetsAfterSkillLoss = r.Has("persistTargetsAfterSkillLoss") ? r.RequiredBool("persistTargetsAfterSkillLoss") : null
        };
        RequireAlways(effect,r.Path);return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),new RequireChoiceOptions(e.SourceBind!,["draw","targets"])];
}
internal sealed class ObtainDeckCardWithConsecutiveTargetDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ObtainDeckCardWithConsecutiveTarget;
    public override ISkillProgramEffectHandler Handler { get; } = new ObtainDeckCardWithConsecutiveTargetHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (e,c)=>c.Draw(e));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op","target","stateId","resultBind","cardSuits","cardCategories","condition");
        if(r.RequiredEnum<SkillProgramEffectTarget>("target")!=SkillProgramEffectTarget.SelectedTarget) throw new InvalidOperationException("A consecutive-target deck gift requires another selected participant.");
        var suits=r.RequiredEnumArray<Suit>("cardSuits");var categories=r.RequiredEnumArray<SkillProgramCardCategory>("cardCategories");
        if(suits.Count==0 || categories.Count==0) throw new InvalidOperationException("Deck matching requires nonempty suits and categories.");
        var e=new SkillProgramEffect(Op,SkillProgramEffectTarget.SelectedTarget,1,r.Condition(),stateId:r.RequiredIdentifier("stateId"),resultBind:r.RequiredIdentifier("resultBind"),suits:suits,cardCategories:categories);
        RequireAlways(e,r.Path);return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding),new RequireOwnTurnBoundary(),new ReadSelectedTarget(),new RequireSelectedTargetKind(SkillProgramTargetKind.OtherLiving),new CreateChoiceResult(e.ResultBind!,["repeat","new"])];
}
public sealed class ApplyAlternatingChoiceBenefitHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.ApplyAlternatingChoiceBenefit;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e,ProgramSkillFrame f,int seat,ISkillProgramEffectHost host)
    {host.GetCapability<IAlternatingBenefitsProgramHost>().ApplyAlternatingChoiceBenefit(f,e);return SkillProgramStepOutcome.Continue;}
}
public sealed class ObtainDeckCardWithConsecutiveTargetHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.ObtainDeckCardWithConsecutiveTarget;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e,ProgramSkillFrame f,int seat,ISkillProgramEffectHost host)=>host.GetCapability<IAlternatingBenefitsProgramHost>().ObtainDeckCardWithConsecutiveTarget(f,seat,e);
}
