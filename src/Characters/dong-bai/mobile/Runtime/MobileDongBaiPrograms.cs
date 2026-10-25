using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record MobileLianzhuIssuedEvent(long FrameId, CardConversionSource Source, string Hash,
    SkillGrantPhaseExpiry Phase, int CardId, CardLocation From, int Recipient, int Before) : IGameEvent;
public sealed record MobileLianzhuGainProcessedEvent(long FrameId, long IssuedFrameId, int MovementSequence, int Holder) : IGameEvent;
public sealed record MobileLianzhuSegmentPaidEvent(long FrameId, MobileLianzhuStage Stage, int Actor, int? CardId,
    CardLocation? From, int? Recipient, long Before, long After, CardLocation? ActualTo = null) : IGameEvent;
public enum MobileLianzhuStage { Choice, SelectingGift, GiftChildren, PassChildren, Drawing, DrawChildren, RecoverChildren, InitialGiftChildren }
public sealed record MobileLianzhuReceipt(long IssuedFrameId, int CardId, int Holder, int GainSequence, MobileLianzhuStage Stage,
    int? PaymentCardId = null, CardLocation? PaymentFrom = null, int? Recipient = null, long Before = 0, long After = 0,
    NativeDrawOutcome? Draw = null, CardLocation? ActualTo = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileLianzhuReceipt? MobileLianzhu { get; init; }
}
internal interface IMobileDongBaiHost
{ SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect); }
internal abstract class MobileDongBaiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        if (Op != SkillProgramEffectOp.PrepareMobileDongBaiLianzhu) return ParseOwnerAlways(r);
        r.AllowOnly("op", "target", "condition", "sourceBind");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), sourceBind: r.OptionalIdentifier("sourceBind"));
        RequireAlways(effect, r.Path); return effect;
    }
    public override ISkillProgramEffectHandler Handler => new MobileDongBaiHandler(Op);
    public override ProgramOperationInteraction Interaction => Op == SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay
        ? ProgramOperationInteraction.Choice : ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => Op switch
    {
        SkillProgramEffectOp.PrepareMobileDongBaiLianzhu => e.SourceBind is { } bind
            ? [new ReadSelectedTarget(), new ReadSingleCardSet(bind),
                new MoveCardSet(bind, null, SkillProgramCardDestination.SelectedTargetHand)] : [new ReadSelectedTarget()],
        SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsGained)],
        _ => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)]
    };
}
internal sealed class PrepareMobileDongBaiLianzhuDescriptor : MobileDongBaiDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileDongBaiLianzhu; }
internal sealed class ResolveMobileDongBaiLianzhuRelayDescriptor : MobileDongBaiDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay; }
internal sealed class RecoverMobileDongBaiLianzhuDescriptor : MobileDongBaiDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecoverMobileDongBaiLianzhu; }
internal sealed class MobileDongBaiHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileDongBaiHost>().Execute(f, e);
}
internal static class MobileDongBaiComposition
{
    [ProgramCatalogValidation(79100)]
    private static void Validate(string path, SkillProgram p)
    {
        if (p.Id != "mobile:lianzhu") return;
        if (p.Activations is not [{ Id: "reveal-and-give", MinCards: 1, MaxCards: 1, MinTargets: 1, MaxTargets: 1,
            TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerTurn: null, UsesPerPhase: 1, SourceZones: [CardZoneKind.Hand, CardZoneKind.Equipment],
            Effects: [{ Op: SkillProgramEffectOp.PrepareMobileDongBaiLianzhu, SourceBind: null },
                { Op: SkillProgramEffectOp.CaptureSelectedCards, ResultBind: "lianzhu-card" },
                { Op: SkillProgramEffectOp.RevealBoundCards, SourceBind: "lianzhu-card" },
                { Op: SkillProgramEffectOp.PrepareMobileDongBaiLianzhu, SourceBind: "lianzhu-card" }] }] ||
            p.Triggers is not [{ Id: "gained-relay", Window: SkillProgramTriggerWindow.CardsGained, Subject: SkillProgramTriggerSubject.Owner,
                MovementOccurrence: SkillProgramMovementOccurrence.PerThirdPartyHandGain, Optional: false,
                Effects: [{ Op: SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay }] },
                { Id: "damage-recover", Window: SkillProgramTriggerWindow.AfterDamageApplied, Subject: SkillProgramTriggerSubject.DamageSource,
                    DamageOccurrence: SkillProgramDamageOccurrence.PerDamage, Optional: true, Effects: [{ Op: SkillProgramEffectOp.RecoverMobileDongBaiLianzhu }] }])
            throw new InvalidOperationException(path + ": mobile Lianzhu requires its exact native revealed HE gift, actual gained-card relay and optional outgoing-damage recovery.");
    }
}
