#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuZhiOlRuntime
{
    internal bool IsBuZhiActivation(SkillProgramActivation a) => DingpanRuntime.IsActivation(a);
    internal int? BuZhiPhaseLimit(CharacterState owner, SkillProgramActivation a) => DingpanRuntime.PhaseLimit(a, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);
    internal bool BuZhiActivationTargetAllowed(CharacterState target, SkillProgramActivation a) => DingpanRuntime.AllowsTarget(target.Seat, a, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);
    internal bool BuZhiPaidEquipmentEntitlement(ProgramSkillFrame f) => f.BuZhi is { BatchId: null, Target: { } target } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && ValidBuZhi(f) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<BuZhiMovementPaidEvent>().Count(e => e.FrameId == f.Id && e.Target == target && e.ReturnStage == "branch" && e.CardIds.Count == 0) == 1;
    internal void PayBuZhiEquipment(ProgramSkillFrame f, IReadOnlyList<int> ids, CardLocation destination, string next)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BuZhi = f.BuZhi!with { Stage = "movement", ReturnStage = next, Before = before, After = before, Actual = 0, PaidIds = ids }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var logicalDiscard = destination == CardLocation.DiscardPile ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, ids, new(BuZhiMoveReason), f.Id) : null;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(BuZhiMoveReason), (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var actual = records.Count(m => m.To == destination || destination == CardLocation.DiscardPile && m.To == CardLocation.OutsideGame);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BuZhi = active.BuZhi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Actual = actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BuZhiMovementPaidEvent(f.Id, next, active.BuZhi!.Target!.Value, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual, ids));
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    internal void IssueBuZhiDamage(ProgramSkillFrame f)
    {
        if (!BuZhiCurrent(f) || !_coreMatchParticipantsPlayersAccess._players[f.BuZhi!.Target!.Value].IsAlive)
        {
            FinishBuZhi(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BuZhi = f.BuZhi!with { Stage = "damage-issued", DamageIssued = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BuZhiDamageIssuedEvent(f.Id, f.OwnerSeat, f.BuZhi!.Target!.Value, 1));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.BuZhi.Target.Value, 1);
    }
}
