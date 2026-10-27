using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheSourceZhaoranArmedEvent(long FrameId, CardConversionSource Source,
    int ActualTurn, int PlayInstance) : IGameEvent;
public sealed record ShanheSourceZhaoranSuitLostEvent(long ArmFrameId, int OwnerSeat, string SkillInstanceId,
    int ActualTurn, int PlayInstance, long BatchId, int MovementIndex, int MovementSequence, Suit Suit) : IGameEvent;
public sealed record ShanheSourceZhaoranSuitClaimedEvent(long FrameId, long ArmFrameId, int OwnerSeat,
    long BatchId, int MovementIndex, Suit Suit) : IGameEvent;

internal interface IShanheSourceZhaoranHost
{
    void Arm(ProgramSkillFrame frame);
    void ClaimSuit(ProgramSkillFrame frame);
}

internal abstract class ShanheSourceZhaoranDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new ShanheSourceZhaoranHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Reveal, static (_, _) => { });
}
internal sealed class ArmShanheSourceZhaoranDescriptor : ShanheSourceZhaoranDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ArmShanheSourceZhaoran;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayPhaseStarting)];
}
internal sealed class ClaimShanheSourceZhaoranSuitDescriptor : ShanheSourceZhaoranDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimShanheSourceZhaoranSuit;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
}
internal sealed class ShanheSourceZhaoranHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        var capability = host.GetCapability<IShanheSourceZhaoranHost>();
        if (op == SkillProgramEffectOp.ArmShanheSourceZhaoran) capability.Arm(frame);
        else capability.ClaimSuit(frame);
        return SkillProgramStepOutcome.Continue;
    }
}

