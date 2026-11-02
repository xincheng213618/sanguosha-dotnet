using System.Text.Json.Serialization;
using AcquisitionNode = CardGame.Core.ProgramStageNode<CardGame.Core.ProgramSeatAcquisitionCall, CardGame.Core.ProgramSeatAcquisitionReceipt, CardGame.Core.ProgramSeatAcquisitionStage>;
namespace CardGame.Core;

public enum ProgramSeatAcquisitionMode { PrioritizedRegion, AllRegionsConditionalTurnover }
public enum ProgramSeatAcquisitionStage { PriorityChoice, Visiting, MovementChildren, TurnoverChildren, Finished }
public sealed record ProgramSeatAcquisitionCandidate(int CardId, CardLocation From);
public sealed record ProgramSeatAcquisitionPayment(int Cursor, int TargetSeat, int CardId, CardLocation From,
    long? BatchId, long Before, long After, uint RandomBefore, uint RandomAfter)
{
    private readonly IReadOnlyList<ProgramSeatAcquisitionCandidate> _candidates = Array.AsReadOnly(Array.Empty<ProgramSeatAcquisitionCandidate>());
    private readonly IReadOnlyList<CardMovementRecord> _movements = Array.AsReadOnly(Array.Empty<CardMovementRecord>());
    public IReadOnlyList<ProgramSeatAcquisitionCandidate> Candidates { get => _candidates; init => _candidates = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardMovementRecord> Movements { get => _movements; init => _movements = Array.AsReadOnly(value.ToArray()); }
}
public sealed record ProgramSeatAcquisitionReceipt(ProgramSeatAcquisitionMode Mode, CardConversionSource Source, string GameplayHash,
    long DamageWindowId, long DamageFrameId, int OccurrenceIndex, int DamageAmount)
{
    private readonly IReadOnlyList<int> _seats = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<ProgramSeatAcquisitionPayment> _payments = Array.AsReadOnly(Array.Empty<ProgramSeatAcquisitionPayment>());
    public IReadOnlyList<int> OtherSeats { get => _seats; init => _seats = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<ProgramSeatAcquisitionPayment> Payments { get => _payments; init => _payments = Array.AsReadOnly(value.Select(p => p with
        { Candidates = p.Candidates, Movements = p.Movements }).ToArray()); }
    public int Cursor { get; init; }
    public CardZoneKind? PreferredZone { get; init; }
    public ProgramSeatAcquisitionStage Stage { get; init; }
    public bool TurnoverIssued { get; init; }
    public bool? FaceDownBefore { get; init; }
    public bool? FaceDownAfter { get; init; }
}
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ProgramSeatAcquisitionReceipt? SeatAcquisition { get; init; } }

internal sealed record ProgramSeatAcquisitionDamagePoint(long WindowId, long DamageFrameId, int OccurrenceIndex, int Amount);
internal interface IProgramSeatAcquisitionFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    bool IsAlive(int seat);
    bool IsFaceDown(int seat);
    bool CannotTurnOver(int seat);
    bool CanInvokeProgramSkill(ProgramSkillFrame frame);
    uint RandomState { get; }
    long MovementSequence { get; }
    ProgramSeatAcquisitionDamagePoint? DamagePoint(ProgramSkillFrame frame);
    IReadOnlyList<int> OtherSeats(int owner);
    IReadOnlyList<ProgramSeatAcquisitionCandidate> Candidates(ProgramSkillFrame frame, int target);
}
internal interface IProgramSeatAcquisitionOperations : IProgramRuntimeOperations
{
    int NextRandom(int count);
    void Obtain(long producerFrameId, int cardId, int recipient, CardMoveReason reason, CardMovementDisclosure disclosure,
        Action<long?, IReadOnlyList<CardMovementRecord>> settled);
    bool DrainChildren(ProgramSkillFrame frame);
    void TurnOver(ProgramSkillFrame frame);
    void Finish(ProgramSkillFrame frame);
}
// Policies project the original scalar events and the rule's actual differences.
// They are local adapters, not independently registered capabilities.
internal interface IProgramSeatAcquisitionPolicy
{
    ProgramSeatAcquisitionMode Mode { get; }
    string SkillId { get; }
    string Reason { get; }
    void PublishPriority(ProgramSkillFrame frame);
    bool ShouldTurnOver(ProgramSkillFrame frame, ProgramSeatAcquisitionReceipt receipt, bool before);
    IGameEvent Started(ProgramSkillFrame frame, ProgramSeatAcquisitionReceipt receipt);
    IGameEvent Skipped(ProgramSkillFrame frame, int cursor, int target);
    IGameEvent Paid(ProgramSkillFrame frame, ProgramSeatAcquisitionPayment payment);
    IGameEvent Turned(ProgramSkillFrame frame, int obtained, bool before, bool after);
    IGameEvent Completed(ProgramSkillFrame frame, ProgramSeatAcquisitionReceipt receipt, bool cancelled);

}
// A synchronous invocation context contains only the coordinator and its policy.
// It is never retained between Run calls; all pending data is SeatAcquisition.
internal sealed record ProgramSeatAcquisitionCall(ProgramRandomSeatAcquisitionRuntime Runtime, IProgramSeatAcquisitionPolicy Policy);

