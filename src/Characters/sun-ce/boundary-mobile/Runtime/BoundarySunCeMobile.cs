using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundarySunCeChallengeReceipt(int ProviderSeat, int ProviderCardId, bool Awakened, string Stage, long? PindianFrameId = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundarySunCeChallengeReceipt? BoundarySunCeChallenge { get; init; }
}
public sealed record BoundarySunCeChallengeStartedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    int ProviderSeat, int ProviderCardId, bool Awakened, int TurnNumber) : IGameEvent;
public sealed record BoundarySunCeChallengeAnsweredEvent(long FrameId, bool Accepted) : IGameEvent;
internal interface IBoundarySunCeMobileHost { SkillProgramStepOutcome Challenge(ProgramSkillFrame f); }
internal sealed class ResolveBoundarySunCeZhibaDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundarySunCeZhiba;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundarySunCeChallengeHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Pindian, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition", "skillIds");
        var skills = r.Has("skillIds") ? r.RequiredIdentifierArray("skillIds") : [];
        if (skills.Count > 1) throw new InvalidOperationException(r.Path + ": Zhiba accepts at most one awakening skill.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), skillIds: skills);
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadSelectedTarget()];

    [ProgramCatalogValidation(86800)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveBoundarySunCeZhiba)))
            throw new InvalidOperationException(path + ": Zhiba is supplied only through another Wu player's Play-phase activation.");
        foreach (var activation in program.Activations.Where(a => ProgramInstructionResolver.Default.Features(a).HasOperation(SkillProgramEffectOp.ResolveBoundarySunCeZhiba)))
            if (activation is not { MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLivingWithHand,
                Effects: [{ Op: SkillProgramEffectOp.ResolveBoundarySunCeZhiba, Target: SkillProgramEffectTarget.Owner }] })
                throw new InvalidOperationException(path + ": Zhiba requires its single native challenge instruction and one other hand-bearing target.");
    }
}
internal sealed class BoundarySunCeChallengeHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundarySunCeZhiba;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundarySunCeMobileHost>().Challenge(f);
}
// Zhiba is supplied to another Wu player's native Play phase, never an action
// the Lord may initiate against that player.
[ProgramSelectionModule(ProgramSelectionStage.Availability, 66300, SkillProgramEffectOp.ResolveBoundarySunCeZhiba)]
internal sealed class BoundarySunCeOwnChallengeUnavailable : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext c, ProgramSelectionCandidates p, IProgramSelectionQueries q) => false;
}
public sealed partial class GameEngine
{
}
