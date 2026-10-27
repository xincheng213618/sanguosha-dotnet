namespace CardGame.Core;

internal sealed class SourceMouZhibingCommittedContract : IProgramCommittedUseContract
{
    public string SkillId => "shanhe:source-zhibing-14208";
    public bool AllowsSingleActionInstance => true;
    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        if (trigger.Id != "other-actual-black-use" || trigger.Window != SkillProgramTriggerWindow.CardUseCommitted ||
            trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Observer || !trigger.IncludeResponseUses || !trigger.SingleActionInstance ||
            trigger.Optional || trigger.UsageLimit is not null || trigger.UsageScope is not null || trigger.CardKinds.Count != 0 || trigger.CardCategories.Count != 0 ||
            trigger.Condition.Kind != SkillProgramTriggerConditionKind.Always || trigger.Effects is not
                [{ Op: SkillProgramEffectOp.ResolveShanheSourceZhibing, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException("Source Zhibing requires its exact once-per-real-Use observer contract.");
    }
}
internal static class ShanheSourceMouContract
{
    private static bool Handles(SkillProgramEffectOp op) => op is SkillProgramEffectOp.ResolveShanheSourceXiongni or
        SkillProgramEffectOp.ResolveShanheSourceFengshang or SkillProgramEffectOp.ResolveShanheSourceZhibing;
    [ProgramCatalogValidation(62053)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => Handles(e.Op))) return;
        bool One(IReadOnlyList<SkillProgramEffect> e, SkillProgramEffectOp op) => e is
            [{ Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always } first] && first.Op == op;
        var valid = p.Modifiers.Count == 0 && p.CardPolicies.Count == 0 && p.DamageModifiers.Count == 0 && p.ViewAs.Count == 0 && p.BooleanStates.Count == 0 && p.HpSkillGrants.Count == 0;
        valid &= p.Id switch
        {
            "shanhe:source-xiongni-14206" => p.Activations.Count == 0 && p.Triggers is
                [{ Id: "five-suit-or-maximum", Window: SkillProgramTriggerWindow.PlayPhaseStarting, Subject: SkillProgramTriggerSubject.Owner,
                    Optional: true, UsageLimit: null, UsageScope: null } t] && One(t.Effects, SkillProgramEffectOp.ResolveShanheSourceXiongni),
            "shanhe:source-fengshang-14207" => p.Triggers is
                [{ Id: "dying-same-suit-allocation", Window: SkillProgramTriggerWindow.DyingEntering, Subject: SkillProgramTriggerSubject.Any,
                    Optional: true, UsageLimit: null, UsageScope: null } t] && One(t.Effects, SkillProgramEffectOp.ResolveShanheSourceFengshang) && p.Activations is
                [{ Id: "same-suit-allocation", MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0, UsesPerTurn: null, UsesPerPhase: 1 } a] &&
                    One(a.Effects, SkillProgramEffectOp.ResolveShanheSourceFengshang),
            "shanhe:source-zhibing-14208" => p.Activations.Count == 0 && p.Triggers.Count == 1,
            _ => false
        };
        if (!valid) throw new InvalidOperationException(path + ": source2684 requires its independent five-card, unrestricted dying gift and actual-Use programs.");
        if (p.Id == "shanhe:source-zhibing-14208") new SourceMouZhibingCommittedContract().ValidateTrigger(p.Triggers[0]);
    }
}
