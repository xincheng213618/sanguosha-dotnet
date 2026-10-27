using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class SourceExecutionerCapability(GameEngine e) : ISourceExecutionerHost
    {
        public SkillProgramStepOutcome Yishi(ProgramSkillFrame f) => e.ResolveSourceExecutionerYishi(f);
        public SkillProgramStepOutcome Huanfu(ProgramSkillFrame f) => e.BeginSourceExecutionerHuanfu(f);
    }

    private bool SourceExecutionerMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;
    private bool SourceYishiQualifies(int owner, ProgramSkillWindowContext context) => SourceExecutionerMode &&
        context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target } &&
        source == owner && _players[owner].IsAlive && _players[target].IsAlive &&
        CurrentDamageAttempt is { IsSourceLess: false, IsChainPropagation: false, EffectiveCardKind: { } kind } attack && IsSlashCard(kind) &&
        attack.SourceSeat == owner && attack.TargetSeat == target &&
        _resolutionStack.OfType<CardUseFrame>().Any(use => use.Id == attack.ResolutionId && use.SourceSeat == owner && IsSlashCard(use.CardKind) &&
            use.Action is { Type: CardActionType.Use } action && action.ActorSeat == owner && use.TargetSeats.Contains(target)) &&
        GetHand(_players[target]).Count + GetEquipment(_players[target]).Count + _cardZones.CardsAt(CardLocation.Judgment(target)).Count > 0;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62059, SkillProgramEffectOp.ResolveSourceExecutionerYishi)]
    private bool CanOfferSourceExecutionerYishi(ProgramTriggerLegalityContext gate) =>
        gate.SkillId == SourceExecutionerContract.Yishi && SourceYishiQualifies(gate.OwnerSeat, gate.Context);

    private SkillProgramStepOutcome ResolveSourceExecutionerYishi(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != SourceExecutionerContract.Yishi || f.TriggerId != "prevent-slash-and-take" || f.InstructionIndex != 1 ||
            f.WindowContext is not { } context || !SourceYishiQualifies(f.OwnerSeat, context) || _resolutionStack.Count < 2 ||
            _resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.Prevented ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].Candidate))
            throw new InvalidOperationException("Source611 Yishi requires its actual positive Slash damage and nonempty original target.");
        var attack = CurrentDamageAttempt!;
        PreventProgramCurrentDamage(f);
        AdvanceEventRulesAndQueueFact(new SourceExecutionerYishiPreventedEvent(f.Id, window.Id, attack.ResolutionId,
            f.OwnerSeat, window.TargetSeat, window.Amount));
        return SkillProgramStepOutcome.Continue;
    }
}
