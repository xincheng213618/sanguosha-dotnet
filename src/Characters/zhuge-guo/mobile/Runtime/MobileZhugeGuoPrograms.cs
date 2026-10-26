using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record MobileQirangGainReceipt(CardConversionSource Source, string Hash, long EquipmentBatchId, long EquipmentSequence,
    int EquipmentCardId, int CardId, CardKind Kind, long Before, long After, long GainSequence);
public sealed record MobileQirangGainPaidEvent(long FrameId, MobileQirangGainReceipt Receipt) : IGameEvent;
public sealed record MobileQirangUseReceipt(MobileQirangGainReceipt Gain, long UseId, CardActionContext Action, long MovementBefore);
public sealed record MobileQirangUseIssuedEvent(long UseId, long ActionId, int OwnerSeat, int CardId, long GainSequence, long MovementBefore) : IGameEvent;
public sealed record MobileYuhuaPeekStartedEvent(long FrameId, CardConversionSource Source, string Hash, long EndingId, int HandCount, int Hp, int Requested, int ActualCount) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileQirangGainReceipt? MobileQirangGain { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileQirangUseReceipt? MobileQirangUse { get; init; }
}
internal interface IMobileZhugeGuoHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class MobileZhugeGuoDescriptor(SkillProgramEffectOp op) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => op;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new MobileZhugeGuoHandler(op);
    public override ProgramOperationInteraction Interaction => op == SkillProgramEffectOp.InspectMobileYuhuaTop ? ProgramOperationInteraction.Choice : ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(op == SkillProgramEffectOp.ObtainMobileQirangTrick ? SkillProgramTriggerWindow.CardsGained : SkillProgramTriggerWindow.TurnEnding)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class ObtainMobileQirangTrickDescriptor() : MobileZhugeGuoDescriptor(SkillProgramEffectOp.ObtainMobileQirangTrick);
internal sealed class InspectMobileYuhuaTopDescriptor() : MobileZhugeGuoDescriptor(SkillProgramEffectOp.InspectMobileYuhuaTop);
internal sealed class MobileZhugeGuoHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileZhugeGuoHost>().Resolve(f, op);
}
internal sealed class MobileQirangCommittedContract : IProgramCommittedUseContract
{
    public string SkillId => "mobile:qirang";
    public bool AllowsSingleActionInstance => true;
    public void ValidateTrigger(SkillProgramTrigger t)
    {
        if (t.Id != "acquired-trick-use" || t.Window != SkillProgramTriggerWindow.CardUseCommitted || t.Optional ||
            t.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || !t.IncludeResponseUses || !t.SingleActionInstance ||
            t.Subject is not null || t.Condition.Kind != SkillProgramTriggerConditionKind.Always || t.CardCategories is not [SkillProgramCardCategory.Trick] ||
            t.UsageScope is not null || t.UsageLimit is not null || t.MarkerCost is not null ||
            t.Effects is not [{ Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1, NumberExpression: null,
                SourceBind: null, ResultBind: null, TargetReference: null, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException("Mobile Qirang requires exactly one actual-use owner draw, including actual Nullification response uses.");
    }
}
internal static class MobileZhugeGuoComposition
{
    [ProgramCatalogValidation(81800)]
    private static void Validate(string path, SkillProgram p)
    {
        if (p.Id == "mobile:qirang")
        {
            if (p.Activations.Count != 0 || p.Triggers.Count != 2 || p.Triggers.SingleOrDefault(t => t.Id == "equipment-entry") is not
                { Window: SkillProgramTriggerWindow.CardsGained, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
                  MovementOccurrence: SkillProgramMovementOccurrence.PerCard, DestinationZones: [CardZoneKind.Equipment],
                  Effects: [{ Op: SkillProgramEffectOp.ObtainMobileQirangTrick, Target: SkillProgramEffectTarget.Owner }] })
                throw new InvalidOperationException(path + ": mobile Qirang requires actual per-entity equipment entry.");
            new MobileQirangCommittedContract().ValidateTrigger(p.Triggers.Single(t => t.Id == "acquired-trick-use"));
        }
        if (p.Id == "mobile:yuhua" && p.Triggers.SingleOrDefault(t => t.Id == "ending-category-top") is not
            { Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner, Optional: false,
              Effects: [{ Op: SkillProgramEffectOp.InspectMobileYuhuaTop, Target: SkillProgramEffectTarget.Owner }] })
            throw new InvalidOperationException(path + ": mobile Yuhua requires its actual own ending top arrangement.");
    }
}