public sealed partial class GameEngine
{
    private const string ShanheSourceZhaoranSkill = "shanhe:source-zhaoran-7014";
    private bool HasShanheSourceZhaoranRules => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmShanheSourceZhaoran);
    private bool IsShanheSourceZhaoranMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;

    private ShanheSourceZhaoranArmedEvent? ActiveShanheSourceZhaoranArm(int ownerSeat)
    {
        if (!HasShanheSourceZhaoranRules || !IsShanheSourceZhaoranMode || !IsValidPlayerSeat(ownerSeat) ||
            !_players[ownerSeat].IsAlive || _currentSeat != ownerSeat || _phase != TurnPhase.Play || _cardUseDebitPhaseInstanceId <= 0)
            return null;
        var arm = CompleteProgramEventHistory().OfType<ShanheSourceZhaoranArmedEvent>().LastOrDefault(e =>
            e.Source.OwnerSeat == ownerSeat && e.ActualTurn == _turnNumber && e.PlayInstance == _cardUseDebitPhaseInstanceId);
        return arm is not null && GetSkillBindingShard(_players[ownerSeat]).ProgramInstances.Any(instance =>
            instance.SkillId == ShanheSourceZhaoranSkill && instance.SkillInstanceId == arm.Source.SkillInstanceId) ? arm : null;
    }

    // The projector reads current Hand entities each time; cards gained later in
    // this actual Play are public too. Losing the source instance closes it now.
    private bool HasShanheSourceZhaoranPublicHand(int ownerSeat) => ActiveShanheSourceZhaoranArm(ownerSeat) is not null;

    private void ArmShanheSourceZhaoran(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourceZhaoranMode || frame.SkillId != ShanheSourceZhaoranSkill ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } context ||
            context.OwnerSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || _currentSeat != frame.OwnerSeat ||
            _phase != TurnPhase.Play || _cardUseDebitPhaseInstanceId <= 0 || ActiveShanheSourceZhaoranArm(frame.OwnerSeat) is not null)
            throw new InvalidOperationException("Source Zhaoran must arm once on its owner's exact actual Play entry.");
        AdvanceEventRulesAndQueueFact(new ShanheSourceZhaoranArmedEvent(frame.Id,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), _turnNumber, _cardUseDebitPhaseInstanceId));
    }

    // Called after the complete atomic movement batch, before any movement
    // children can draw, give or otherwise change the last-suit qualification.
    private void CaptureShanheSourceZhaoranSuitLosses(CardMovementBatchContext batch)
    {
        if (!HasShanheSourceZhaoranRules || ActiveShanheSourceZhaoranArm(_currentSeat) is not { } arm ||
            batch.TurnNumber != arm.ActualTurn || batch.MovementTiming is not { Phase: TurnPhase.Play } timing ||
            timing.PhaseActorSeat != arm.Source.OwnerSeat) return;
        var owner = _players[arm.Source.OwnerSeat];
        var losses = batch.Movements.Select((movement, index) => (movement, index))
            .Where(item => item.movement.From == CardLocation.Hand(owner.Seat) && item.movement.To != item.movement.From)
            .Select(item => (item.movement, item.index, suit: EffectiveSuit(owner, GetAdvancedCard(item.movement.CardId))))
            .Where(item => item.suit != Suit.None).GroupBy(item => item.suit).ToArray();
        foreach (var suitLosses in losses)
        {
            if (GetHand(owner).Any(card => EffectiveSuit(owner, card) == suitLosses.Key) ||
                CompleteProgramEventHistory().OfType<ShanheSourceZhaoranSuitLostEvent>().Any(e =>
                    e.OwnerSeat == owner.Seat && e.ActualTurn == arm.ActualTurn && e.PlayInstance == arm.PlayInstance && e.Suit == suitLosses.Key))
                continue;
            var first = suitLosses.First();
            AdvanceEventRulesAndQueueFact(new ShanheSourceZhaoranSuitLostEvent(arm.FrameId, owner.Seat, arm.Source.SkillInstanceId,
                arm.ActualTurn, arm.PlayInstance, batch.Id, first.index, first.movement.Sequence, suitLosses.Key));
        }
    }

    private ShanheSourceZhaoranSuitLostEvent? EligibleShanheSourceZhaoranSuit(int owner, string instanceId, ProgramSkillWindowContext? context)
    {
        if (ActiveShanheSourceZhaoranArm(owner) is not { } arm || arm.Source.SkillInstanceId != instanceId ||
            context is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch, MovementIndex: { } index } ||
            index < 0 || index >= batch.Movements.Count || batch.Movements[index].From != CardLocation.Hand(owner) ||
            batch.Movements[index].From == batch.Movements[index].To) return null;
        var loss = CompleteProgramEventHistory().OfType<ShanheSourceZhaoranSuitLostEvent>().SingleOrDefault(e =>
            e.ArmFrameId == arm.FrameId && e.OwnerSeat == owner && e.SkillInstanceId == instanceId &&
            e.BatchId == batch.Id && e.MovementIndex == index && e.MovementSequence == batch.Movements[index].Sequence);
        return loss is not null && !CompleteProgramEventHistory().OfType<ShanheSourceZhaoranSuitClaimedEvent>().Any(e =>
            e.ArmFrameId == loss.ArmFrameId && e.OwnerSeat == owner && e.Suit == loss.Suit) ? loss : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24661, SkillProgramEffectOp.ClaimShanheSourceZhaoranSuit)]
    private bool CanClaimShanheSourceZhaoranSuit(ProgramTriggerLegalityContext context) =>
        context.SkillId == ShanheSourceZhaoranSkill &&
        EligibleShanheSourceZhaoranSuit(context.OwnerSeat, context.Candidate.SkillInstanceId, context.WindowContext) is not null;

    private void ClaimShanheSourceZhaoranSuit(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var loss = EligibleShanheSourceZhaoranSuit(frame.OwnerSeat, frame.SkillInstanceId, frame.WindowContext);
        if (frame.SkillId != ShanheSourceZhaoranSkill || loss is null ||
            _resolutionStack.Count < 2 || _resolutionStack[^2] is not CardsMovedTriggerWindowFrame window ||
            frame.WindowContext?.ParentFrameId != window.Id || window.Batch.Id != loss.BatchId ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]))
            throw new InvalidOperationException("Source Zhaoran lost its exact first last-suit batch and original movement owner.");
        AdvanceEventRulesAndQueueFact(new ShanheSourceZhaoranSuitClaimedEvent(frame.Id, loss.ArmFrameId,
            frame.OwnerSeat, loss.BatchId, loss.MovementIndex, loss.Suit));
    }

    [ProgramCapability]
    private sealed class ShanheSourceZhaoranCapability(GameEngine engine) : IShanheSourceZhaoranHost
    {
        public void Arm(ProgramSkillFrame frame) => engine.ArmShanheSourceZhaoran(frame);
        public void ClaimSuit(ProgramSkillFrame frame) => engine.ClaimShanheSourceZhaoranSuit(frame);
    }
}
