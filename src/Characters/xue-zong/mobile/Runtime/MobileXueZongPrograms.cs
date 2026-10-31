namespace CardGame.Core;

// Each entity qualification is scalar. Neither the event nor its committed
// projection can expose a mutable physical-material collection.
public sealed record MobileFunanUseEntityCapturedEvent(long ActionId, int ActorSeat, string SkillInstanceId,
    string GameplayHash, int CardId, long ClaimedActionId, long ClaimMovementSequence) : IGameEvent;
public sealed record MobileFunanDirectResponseObservedEvent(long ResponseActionId, long RespondedActionId, int ResponderSeat) : IGameEvent;
public sealed record MobileFunanDeferredCounterspellCompletion(CardActionContext Action,CardConversionSource Source,string GameplayHash);
public sealed record MobileFunanCounterspellCompletions(IReadOnlyList<MobileFunanDeferredCounterspellCompletion> Items,int Cursor=0,long? ActiveActionId=null);
public sealed partial record NullificationWindowFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public MobileFunanCounterspellCompletions? MobileFunanCompletions { get; init; }
}

internal sealed class MobileFunanCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "mobile:funan";
    public void ValidateTrigger(SkillProgramTrigger trigger) => MobileXueZongComposition.ValidateReward(trigger);
}

internal static class MobileXueZongComposition
{
    internal const string Funan = "mobile:funan",Reward = "obtained-use-unanswered",State = "mobile-response-exchange";
    internal static void ValidateReward(SkillProgramTrigger t)
    {
        if(t.Id!=Reward || t.Window!=SkillProgramTriggerWindow.CardUseCompleted || t.OwnerRelation!=SkillProgramCardActionOwnerRelation.Actor ||
            !t.IncludeResponseUses || !t.SingleActionInstance || t.Optional || t.IsSkillInvocation || t.Condition.Kind!=SkillProgramTriggerConditionKind.Always ||
            t.CardKinds.Count!=0 || t.CardCategories.Count!=0 || t.SourceSkillId is not null || t.Subject is not null || t.Priority!=0 ||
            t.UsageLimit is not null || t.UsageScope is not null || t.MarkerCost is not null || t.SourceZones.Count!=0 || t.DestinationZones.Count!=0 ||
            t.Effects is not [{Op:SkillProgramEffectOp.Draw,Target:SkillProgramEffectTarget.Owner,Amount:1,Condition.Kind:SkillProgramConditionKind.Always},
                {Op:SkillProgramEffectOp.AwaitBoundCardMovements,Target:SkillProgramEffectTarget.Owner,Condition.Kind:SkillProgramConditionKind.Always}])
            throw new InvalidOperationException("Mobile Funan needs exactly one native draw and its original paid movement return after an unanswered obtained-entity Use.");
    }
    [ProgramCatalogValidation(80800)]
    private static void Validate(string path,SkillProgram p)
    {
        if(p.Id is not(Funan or "mobile:jiexun")) return;
        if(p.Activations.Count!=0 || p.ViewAs.Count!=0 || p.CardPolicies.Count!=0 || p.BooleanStates.Count!=0 || p.Modifiers.Count!=0 ||
            p.Contributions.Count!=0 || p.CardIdentities.Count!=0 || p.DamageModifiers.Count!=0 || p.HpSkillGrants.Count!=0 || p.EquipmentSuitSkillGrants.Count!=0 || p.LordSkillProjection)
            throw new InvalidOperationException(path+": mobile Xue Zong uses only the response entity and escalating discard flows.");
        if(p.Id==Funan)
        {
            if(p.Triggers is not [{Id:"claim-response",Window:SkillProgramTriggerWindow.CardResponseAccepted,OwnerRelation:SkillProgramCardActionOwnerRelation.Observer,Optional:true,
                Effects:[{Op:SkillProgramEffectOp.ExchangeRespondedCardEntities,StateId:State,ResponseExchangeMode:ProgramResponseExchangeMode.ClaimResponseOnly}]},var reward])
                throw new InvalidOperationException(path+": mobile Funan must acquire responses directly at level one.");
            ValidateReward(reward);
        }
        else if(p.Triggers is not [{Id:"chosen-suit-escalating-discard",Window:SkillProgramTriggerWindow.TurnEnding,Subject:SkillProgramTriggerSubject.Owner,Optional:true,
            Effects:[{Op:SkillProgramEffectOp.SelectTarget,TargetKind:SkillProgramTargetKind.OtherLiving},
                {Op:SkillProgramEffectOp.DrawPublicSuitThenEscalatingDiscard,StateId:State,PublicSuitDiscardMode:ProgramPublicSuitDiscardMode.ChooseSuitCappedCurrentCountRetain} discard]}] ||
            !discard.SkillIds.SequenceEqual([Funan]))
            throw new InvalidOperationException(path+": mobile Jiexun requires all four suits, current-count discard, hand-only exhaustion and retained skill.");
    }
}
