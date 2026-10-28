using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SourceDingxiStage { Choose, EntryChildren, Issuing, NativeUse, Returned, StoreChildren, DrawChildren }
public sealed record SourceDingxiReceipt(long BatchId, long OriginalUseId, long OriginalActionId, int LeftSeat,
    IReadOnlyList<int> OriginalCardIds, SourceDingxiStage Stage, int CardId = 0, int DiscardSequence = 0,
    int EntrySequence = 0, long UseId = 0, long ActionId = 0, long Before = 0, long After = 0);
public sealed record SourceDingxiNativeOrigin(long ProgramFrameId, long OriginalUseId, long OriginalActionId,
    int OwnerSeat, int LeftSeat, int CardId, CardKind Kind, int DiscardSequence, int EntrySequence,
    string SkillInstanceId, string GameplayHash, long UseId, long ActionId);
public enum SourceHuojieStage { Judging, JudgmentChildren, DamageChildren, GainChildren }
public sealed record SourceHuojieReceipt(int MaximumJudgments, int Done, SourceHuojieStage Stage,
    long JudgmentId = 0, bool Hit = false, long Before = 0, long After = 0, int JudgmentCardId = 0, int JudgmentHandoffSequence = 0);
public enum SourceXingxiaStage { FriendlyDamage, EnemyChoice, PaymentChildren, EnemyDamage }
public sealed record SourceXingxiaReceipt(int Round, int FriendSeat, IReadOnlyList<int> Enemies, int Index,
    SourceXingxiaStage Stage, int PaymentCardId = 0, long Before = 0, long After = 0);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SourceDingxiReceipt? SourceDingxi { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SourceHuojieReceipt? SourceHuojie { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SourceXingxiaReceipt? SourceXingxia { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SourceDingxiNativeOrigin? SourceDingxiUse { get; init; }
}
public sealed record SourceDingxiEnteredEvent(long ProgramFrameId, int OwnerSeat, int CardId, int OriginalDiscardSequence, int EntrySequence) : IGameEvent;
public sealed record SourceDingxiDiscardQualifiedEvent(long BatchId, long OriginalUseId, long OriginalActionId, int OwnerSeat, int CardId, int DiscardSequence) : IGameEvent;
public sealed record SourceDingxiIssuedEvent(SourceDingxiNativeOrigin Origin) : IGameEvent;
public sealed record SourceDingxiReturnedEvent(long ProgramFrameId, long UseId, int CardId) : IGameEvent;
public sealed record SourceDingxiStoredEvent(long ProgramFrameId, int OwnerSeat, int CardId, int MovementSequence) : IGameEvent;
public sealed record SourceHuojieJudgmentIssuedEvent(long ProgramFrameId, long JudgmentId, int OwnerSeat, int Ordinal, int Maximum) : IGameEvent;
public sealed record SourceHuojieReceivedDamageEvent(long ProgramFrameId, long JudgmentId, int OwnerSeat, int Amount) : IGameEvent;
public sealed record SourceHuojieCompletedEvent(long ProgramFrameId, int OwnerSeat, int Judgments, bool ReceivedDamage) : IGameEvent;
public sealed record SourceXingxiaIssuedEvent(long ProgramFrameId, int OwnerSeat, string SkillInstanceId, int Round, int FriendSeat) : IGameEvent;
public sealed record SourceXingxiaPaymentEvent(long ProgramFrameId, int OwnerSeat, int PayerSeat, int CardId, int MovementSequence) : IGameEvent;

internal static class SourceBeigongIds
{
    internal const string Dingxi = "shanhe:source-dingxi-3601", Huojie = "shanhe:source-huojie-3603", Xingxia = "shanhe:source-xingxia-1117";
}
internal interface ISourceBeigongHost
{ SkillProgramStepOutcome Run(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class SourceBeigongDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler => new SourceBeigongHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeTrueUseProcessingEntry | ProgramTrackedFact.NativeLogicalDiscard);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(Op switch
    {
        SkillProgramEffectOp.ResolveSourceDingxiUsedCard => SkillProgramTriggerWindow.DiscardPileReceived,
        SkillProgramEffectOp.DrawSourceDingxiPile => SkillProgramTriggerWindow.TurnEnding,
        _ => SkillProgramTriggerWindow.PlayPhaseStarting
    })];
}
internal sealed class ResolveSourceDingxiUsedCardDescriptor : SourceBeigongDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceDingxiUsedCard; }
internal sealed class DrawSourceDingxiPileDescriptor : SourceBeigongDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawSourceDingxiPile; }
internal sealed class ResolveSourceHuojieDescriptor : SourceBeigongDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceHuojie; }
internal sealed class ResolveSourceXingxiaDescriptor : SourceBeigongDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveSourceXingxia; }
internal sealed class SourceBeigongHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) => host.GetCapability<ISourceBeigongHost>().Run(f, op);
}

internal static class SourceBeigongComposition
{
    [ProgramCatalogValidation(24510)]
    internal static void Validate(string path, SkillProgram p)
    {
        foreach (var t in p.Triggers.Where(t => t.Effects.Any(e => e.Op is SkillProgramEffectOp.ResolveSourceDingxiUsedCard or
                     SkillProgramEffectOp.DrawSourceDingxiPile or SkillProgramEffectOp.ResolveSourceHuojie or SkillProgramEffectOp.ResolveSourceXingxia)))
        {
            var correct = t.Subject == SkillProgramTriggerSubject.Owner && t.Condition.Kind == SkillProgramTriggerConditionKind.Always &&
                t.Effects.All(e => e.Target == SkillProgramEffectTarget.Owner && e.Condition.Kind == SkillProgramConditionKind.Always) &&
                (p.Id == SourceBeigongIds.Dingxi &&
                    (t.Window == SkillProgramTriggerWindow.DiscardPileReceived && t.Optional && t.SourceZones.SequenceEqual([CardZoneKind.Processing]) &&
                         t.MovementOccurrence == SkillProgramMovementOccurrence.PerBatch && t.Effects is [{ Op: SkillProgramEffectOp.ResolveSourceDingxiUsedCard }] ||
                     t.Window == SkillProgramTriggerWindow.TurnEnding && !t.Optional && t.Effects is [{ Op: SkillProgramEffectOp.DrawSourceDingxiPile }]) ||
                 p.Id == SourceBeigongIds.Huojie && t.Window == SkillProgramTriggerWindow.PlayPhaseStarting && !t.Optional &&
                     t.Effects is [{ Op: SkillProgramEffectOp.ResolveSourceHuojie }] ||
                 p.Id == SourceBeigongIds.Xingxia && t.Window == SkillProgramTriggerWindow.PlayPhaseStarting && !t.Optional &&
                     t.Effects is [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherPublicTeamAllies },
                         { Op: SkillProgramEffectOp.ResolveSourceXingxia }]);
            if (!correct || p.Activations.Count != 0) throw new InvalidOperationException(path + ": original Beigong source instructions require their exact owner lifecycle and payment domain.");
        }
    }
}
