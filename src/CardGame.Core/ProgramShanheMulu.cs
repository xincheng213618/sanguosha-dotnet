using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ShanheMuluRevivalStage { Cleanup, CleanupChildren, StateChildren }
public sealed record ShanheMuluRevivalReceipt(int InstructionIndex, int TargetIndex, ShanheMuluRevivalStage Stage)
{
    private readonly IReadOnlyList<int> _targetSeats = Array.Empty<int>();
    public IReadOnlyList<int> TargetSeats
    { get => _targetSeats; init => _targetSeats = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheMuluRevivalReceipt? ShanheMuluRevival { get; init; }
}
public sealed record ShanheMuluRevivalIssuedEvent(long FrameId, int InstructionIndex, CardConversionSource Source,
    string TargetSeatsKey) : IGameEvent;
public sealed record ShanheMuluFollowerRevivedEvent(long FrameId, int SummonerSeat, int TargetSeat, int Hp) : IGameEvent;
public sealed record ShanheElephantDamageFrozenEvent(long FrameId, CardConversionSource Source,
    int RoundNumber, int TurnNumber, int Amount) : IGameEvent;

internal interface IShanheMuluProgramHost
{
    SkillProgramStepOutcome ReviveFollowers(ProgramSkillFrame frame);
    void FreezeRoundDamage(ProgramSkillFrame frame);
}
internal sealed class ReviveShanheMuluFollowersDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReviveShanheMuluFollowers;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Recover, static (_, _) => { });
    public override ISkillProgramEffectHandler Handler { get; } = new ReviveShanheMuluFollowersHandler();
}
internal sealed class FreezeShanheElephantRoundDamageDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.FreezeShanheElephantRoundDamage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnCardDamageModifier, static (_, _) => { });
    public override ISkillProgramEffectHandler Handler { get; } = new FreezeShanheElephantRoundDamageHandler();
}
internal sealed class ReviveShanheMuluFollowersHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ReviveShanheMuluFollowers;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheMuluProgramHost>().ReviveFollowers(frame);
}
internal sealed class FreezeShanheElephantRoundDamageHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.FreezeShanheElephantRoundDamage;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IShanheMuluProgramHost>().FreezeRoundDamage(frame); return SkillProgramStepOutcome.Continue; }
}

internal static class ShanheMuluProgramContract
{
    [ProgramCatalogValidation(5420)]
    internal static void Validate(string path, SkillProgram program)
    {
        foreach (var op in new[] { SkillProgramEffectOp.ReviveShanheMuluFollowers, SkillProgramEffectOp.FreezeShanheElephantRoundDamage })
        {
            if (!program.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == op)) &&
                !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == op))) continue;
            var id = op == SkillProgramEffectOp.ReviveShanheMuluFollowers ? "shanhe:mulu-summon" : "shanhe:elephant-yangsheng";
            if (program.Id != id || program.Activations.Count != 0 || program.Triggers is not
                [{ Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Subject: SkillProgramTriggerSubject.Owner,
                   Optional: false, Effects.Count: 1 } trigger] || trigger.Effects[0].Op != op)
                throw new InvalidOperationException(path + ": Mulu and Yangsheng require their single mandatory actual owner-turn boundary.");
        }
    }
}
