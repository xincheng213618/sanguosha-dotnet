#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourcePojunRuntime
{
    internal const string ShanheSourcePojunSkill = "shanhe:source-pojun-14112";
    internal const string ShanheSourcePojunBind = "source-pojun-held";
    internal bool IsShanheSourcePojunMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    SkillProgramStepOutcome global::CardGame.Core.IShanheSourcePojunHost.HoldAllTargetHand(ProgramSkillFrame frame) => HoldShanheSourcePojunTargetHand(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62049, SkillProgramEffectOp.HoldShanheSourcePojunHand)]
    internal bool CanHoldShanheSourcePojunHand(ProgramTriggerLegalityContext context) => IsShanheSourcePojunMode && context.WindowContext is { Window: SkillProgramTriggerWindow.SlashBeforeResponse, TargetSeat: { } target, CardUse: { } use } && use.ActorSeat == context.OwnerSeat && target != context.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count > 0;
    internal SkillProgramStepOutcome HoldShanheSourcePojunTargetHand(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourcePojunMode || frame.SkillId != ShanheSourcePojunSkill || frame.WindowContext is not { Window: SkillProgramTriggerWindow.SlashBeforeResponse, TargetSeat: { } target, CardUse: { } action } context || frame.OwnerSeat != action.ActorSeat || target == frame.OwnerSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId || window.AttackOwnerFrameId is not { } attackId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(candidateUse => candidateUse.Id == attackId)is not { } use || use.Id != action.ParentCardUseFrameId || use.Action?.ActionId != action.CardActionId || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, _kernelCoreMainOperations.CreateCardAttackHandle(attackId)) || _kernelCoreMainOperations.CreateCardAttackHandle(attackId).TargetSeat != target || frame.CardSetBindings.Any(set => set.Name == ShanheSourcePojunBind) || frame.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Source Pojun lost its original current Slash, final target or unpaid instruction.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).ToArray();
        if (cards.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        // Native PojunHold is an outside-game zone. Its existing turn-end return
        // and death cleanup own the cards after this paid program has completed.
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Hand(target), CardLocation.PojunHold(target), CardMoveReasons.PojunHold);
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, ShanheSourcePojunBind, cards.Select(card => card.Id).ToArray(), SkillProgramCardSetVisibility.Private, cards.Select(_ => CardLocation.PojunHold(target)).ToArray());
        var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourcePojunHeldEvent(frame.Id, use.Id, action.CardActionId, frame.OwnerSeat, target, cards.Length, before, after));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }
}
