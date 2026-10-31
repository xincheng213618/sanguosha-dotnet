namespace CardGame.Core;

internal sealed class DuanWeiTaohuaiContract : IProgramCommittedUseContract
{
    public string SkillId => "ol:taohuai";

    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        var draw = trigger.Id == "extreme-hand-use-draw" && !trigger.Optional && trigger.Effects is
            [{ Op: SkillProgramEffectOp.ToggleBooleanState, StateId: "yin", Target: SkillProgramEffectTarget.Owner },
             { Op: SkillProgramEffectOp.Draw, Amount: 1, Target: SkillProgramEffectTarget.Owner,
               NumberExpression: null, SourceBind: null, ResultBind: null, TargetReference: null }];
        var discard = trigger.Id == "non-extreme-hand-use-discard" && trigger.Optional && trigger.Effects is
            [{ Op: SkillProgramEffectOp.ToggleBooleanState, StateId: "yin", Target: SkillProgramEffectTarget.Owner },
             { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Amount: 1, Destination: SkillProgramCardDestination.DiscardPile,
               ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef.Kind: ProgramParticipantRef.Owner,
               Zones: [CardZoneKind.Hand, CardZoneKind.Equipment], AwaitMovementTriggers: true }];
        if (trigger.Window != SkillProgramTriggerWindow.CardUseCommitted ||
            trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || !trigger.IncludeResponseUses ||
            trigger.Subject is not null || trigger.SourceSkillId is not null || trigger.SourceViewAsId is not null ||
            trigger.UsageScope is not null || trigger.UsageLimit is not null || trigger.DynamicUsageLimit is not null ||
            trigger.NamedUsageGroup is not null || trigger.MarkerCost is not null ||
            trigger.CardKinds.Count != 0 || trigger.CardCategories.Count != 0 || trigger.RequireNoCardConversion is not null ||
            trigger.Condition.Kind != SkillProgramTriggerConditionKind.Always ||
            trigger.Effects.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always) || !draw && !discard)
            throw new InvalidOperationException("Taohuai requires its actual hand-use draw or optional discard conversion binding.");
    }
}
