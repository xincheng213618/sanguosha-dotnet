using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IHulaoActivityProgramHost
{
    SkillProgramStepOutcome ExecuteHulaoActivity(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal abstract class HulaoActivityOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new HulaoActivityEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
}

internal sealed class ResolveHulaoXiuluoDescriptor : HulaoActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveHulaoXiuluo;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}

internal sealed class OfferHulaoDamagePeachDescriptor : HulaoActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.OfferHulaoDamagePeach;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}

internal sealed class PreventHulaoArmoredLargeDamageDescriptor : HulaoActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PreventHulaoArmoredLargeDamage;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
}

internal sealed class SkipHulaoTreasureJudgmentDescriptor : HulaoActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SkipHulaoTreasureJudgment;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}

internal sealed class BindPublicAllyDeathDescriptor : HulaoActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BindPublicAllyDeath;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind"));
        RequireAlways(e, r.Path); return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CharacterDied), new CreateChoiceResult(effect.ResultBind!, ["ally", "other"])];
}

internal sealed class HulaoActivityEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IHulaoActivityProgramHost>().ExecuteHulaoActivity(frame, effect);
}

public enum HulaoXiuluoStage { Offering, CostChildren, JudgmentOffering, JudgmentChildren, Complete }
public sealed record HulaoXiuluoDraft(int InstructionIndex, CardConversionSource Source, string GameplayHash,
    int ActualTurnNumber, long LifecycleFrameId, HulaoXiuluoStage Stage = HulaoXiuluoStage.Offering,
    int Iteration = 0, int? CostCardId = null, CardLocation? CostFrom = null, Suit PaidSuit = Suit.None, int? JudgmentCardId = null);
public enum HulaoPeachStage { Offering, Using, Children, Complete }
public sealed record HulaoPeachDraft(int InstructionIndex, CardConversionSource Source, string GameplayHash,
    int ActualTurnNumber, long DamageFrameId, HulaoPeachStage Stage = HulaoPeachStage.Offering,
    int? CardId = null, long? CardUseFrameId = null);
public sealed record HulaoPeachOrigin(long ProgramFrameId, HulaoPeachDraft Issuance);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HulaoXiuluoDraft? HulaoXiuluo { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HulaoPeachDraft? HulaoPeach { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HulaoPeachOrigin? HulaoPeachOrigin { get; init; }
}
public sealed record HulaoXiuluoPaidEvent(long ProgramFrameId, int OwnerSeat, int Iteration, int CardId, Suit EffectiveSuit) : IGameEvent;
public sealed record HulaoXiuluoJudgmentDiscardedEvent(long ProgramFrameId, int OwnerSeat, int Iteration, int CardId) : IGameEvent;
public sealed record HulaoPeachIssuedEvent(HulaoPeachOrigin Origin) : IGameEvent;
public sealed record HulaoPeachReturnedEvent(HulaoPeachOrigin Origin) : IGameEvent;
