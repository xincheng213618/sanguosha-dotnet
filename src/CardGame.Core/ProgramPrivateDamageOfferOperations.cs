namespace CardGame.Core;

public sealed record ProgramSelectedParticipantDiscardDraft(int TargetSeat, int Cursor);
public sealed record ProgramDamageCardOffer(int SourceSeat, string SourceBind, bool Refusing = false,
    int RequiredDiscardCount = 0, IReadOnlyList<int>? SelectedDiscardIds = null);
public sealed record ProgramPrivateCardOfferResolvedEvent(int OwnerSeat, int SourceSeat, int OfferedCount, bool Accepted, bool Prevented) : IGameEvent;
public interface IPrivateOfferProgramEffectHost
{
    SkillProgramStepOutcome DiscardSelectedParticipantCards(ProgramSkillFrame frame);
    SkillProgramStepOutcome OfferBoundCardsForDamagePrevention(ProgramSkillFrame frame,string bind);
}
internal sealed class DiscardSelectedParticipantCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op=>SkillProgramEffectOp.DiscardSelectedParticipantCards;
    public override ISkillProgramEffectHandler Handler { get; }=new DiscardSelectedParticipantCardsHandler();
    public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; }=new(ProgramOperationAiSemantic.ChooseOtherOwnedCardDiscard,static(_,_)=>{});
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op","target","condition");
        if(r.RequiredEnum<SkillProgramEffectTarget>("target")!=SkillProgramEffectTarget.SelectedTargets) throw new InvalidOperationException("Participant discard requires selectedTargets.");
        var effect=new SkillProgramEffect(Op,SkillProgramEffectTarget.SelectedTargets,0,r.Condition());RequireAlways(effect,r.Path);return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)=>[new ReadTargetSet(1)];
}
public sealed class DiscardSelectedParticipantCardsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.DiscardSelectedParticipantCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int seat,ISkillProgramEffectHost host)=>host.GetCapability<IPrivateOfferProgramEffectHost>().DiscardSelectedParticipantCards(frame);
}
internal sealed class OfferBoundCardsForDamagePreventionDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op=>SkillProgramEffectOp.OfferBoundCardsForDamagePrevention;
    public override ISkillProgramEffectHandler Handler { get; }=new OfferBoundCardsForDamagePreventionHandler();
    public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; }=new(ProgramOperationAiSemantic.PreventCurrentDamage,static(_,_)=>{});
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op","target","sourceBind","condition");
        var effect=new SkillProgramEffect(Op,FilterBoundCardsProgramOperationDescriptor.Owner(r),0,r.Condition(),sourceBind:r.RequiredIdentifier("sourceBind"));RequireAlways(effect,r.Path);return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)=>[new RequireOwnedCardSet(effect.SourceBind!,SkillProgramEffectTarget.Owner,null,[CardZoneKind.Hand,CardZoneKind.Equipment]),new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
}
public sealed class OfferBoundCardsForDamagePreventionHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.OfferBoundCardsForDamagePrevention;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int seat,ISkillProgramEffectHost host)=>host.GetCapability<IPrivateOfferProgramEffectHost>().OfferBoundCardsForDamagePrevention(frame,effect.SourceBind!);
}
