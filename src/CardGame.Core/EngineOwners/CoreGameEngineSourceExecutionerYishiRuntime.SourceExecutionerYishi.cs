#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceExecutionerYishiRuntime
{
    SkillProgramStepOutcome ISourceExecutionerHost.Yishi(ProgramSkillFrame f) => ResolveSourceExecutionerYishi(f);
    SkillProgramStepOutcome ISourceExecutionerHost.Huanfu(ProgramSkillFrame f) => _coreSourceExecutionerHuanfuOperations.BeginSourceExecutionerHuanfu(f);
    internal bool SourceExecutionerMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    internal bool SourceYishiQualifies(int owner, ProgramSkillWindowContext context) => SourceExecutionerMode && context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target } && source == owner && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, IsChainPropagation: false, EffectiveCardKind: { } kind } attack && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.SourceSeat == owner && attack.TargetSeat == target && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(use => use.Id == attack.ResolutionId && use.SourceSeat == owner && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.Action is { Type: CardActionType.Use } action && action.ActorSeat == owner && use.TargetSeats.Contains(target)) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count + _cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(target)).Count > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62059, SkillProgramEffectOp.ResolveSourceExecutionerYishi)]
    internal bool CanOfferSourceExecutionerYishi(ProgramTriggerLegalityContext gate) => gate.SkillId == SourceExecutionerContract.Yishi && SourceYishiQualifies(gate.OwnerSeat, gate.Context);
    internal SkillProgramStepOutcome ResolveSourceExecutionerYishi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != SourceExecutionerContract.Yishi || f.TriggerId != "prevent-slash-and-take" || f.InstructionIndex != 1 || f.WindowContext is not { } context || !SourceYishiQualifies(f.OwnerSeat, context) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.Prevented || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].Candidate))
            throw new InvalidOperationException("Source611 Yishi requires its actual positive Slash damage and nonempty original target.");
        var attack = _coreProgramAttackAttemptOperations.CurrentDamageAttempt!;
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceExecutionerYishiPreventedEvent(f.Id, window.Id, attack.ResolutionId, f.OwnerSeat, window.TargetSeat, window.Amount));
        return SkillProgramStepOutcome.Continue;
    }
}
