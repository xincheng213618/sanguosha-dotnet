using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NianRevivalStage { DiscardChildren, StateChildren, DrawChildren, DrawAttempt }
public sealed record NianRevivalReceipt(int InstructionIndex, int TargetSeat, int UsedRevives,
    int MaximumHp, NianRevivalStage Stage, long Before, long After, int Count);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NianRevivalReceipt? NianRevival { get; init; }
}

// Scalar facts also serve as the shared, replayed payment ledger. A suspended
// operation owns its continuation on its ProgramSkillFrame, never on the team.
public sealed record NianRevivalPaidEvent(long FrameId, int ActorSeat, int TargetSeat, int UsedRevives,
    int MaximumHp) : IGameEvent;
public sealed record NianRevivalSegmentEvent(long FrameId, int TargetSeat, NianRevivalStage Stage,
    long Before, long After, int Count) : IGameEvent;
public sealed record NianAllyRevivedEvent(long FrameId, int ActorSeat, int TargetSeat, int UsedRevives,
    int Hp) : IGameEvent;
public sealed record NianRevivalCompletedEvent(long FrameId, int TargetSeat, int DrawnCards) : IGameEvent;
public sealed record NianRevivalCancelledEvent(long FrameId, int TargetSeat, string Reason) : IGameEvent;

internal interface INianRevivalHost
{
    SkillProgramStepOutcome ReviveNianAlly(ProgramSkillFrame frame);
}
internal interface INianRevivalSelectionQuery : IProgramSelectionQuery
{
    IReadOnlyList<int> DeadAllies(int ownerSeat);
}

internal sealed class ReviveNianAllyDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReviveNianAlly;
    public override ISkillProgramEffectHandler Handler { get; } = new ReviveNianAllyHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Recover,
        static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}
internal sealed class ReviveNianAllyHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ReviveNianAlly;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<INianRevivalHost>().ReviveNianAlly(frame);
}

internal static class NianRevivalComposition
{
    internal static void Validate(string path, IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerWindow? window, bool selectedTarget, int selectedCards, int targetSetMaximum,
        SkillProgramTargetKind? targetKind)
    {
        if (!effects.Any(e => e.Op == SkillProgramEffectOp.ReviveNianAlly) &&
            targetKind != SkillProgramTargetKind.NianDeadAlly) return;
        if (window is not null || !selectedTarget || selectedCards != 0 || targetSetMaximum != 0 ||
            targetKind != SkillProgramTargetKind.NianDeadAlly || effects is not
            [{ Op: SkillProgramEffectOp.ReviveNianAlly, Target: SkillProgramEffectTarget.Owner,
                Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException($"{path}: Nian revival is one zero-card, one-dead-ally activation.");
    }
}

[ProgramTargetResolver(SkillProgramTargetKind.NianDeadAlly)]
internal sealed class NianDeadAllyResolver : IProgramTargetResolver
{
    public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) =>
        queries.Get<INianRevivalSelectionQuery>().DeadAllies(context.OwnerSeat);
}

// A specialized source provider owns the exceptional dead target domain. The
// ordinary activation selector keeps its universal living-target restriction.
[ProgramSelectionModule(ProgramSelectionStage.Sources, 27930, SkillProgramEffectOp.ReviveNianAlly)]
internal sealed class NianRevivalSelectionSource : IProgramSelectionSourceProvider
{
    public bool TryBuild(ProgramSelectionContext context, ProgramSelectionCandidates candidates,
        IProgramSelectionQueries queries)
    {
        candidates.Cards = [];
        return true;
    }
    public void BuildTargets(ProgramSelectionContext context, ProgramSelectionCandidates candidates,
        IProgramSelectionQueries queries) =>
        candidates.Targets = queries.Get<INianRevivalSelectionQuery>().DeadAllies(context.OwnerSeat).ToArray();
}

public sealed partial class GameEngine
{

}
