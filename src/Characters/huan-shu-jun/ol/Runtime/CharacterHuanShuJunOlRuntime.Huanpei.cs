#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal bool HuanpeiRoundSpent(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiAdjustmentAcceptedEvent>().Any(e => e.OwnerSeat == owner && e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal IEnumerable<CardConversionSource> HuanpeiSources(CharacterState owner)
    {
        if (!owner.IsAlive || !owner.HasHpValue || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || HuanpeiRoundSpent(owner.Seat))
            yield break;
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
            foreach (var activation in instance.Program.Activations.Where(a => a.Effects is [{ Op: SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse }]))
                if (_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, instance.SkillId, instance.SkillInstanceId))
                    yield return new(instance.SkillId, activation.Id, owner.Seat, instance.SkillInstanceId);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28100, SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse)]
    internal bool CanActivateHuanpei(ProgramActivationLegalityContext c) => c.Owner.IsAlive && c.Owner.HasHpValue && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && HuanpeiSources(c.Owner).Any(s => s.SkillId == c.SkillId) && CanAdjustHuanpeiHand(c.Owner);
    internal bool CanAdjustHuanpeiHand(CharacterState actor) => actor.Hp <= _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count ? _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(actor).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count - Math.Max(0, actor.Hp) : true;
    internal void PayHuanpeiAdjustment(ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count;
        var hp = Math.Max(0, owner.Hp);
        var discard = Math.Max(0, hand - hp);
        if (r.Paid || !HuanpeiSources(owner).Contains(r.Source) || !CanAdjustHuanpeiHand(owner) || r.Selected.Count != discard || r.Selected.Distinct().Count() != discard || r.Selected.Any(id => _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(owner).All(c => c.Id != id)))
            throw new InvalidOperationException("Huanpei changed its exact current Hand adjustment cost or paid round.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (r.Request is { Intent: HuanpeiIntent.WenyiPeach, ParentFrameId: { } wenyiId } && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == wenyiId)is ProgramSkillFrame wenyi && !wenyi.HuanShuJun!.Paid)
            PrepareWenyiSuppliedPeach(wenyi, f.OwnerSeat, [], r.Source);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r = r with { Paid = true, FrozenHandCount = hand, FrozenHp = hp, RequestedCount = Math.Max(0, hp - hand), Before = before, After = before, Stage = HuanShuJunStage.AdjustmentChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiAdjustmentAcceptedEvent(f.Id, f.OwnerSeat, r.Round, hand, hp, Math.Abs(hand - hp), before));
        if (discard > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(HuanpeiDiscardReason), (batch, moves) =>
            {
                for (var i = 0; i < moves.Count; i++)
                {
                    var m = moves[i];
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiCostEntityEvent(f.Id, i, m.CardId, m.CardKind, m.From, m.Sequence));
                }
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = f.HuanShuJun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (r.RequestedCount > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = f.HuanShuJun!with { Stage = HuanShuJunStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.RequestedCount, new(HuanpeiDrawReason));
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiAdjustmentCompletedEvent(f.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 0, 0));
        ContinueHuanShuJun(f.Id);
    }
}
