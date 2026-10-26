using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ShanheTeamRankStage { Grant, Health, Draw, Next }

public sealed record ShanheTeamRankApplication
{
    public int Rank { get; }
    public bool EntryApplied { get; }
    public IReadOnlyList<int> Seats { get; }
    public int Index { get; init; }
    public ShanheTeamRankStage Stage { get; init; }
    public ShanheTeamRankApplication(int rank, bool entryApplied, IReadOnlyList<int> seats)
    { Rank = rank; EntryApplied = entryApplied; Seats = Array.AsReadOnly(seats.ToArray()); }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheTeamRankApplication? ShanheTeamRank { get; init; }
}

public sealed record ShanheTeamRankActivatedEvent(long FrameId, int Rank, bool EntryApplied) : IGameEvent;
public sealed record ShanheAlliedHealthAppliedEvent(long FrameId, int OwnerSeat, int RecipientSeat,
    int MaximumBefore, int MaximumAfter, int HpBefore, int HpAfter) : IGameEvent;

internal interface IShanheSchoolTeamHost
{
    SkillProgramStepOutcome ApplyTeamRank(ProgramSkillFrame frame);
    void ApplyAlliedHealth(ProgramSkillFrame frame);
}

internal abstract class ShanheTeamOpeningDescriptor : ProgramOperationDescriptorBase
{
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantSkills, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)];
}

internal sealed class ApplyShanheSchoolTeamRankDescriptor : ShanheTeamOpeningDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyShanheSchoolTeamRank;
    public override ISkillProgramEffectHandler Handler { get; } = new ApplyShanheSchoolTeamRankHandler();
}
internal sealed class ApplyShanheSchoolTeamRankHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyShanheSchoolTeamRank;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSchoolTeamHost>().ApplyTeamRank(frame);
}
internal sealed class ApplyShanheAlliedHealthBonusDescriptor : ShanheTeamOpeningDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyShanheAlliedHealthBonus;
    public override ISkillProgramEffectHandler Handler { get; } = new ApplyShanheAlliedHealthBonusHandler();
}
internal sealed class ApplyShanheAlliedHealthBonusHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyShanheAlliedHealthBonus;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IShanheSchoolTeamHost>().ApplyAlliedHealth(frame); return SkillProgramStepOutcome.Continue; }
}
