using System.Text.Json.Serialization;

namespace CardGame.Core;
public enum HuoxinStage { TargetChoice, CardChoice, ObtainChoice, ObtainChildren }
public sealed record HuoxinReceipt(Suit EffectiveSuit)
{
    public HuoxinStage Stage { get; init; }
    public int TargetSeat { get; init; } = -1;
    private readonly IReadOnlyList<int> _candidates = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Candidates { get => _candidates; init => _candidates = Array.AsReadOnly(value.ToArray()); }
    public int? CardId { get; init; }
    public bool MayObtain { get; init; }
    public bool Paid { get; init; }
}
internal sealed class ResolveHuoxinNonEquipmentUseDescriptor() : LeZouShiDescriptor(SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse){
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance);
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
}
internal sealed class HuoxinCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "le:huoxin";
    public void ValidateTrigger(SkillProgramTrigger t)
    {
        if (t.Id != "non-equipment-use" || t.Window != SkillProgramTriggerWindow.CardUseCommitted ||
            t.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || !t.IncludeResponseUses ||
            !t.SingleActionInstance || !t.AllowNoEventTarget || !t.Optional || t.Subject is not null ||
            t.UsageScope != SkillUsageScope.Event || t.UsageLimit != 1 ||
            t.SourceSkillId is not null || t.CardKinds.Count != 0 || t.CardCategories.Count != 0 ||
            t.Effects is not [{ Op: SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException("Huoxin requires its optional complete non-equipment true-use actor contract, including native response uses.");
    }
}
