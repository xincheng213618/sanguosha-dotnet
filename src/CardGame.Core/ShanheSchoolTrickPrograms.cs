using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

/// <summary>An extra effect belongs to the original paid use, independently of other extra-effect grants.</summary>
public sealed record ShanheSchoolTrickRound(long ActionId, long ProgramFrameId, long WindowFrameId,
    string GameplayHash, ExtraCardEffectState Native, bool Started = false, bool Active = false);

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSchoolTrickRound? ShanheSchoolTrick { get; init; }
}

public sealed record ShanheSchoolTrickIssuedEvent(long UseFrameId, long ActionId, long ProgramFrameId,
    long WindowFrameId, CardConversionSource Source, string GameplayHash) : IGameEvent;
public sealed record ShanheSchoolTrickStartedEvent(long UseFrameId, long ActionId, long ProgramFrameId) : IGameEvent;
public sealed record ShanheSchoolTrickCompletedEvent(long UseFrameId, long ActionId, long ProgramFrameId) : IGameEvent;

internal interface IShanheSchoolTrickHost
{
    void RepeatCurrentDamageTrick(ProgramSkillFrame frame);
}

internal sealed class RepeatCurrentDamageTrickDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentDamageTrick;
    public override ISkillProgramEffectHandler Handler { get; } = new RepeatCurrentDamageTrickHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted), new RequireCardActionActor()];
}

internal sealed class RepeatCurrentDamageTrickHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentDamageTrick;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.GetCapability<IShanheSchoolTrickHost>().RepeatCurrentDamageTrick(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
