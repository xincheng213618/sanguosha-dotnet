namespace CardGame.Core;
public enum ProgramResponseExchangeMode { ClaimResponseOnly = 0 }
public enum ProgramPublicSuitDiscardMode { ChooseSuitCappedCurrentCountRetain = 0 }
public sealed partial class SkillProgramEffect
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ProgramResponseExchangeMode? ResponseExchangeMode { get; internal init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ProgramPublicSuitDiscardMode? PublicSuitDiscardMode { get; internal init; }
}
internal sealed record RequireResponseActionObserver : ProgramResourceOperation;

internal interface IResponseExchangeProgramHost
{
    SkillProgramStepOutcome ExchangeRespondedCardEntities(ProgramSkillFrame frame, SkillProgramEffect effect);
    SkillProgramStepOutcome DrawPublicSuitThenEscalatingDiscard(ProgramSkillFrame frame, SkillProgramEffect effect);
}
internal sealed class ExchangeRespondedCardEntitiesDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ExchangeRespondedCardEntities;
    public override ISkillProgramEffectHandler Handler { get; } = new ExchangeRespondedCardEntitiesHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (e, c) => c.Draw(new SkillProgramEffect(SkillProgramEffectOp.Draw,SkillProgramEffectTarget.Owner,1,e.Condition)));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "stateId", "responseExchangeMode", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner) throw new InvalidOperationException("Response exchange belongs to the responded action's owner.");
        var effect = new SkillProgramEffect(Op, SkillProgramEffectTarget.Owner, 0, r.Condition(), stateId:r.RequiredIdentifier("stateId"))
        { ResponseExchangeMode = r.Has("responseExchangeMode") ? r.RequiredEnum<ProgramResponseExchangeMode>("responseExchangeMode") : null };
        RequireAlways(effect,r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireResponseActionObserver()];
}
public sealed class ExchangeRespondedCardEntitiesHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ExchangeRespondedCardEntities;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,int targetSeat,ISkillProgramEffectHost host) => host.GetCapability<IResponseExchangeProgramHost>().ExchangeRespondedCardEntities(frame,effect);
}
internal sealed class DrawPublicSuitThenEscalatingDiscardDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawPublicSuitThenEscalatingDiscard;
    public override ISkillProgramEffectHandler Handler { get; } = new DrawPublicSuitThenEscalatingDiscardHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOtherOwnedCardDiscard,static (_,_)=>{});
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op","target","stateId","skillIds","suits","publicSuitDiscardMode","condition");
        var ids=r.RequiredIdentifierArray("skillIds");var suits=r.RequiredEnumArray<Suit>("suits");
        var mode = r.Has("publicSuitDiscardMode") ? r.RequiredEnum<ProgramPublicSuitDiscardMode>("publicSuitDiscardMode") : (ProgramPublicSuitDiscardMode?)null;
        if(r.RequiredEnum<SkillProgramEffectTarget>("target")!=SkillProgramEffectTarget.SelectedTarget || ids.Count!=1 ||
            (mode is null ? suits.Count!=1 : suits.Count!=4 || suits.Distinct().Count()!=4 || suits.Any(s=>s is not(Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond))))
            throw new InvalidOperationException("An escalating public-suit discard requires one target, one upgraded skill and the policy's exact suit domain.");
        var effect=new SkillProgramEffect(Op,SkillProgramEffectTarget.SelectedTarget,0,r.Condition(),stateId:r.RequiredIdentifier("stateId"),skillIds:ids,suits:suits)
        { PublicSuitDiscardMode = mode };
        RequireAlways(effect,r.Path);return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding),new ReadSelectedTarget(),new RequireSelectedTargetKind(SkillProgramTargetKind.OtherLiving)];
}
public sealed class DrawPublicSuitThenEscalatingDiscardHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DrawPublicSuitThenEscalatingDiscard;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int targetSeat,ISkillProgramEffectHost host)=>host.GetCapability<IResponseExchangeProgramHost>().DrawPublicSuitThenEscalatingDiscard(frame,effect);
}
public sealed record ProgramResponseEntityExchangeDraft(long RespondedActionId,long ResponseActionId,int RecipientSeat,IReadOnlyList<int> OriginalCardIds,IReadOnlyList<int> ResponseCardIds,string StateId,int Stage=0);
public sealed record ProgramPublicSuitDiscardDraft(int TargetSeat,int Required,string StateId,string UpgradeSkillId,int Stage=0,IReadOnlyList<int>? SelectedIds=null,bool Exhausted=false)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] public Suit? ChosenSuit { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] public int? DrawCount { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] public IReadOnlyList<int>? PaidHandIds { get; init; }
}
public sealed record ProgramResponseEntityClaimedEvent(int OwnerSeat,string SkillId,string SkillInstanceId,long RespondedActionId,long ResponseActionId,long ClaimedActionId,int RecipientSeat,IReadOnlyList<int> CardIds,int TurnNumber,int TurnSeat,bool Restricted):IGameEvent;
public sealed record ProgramEscalatingDiscardStartedEvent(int OwnerSeat,string SkillId,string SkillInstanceId,string StateId,int PreviousCount,int TargetSeat,int DrawCount):IGameEvent;
public sealed record ProgramResponseExchangeUpgradedEvent(int OwnerSeat,string LostSkillId,string LostSkillInstanceId,string SkillId,string SkillInstanceId,string StateId):IGameEvent;
public sealed record ProgramResponseExchangeStateSnapshot(int OwnerSeat,string SkillId,string SkillInstanceId,string StateId,bool IsUpgraded);
public sealed record TurnProhibitedPhysicalCardsSnapshot(int RecipientSeat,int SourceOwnerSeat,string SourceSkillId,string SourceSkillInstanceId,IReadOnlyList<int> CardIds,int TurnNumber,int TurnSeat);