internal interface IProgramSeatAcquisitions : IProgramRuntimeOperations
{
    SkillProgramStepOutcome Begin(ProgramSkillFrame frame, IProgramSeatAcquisitionPolicy policy);
    bool Resume(long id, IProgramSeatAcquisitionPolicy policy);
    bool RetainPaid(ProgramSkillFrame frame, IProgramSeatAcquisitionPolicy policy);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramRandomSeatAcquisitionRuntime(IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    IProgramRuntimeEvents events, IProgramSeatAcquisitionFacts facts, IProgramSeatAcquisitionOperations operations) : IProgramSeatAcquisitions
{
    internal static IReadOnlyList<CardZoneKind> Priority(CardZoneKind preferred) => preferred switch
    {
        CardZoneKind.Hand => [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment],
        CardZoneKind.Equipment => [CardZoneKind.Equipment, CardZoneKind.Hand, CardZoneKind.Judgment],
        CardZoneKind.Judgment => [CardZoneKind.Judgment, CardZoneKind.Hand, CardZoneKind.Equipment],
        _ => throw new InvalidOperationException("A seat acquisition requires one HEJ priority region.")
    };
    internal static int MovedSeats(ProgramSeatAcquisitionReceipt receipt) => receipt.Payments.Count(p => p.Movements.Count > 0);
    internal static int ActualObtained(ProgramSkillFrame f) => f.SeatAcquisition!.Payments.Sum(p =>
        p.Movements.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)));
    private static CardConversionSource Source(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    public SkillProgramStepOutcome Begin(ProgramSkillFrame supplied, IProgramSeatAcquisitionPolicy policy)
    {
        var f = frames.ActiveProgram(supplied.Id); var origin = facts.DamagePoint(f);
        if (f.SeatAcquisition is not null || f.SkillId != policy.SkillId || f.InstructionIndex != 1 ||
            f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || origin is null)
            throw new InvalidOperationException("Seat acquisition requires its exact original positive damage-point candidate.");
        if (ShouldCancel(f, 0)) return SkillProgramStepOutcome.Continue;
        var r = new ProgramSeatAcquisitionReceipt(policy.Mode, Source(f), f.GameplayHash, origin.WindowId, origin.DamageFrameId, origin.OccurrenceIndex, origin.Amount)
        { OtherSeats = facts.OtherSeats(f.OwnerSeat), Stage = policy.Mode == ProgramSeatAcquisitionMode.PrioritizedRegion ? ProgramSeatAcquisitionStage.PriorityChoice : ProgramSeatAcquisitionStage.Visiting };
        frames.ReplaceTop(f = f with { SeatAcquisition = r });
        events.AdvanceFact(policy.Started(f, r)); Resume(f.Id, policy); return SkillProgramStepOutcome.AwaitChild;
    }
    public bool Resume(long id, IProgramSeatAcquisitionPolicy policy)
    {
        if (frames.Top is not ProgramSkillFrame { SeatAcquisition: { } r } f || f.Id != id || r.Mode != policy.Mode || f.SkillId != policy.SkillId) return false;
        ProgramStageRuntime.Run(id, frames, prompts, new ProgramSeatAcquisitionCall(this, policy), Stages.Plan); return true;
    }
    private static class Stages
    {
        internal static readonly ProgramStagePlan<ProgramSeatAcquisitionCall, ProgramSeatAcquisitionReceipt, ProgramSeatAcquisitionStage> Plan = new(
            static f => f.SeatAcquisition, static r => r.Stage,
            [AcquisitionNode.Prompt(ProgramSeatAcquisitionStage.PriorityChoice, static (c, f, _) => c.Policy.PublishPriority(f)),
             AcquisitionNode.Run(ProgramSeatAcquisitionStage.Visiting, static (c, f, r) => c.Runtime.Visit(f, r, c.Policy)),
             AcquisitionNode.ChildrenTo(ProgramSeatAcquisitionStage.MovementChildren, ProgramSeatAcquisitionStage.Visiting,
                static r => r with { Cursor = r.Cursor + 1 }, clearMovement: true),
             AcquisitionNode.ChildrenComplete(ProgramSeatAcquisitionStage.TurnoverChildren),
             AcquisitionNode.AwaitExternalReturn(ProgramSeatAcquisitionStage.Finished)],
            stopBeforeStage: static (c, f, r) => c.Runtime.Stop(f, r, c.Policy),
            withStage: static (f, r, stage) => f with { SeatAcquisition = r with { Stage = stage } },
            drain: static (c, f, _) => c.Runtime.Drain(f),
            complete: static (c, f) => c.Runtime.Finish(f, false, c.Policy));
    }
    private bool ShouldCancel(ProgramSkillFrame f, int paid) => facts.Winner != Winner.None || !facts.IsAlive(f.OwnerSeat) ||
        paid == 0 && !facts.CanInvokeProgramSkill(f);
    private bool Drain(ProgramSkillFrame f) => operations.DrainChildren(f);
    private bool Stop(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, IProgramSeatAcquisitionPolicy policy)
    {
        if (r.Stage is not (ProgramSeatAcquisitionStage.PriorityChoice or ProgramSeatAcquisitionStage.Visiting) || !ShouldCancel(f, r.Payments.Count)) return false;
        Finish(f, true, policy); return true;
    }
    private static IReadOnlyList<ProgramSeatAcquisitionCandidate> RandomPool(ProgramSeatAcquisitionReceipt r, IReadOnlyList<ProgramSeatAcquisitionCandidate> candidates)
    {
        if (r.Mode != ProgramSeatAcquisitionMode.PrioritizedRegion) return candidates;
        var zone = Priority(r.PreferredZone!.Value).First(z => candidates.Any(c => c.From.Zone == z));
        return candidates.Where(c => c.From.Zone == zone).ToArray();
    }
    private ProgramStageResult Visit(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, IProgramSeatAcquisitionPolicy policy)
    {
        if (r.Cursor == r.OtherSeats.Count)
        {
            var before = facts.IsFaceDown(f.OwnerSeat);
            if (!policy.ShouldTurnOver(f, r, before)) { Finish(f, false, policy); return ProgramStageResult.Yield; }
            ProgramStageRuntime.Issue(frames, f with { SeatAcquisition = r with { Stage = ProgramSeatAcquisitionStage.TurnoverChildren,
                TurnoverIssued = true, FaceDownBefore = before, FaceDownAfter = facts.CannotTurnOver(f.OwnerSeat) ? before : !before } }, committed =>
            {
                operations.TurnOver(committed);
                events.AdvanceFact(policy.Turned(committed, ActualObtained(committed), before, facts.IsFaceDown(committed.OwnerSeat)));
            });
            return ProgramStageResult.Continue;
        }
        var target = r.OtherSeats[r.Cursor]; var candidates = facts.IsAlive(target) ? facts.Candidates(f, target) : [];
        if (candidates.Count == 0)
        {
            frames.ReplaceTop(f with { SeatAcquisition = r with { Cursor = r.Cursor + 1 } });
            events.AdvanceFact(policy.Skipped(f, r.Cursor, target)); return ProgramStageResult.Continue;
        }
        var pool = RandomPool(r, candidates); var randomBefore = facts.RandomState;
        var selected = pool[operations.NextRandom(pool.Count)]; var randomAfter = facts.RandomState; var beforeMove = facts.MovementSequence;
        ProgramStageRuntime.Issue(frames, f with { SeatAcquisition = r with { Stage = ProgramSeatAcquisitionStage.MovementChildren },
            PendingMovementContinuation = new(f.OwnerSeat, 0, null) }, _ =>
            operations.Obtain(f.Id, selected.CardId, f.OwnerSeat, new(policy.Reason),
                selected.From.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp, (batch, records) =>
                {
                    var parent = frames.ActiveProgram(f.Id);
                    var payment = new ProgramSeatAcquisitionPayment(r.Cursor, target, selected.CardId, selected.From, batch, beforeMove,
                        facts.MovementSequence, randomBefore, randomAfter) { Candidates = candidates, Movements = records };
                    frames.ReplaceTop(parent = parent with { SeatAcquisition = parent.SeatAcquisition! with { Payments = parent.SeatAcquisition.Payments.Append(payment).ToArray() } });
                    // A destination replacement can settle the random request
                    // without moving its entity. Keep its RNG/cursor receipt, but
                    // publish physical payment only for actual movement records.
                    if (records.Count > 0) events.AdvanceFact(policy.Paid(parent, payment));
                }));
        return ProgramStageResult.Continue;
    }
    private void Finish(ProgramSkillFrame f, bool cancelled, IProgramSeatAcquisitionPolicy policy)
    {
        var r = f.SeatAcquisition!;
        if (f.PendingMovementContinuation is not null || r.Stage == ProgramSeatAcquisitionStage.MovementChildren)
            throw new InvalidOperationException("Seat acquisition cannot retire a paid gain before its children return.");
        frames.ReplaceTop(f = f with { SeatAcquisition = r with { Stage = ProgramSeatAcquisitionStage.Finished } });
        events.AdvanceFact(policy.Completed(f, f.SeatAcquisition!, cancelled)); operations.Finish(f);
    }
    public bool RetainPaid(ProgramSkillFrame f, IProgramSeatAcquisitionPolicy policy) =>
        f.SkillId == policy.SkillId && f.SeatAcquisition is { } r && r.Mode == policy.Mode &&
        (r.Payments.Count > 0 || r.TurnoverIssued);
}
