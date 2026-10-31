using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IHellDizangProgramHost
{
    SkillProgramStepOutcome ExecuteHellDizang(ProgramSkillFrame frame, SkillProgramEffectOp operation);
}

internal abstract class HellDizangOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new HellDizangEffectHandler(Op);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
}

internal sealed class DamageHellBufoAdjacentDescriptor : HellDizangOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DamageHellBufoAdjacent;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage, static (_, _) => { });
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}

internal sealed class ChooseHellDayuanJudgmentDescriptor : HellDizangOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseHellDayuanJudgment;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.JudgmentBeforeResult)];
}

internal sealed class HellDizangEffectHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IHellDizangProgramHost>().ExecuteHellDizang(frame, Op);
}

public sealed record HellBufoReceipt(CardConversionSource Source, string GameplayHash, int InstructionIndex,
    int ActualTurnNumber, int NextTargetIndex)
{
    private IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}

public sealed record HellDayuanChoiceReceipt(CardConversionSource Source, string GameplayHash, int InstructionIndex,
    long WindowFrameId, long JudgmentFrameId, int CardId, Suit? SelectedSuit = null);
public sealed record HellDayuanJudgmentOverride(long ProgramFrameId, long JudgmentFrameId, CardConversionSource Source,
    string GameplayHash, int CardId, Suit Suit, int Rank);
public sealed record HellBufoTargetFrozenEvent(long ProgramFrameId, int OwnerSeat, int TargetSeat, int Index, int Total) : IGameEvent;
public sealed record HellDayuanJudgmentOverriddenEvent(HellDayuanJudgmentOverride Receipt) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HellBufoReceipt? HellBufo { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HellDayuanChoiceReceipt? HellDayuan { get; init; }
}

public sealed partial record JudgmentFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HellDayuanJudgmentOverride? HellDayuanOverride { get; init; }
}

public sealed partial class GameEngine
{
}
