namespace CardGame.Core;

internal sealed class ShanheGuanchaoCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "shanhe:guanchao-3076";
    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        if (trigger.Window != SkillProgramTriggerWindow.CardUseCommitted ||
            trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor ||
            !trigger.IncludeResponseUses || trigger.Optional ||
            trigger.Condition.Kind != SkillProgramTriggerConditionKind.CardActionActorIsCurrentTurn ||
            trigger.Effects is not [{ Op: SkillProgramEffectOp.GuanchaoRankDraw,
                Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException("Shanhe Guanchao requires its exact own-turn committed-use declaration, including true response uses.");
    }
}
