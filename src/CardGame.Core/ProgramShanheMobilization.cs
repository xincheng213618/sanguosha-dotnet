using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheMobilizationReceipt(int InstructionIndex, int RoundNumber, int TargetIndex, bool StatePublished)
{
    private readonly IReadOnlyList<int> _targetSeats = Array.Empty<int>();
    public IReadOnlyList<int> TargetSeats
    { get => _targetSeats; init => _targetSeats = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheMobilizationReceipt? ShanheMobilization { get; init; }
}
public sealed record ShanheMobilizationIssuedEvent(long FrameId, int InstructionIndex, CardConversionSource Source,
    int RoundNumber, string TargetSeatsKey) : IGameEvent;
public sealed record ShanheMobilizationRevivedEvent(long FrameId, int OwnerSeat, int TargetSeat, int RoundNumber, int Hp) : IGameEvent;

internal interface IShanheMobilizationHost
{
    SkillProgramStepOutcome ReviveRoundAllies(ProgramSkillFrame frame);
}
internal sealed class ReviveShanheRoundAlliesDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReviveShanheRoundAllies;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.RoundStarting)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Recover, static (_, _) => { });
    public override ISkillProgramEffectHandler Handler { get; } = new ReviveShanheRoundAlliesHandler();
}
internal sealed class ReviveShanheRoundAlliesHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ReviveShanheRoundAllies;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheMobilizationHost>().ReviveRoundAllies(frame);
}
internal static class ShanheMobilizationContract
{
    [ProgramCatalogValidation(62045)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ReviveShanheRoundAllies)) &&
            !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ReviveShanheRoundAllies))) return;
        if (program.Id != "shanhe:current-mobilization-2203" || program.Activations.Count != 0 || program.Triggers is not
            [{ Id: "next-round-allies", Window: SkillProgramTriggerWindow.RoundStarting, Subject: SkillProgramTriggerSubject.Owner,
               Optional: false, Effects: [{ Op: SkillProgramEffectOp.ReviveShanheRoundAllies, Target: SkillProgramEffectTarget.Owner,
                   Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": Mobilization requires its single mandatory owner round-start instruction.");
    }
}
