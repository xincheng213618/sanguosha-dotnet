using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum BoundaryXiahouDunStage { Selecting, StoreChildren, Distributing, GiveChildren, Drawing, DrawChildren }
public sealed record BoundaryXiahouDunMaterial(int CardId, CardKind Kind, CardLocation From);
public sealed record BoundaryXiahouDunReceipt(CardConversionSource Source, string Hash, SkillProgramEffectOp Op, long WindowId, int Turn)
{
    public BoundaryXiahouDunStage Stage { get; init; }
    public CardLocation? Location { get; init; }
    public int Given { get; init; }
    public int Target { get; init; } = -1;
    public long Before { get; init; }
    public long After { get; init; }
    public NativeDrawOutcome? Draw { get; init; }
    private IReadOnlyList<int> _selected = [];
    private IReadOnlyList<long> _deposits = [];
    private IReadOnlyList<BoundaryXiahouDunMaterial> _payment = [];
    public IReadOnlyList<int> Selected { get => _selected; init => _selected = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<long> Deposits { get => _deposits; init => _deposits = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<BoundaryXiahouDunMaterial> Payment { get => _payment; init => _payment = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryXiahouDunReceipt? BoundaryXiahouDun { get; init; }
}
// Only public counts and provenance are facts; concealed entity identities stay on the owning frame and native movements.
public sealed record BoundaryXiahouDunStoredEvent(long FrameId, CardConversionSource Source, string Hash, CardLocation Location, int Count, long Before, long After) : IGameEvent;
public sealed record BoundaryXiahouDunGivenEvent(long FrameId, int Owner, int Target, int Count, long Before, long After) : IGameEvent;
public sealed record BoundaryXiahouDunRewardEvent(long FrameId, int Owner, int Given, long? AttemptId, int Actual, long Before, long After) : IGameEvent;

internal interface IBoundaryXiahouDunHost { SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal sealed class BoundaryXiahouDunHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryXiahouDunHost>().Execute(frame, op);
}
internal abstract class BoundaryXiahouDunDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new BoundaryXiahouDunHandler(Op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
    public override ProgramDependencyKinds TracksActualTurnCursorReturn => ProgramDependencyKinds.Trigger;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
}
internal sealed class StoreBoundaryXiahouDunQingjianDescriptor : BoundaryXiahouDunDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian; }
internal sealed class DistributeBoundaryXiahouDunQingjianDescriptor : BoundaryXiahouDunDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian; }

internal static class BoundaryXiahouDunComposition
{
    [ProgramCatalogValidation(60010)]
    private static void Validate(string path, SkillProgram p)
    {
        var own = p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op is SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian or SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian);
        if (!own && p.Id != "boundary:qingjian-mobile") return;
        if (p.Id != "boundary:qingjian-mobile" || p.Activations.Count != 0 || p.Triggers is not [
            { Id: "store-after-gain", Window: SkillProgramTriggerWindow.CardsGained, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
                DestinationZones: [CardZoneKind.Hand], MovementOccurrence: SkillProgramMovementOccurrence.PerBatch, GainPhaseQualification: SkillProgramGainPhaseQualification.OutsideOwnerDraw,
                UsageScope: SkillUsageScope.Turn, UsageLimit: 1, Effects: [{ Op: SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian }] },
            { Id: "distribute-at-ending", Window: SkillProgramTriggerWindow.TurnEnding, TurnOwnerScope: SkillProgramTurnOwnerScope.AnyLiving, Optional: false,
                Effects: [{ Op: SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian }] }])
            throw new InvalidOperationException(path + ": mobile Qingjian requires its once-per-turn outside-own-draw storage and mandatory next ending distribution.");
    }
}
