using System.Text.Json.Serialization;

namespace CardGame.Core;

internal static class ShanheSourceJiangdongTrioIds
{
    internal const string Lihuo = "shanhe:source-lihuo-3160",Chunlao = "shanhe:source-chunlao-3161";
    internal const string FireTargetState = "fire-target-unlocked";
}

public sealed record ShanheSourceLihuoCostReceipt(long UseId, long DamageWindowId, long DamageId,
    bool Paid = false, int? FirstCardId = null, CardLocation? FirstFrom = null, int? SecondCardId = null,
    CardLocation? SecondFrom = null, int Before = 0, int After = 0, bool LostHp = false);
public sealed record ShanheSourceLihuoCostStartedEvent(long ProgramFrameId, long UseId, long DamageId,
    int OwnerSeat, string SkillInstanceId) : IGameEvent;
public sealed record ShanheSourceLihuoPaidEvent(long ProgramFrameId, long UseId, int OwnerSeat,
    int? FirstCardId, int? SecondCardId, bool LostHp) : IGameEvent;
public sealed record ShanheSourceChunlaoStorageReceipt(int CardId, int SourceSeat, int DiscardSequence,
    bool Paid = false, int StoreSequence = 0);
public sealed record ShanheSourceChunlaoStoredEvent(long ProgramFrameId, int OwnerSeat, int SourceSeat,
    int CardId, int DiscardSequence, int StoreSequence) : IGameEvent;
public sealed record ShanheSourceChunlaoJudgmentDrawReceipt(long AnchorWineUseId, int OwnerSeat,
    int ActualTurn, int OriginalDelayedEffects);
public sealed record ShanheSourceChunlaoJudgmentConsumedEvent(long WineUseId, long DrawFrameId,
    int OwnerSeat, int ActualTurn) : IGameEvent;
public sealed record ShanheSourceChunlaoJudgmentCompletedEvent(long DrawFrameId, long AnchorWineUseId,
    int OwnerSeat, int ActualTurn, int DrawCount) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceLihuoCostReceipt? ShanheSourceLihuoCost { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceChunlaoStorageReceipt? ShanheSourceChunlaoStorage { get; init; }
}

public interface IShanheSourceJiangdongTrioHost
{
    SkillProgramStepOutcome LihuoFirstDamage(ProgramSkillFrame frame);
    SkillProgramStepOutcome ChunlaoStorage(ProgramSkillFrame frame);
}

internal abstract class ShanheSourceJiangdongDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new ShanheSourceJiangdongHandler(Op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition());
        RequireAlways(e, r.Path); return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(Op == SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage
            ? SkillProgramTriggerWindow.AfterDamageApplied : SkillProgramTriggerWindow.DiscardPileReceived)];
}
internal sealed class ResolveShanheSourceLihuoFirstDamageDescriptor : ShanheSourceJiangdongDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.LoseHp,
        static (_, c) => c.LoseHp(new(SkillProgramEffectOp.LoseHp, SkillProgramEffectTarget.Owner, 1,
            new(SkillProgramConditionKind.Always, 0, []))));
}
internal sealed class OfferShanheSourceChunlaoStorageDescriptor : ShanheSourceJiangdongDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.OfferShanheSourceChunlaoStorage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ClaimMovedCards, static (_, _) => { });
}
internal sealed class ShanheSourceJiangdongHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) =>
        op == SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage
            ? host.GetCapability<IShanheSourceJiangdongTrioHost>().LihuoFirstDamage(f)
            : host.GetCapability<IShanheSourceJiangdongTrioHost>().ChunlaoStorage(f);
}

internal static class ShanheSourceJiangdongComposition
{
    [ProgramCatalogValidation(24920)]
    internal static void Validate(string path, SkillProgram p)
    {
        foreach (var t in p.Triggers)
        foreach (var e in t.Effects.Where(e => e.Op is SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage or SkillProgramEffectOp.OfferShanheSourceChunlaoStorage))
            if (t.Effects.Count != 1 || e.Target != SkillProgramEffectTarget.Owner || t.Optional ||
                e.Condition.Kind != SkillProgramConditionKind.Always || t.Condition.Kind != SkillProgramTriggerConditionKind.Always ||
                (e.Op == SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage
                    ? p.Id != ShanheSourceJiangdongTrioIds.Lihuo || t.Window != SkillProgramTriggerWindow.AfterDamageApplied ||
                      t.Subject != SkillProgramTriggerSubject.DamageSource || t.DamageOccurrence != SkillProgramDamageOccurrence.PerDamage
                    : p.Id != ShanheSourceJiangdongTrioIds.Chunlao || t.Window != SkillProgramTriggerWindow.DiscardPileReceived ||
                      t.Subject != SkillProgramTriggerSubject.Owner))
                throw new InvalidOperationException(path + ": source3160/3161 require their exact native damage or original-discard window.");
    }
}
