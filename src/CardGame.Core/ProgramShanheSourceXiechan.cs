using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheSourceXiechanDuelReceipt(long PindianFrameId, int SourceCardId, int OpponentCardId,
    int SourceRank, int OpponentRank, bool SourceWon, int ActorSeat, int TargetSeat, long UseId);

public sealed partial record ProgramSelectedActorDuelOrigin
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceXiechanDuelReceipt? ShanheSourceXiechan { get; init; }
}
public sealed record ShanheSourceXiechanDuelIssuedEvent(long ProgramFrameId, long UseId, long PindianFrameId,
    int OwnerSeat, int ActorSeat, int TargetSeat, int SourceCardId, int OpponentCardId, bool SourceWon) : IGameEvent;

internal interface IShanheSourceXiechanHost
{
    SkillProgramStepOutcome ResolvePindianDuel(ProgramSkillFrame frame);
}
internal sealed class ResolveShanheSourceXiechanDuelDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceXiechanDuel;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheSourceXiechanDuelHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage,
        static (_, c) => c.Damage(new(SkillProgramEffectOp.Damage, SkillProgramEffectTarget.SelectedTarget, 1,
            new(SkillProgramConditionKind.Always, 0, []))));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new ReadSelectedTarget(), new ReadPindianResult("contest")];
}
internal sealed class ResolveShanheSourceXiechanDuelHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceXiechanDuel;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSourceXiechanHost>().ResolvePindianDuel(f);
}
internal static class ShanheSourceXiechanComposition
{
    internal const string Skill = "shanhe:source-xiechan-4055";
    [ProgramCatalogValidation(24630)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Activations.SelectMany(a => a.Effects).Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceXiechanDuel)) return;
        if (p.Id != Skill || p.Activations is not [{ Id: "limited-contest-duel", MinCards: 0, MaxCards: 0,
                MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLivingWithHand,
                UsesPerTurn: null, UsesPerPhase: null, UsesPerGame: 1 } a] ||
            a.Condition.Kind != SkillProgramConditionKind.HandCountAtLeast || a.Condition.Value != 1 ||
            a.Effects is not [{ Op: SkillProgramEffectOp.StartPindian, ResultBind: "contest", Visibility: SkillProgramCardSetVisibility.Public },
                { Op: SkillProgramEffectOp.ResolveShanheSourceXiechanDuel, Target: SkillProgramEffectTarget.Owner }] ||
            a.Effects.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always) || p.Triggers.Count != 0)
            throw new InvalidOperationException(path + ": source4055 requires its single limited actual-Play Pindian followed by a real directional Duel.");
    }
}
