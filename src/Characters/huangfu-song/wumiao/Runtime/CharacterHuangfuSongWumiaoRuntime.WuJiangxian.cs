#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    internal WuHuangFuSongJiangxianArmedEvent? WuHfOutstandingArm(int owner) => !HasWuHuangFuSongCapability ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<WuHuangFuSongJiangxianArmedEvent>().SingleOrDefault(e => e.Source.OwnerSeat == owner && e.Turn == WuHfTurn() && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuHuangFuSongJiangxianSettledEvent>().Any(s => s.OwnerSeat == owner && s.Turn == e.Turn));
    internal int WuHfPriorDamageCount(int owner)
    {
        var h = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(h, e => e is TurnStartedEvent t && t.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && t.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        return Math.Min(5, h.Skip(start + 1).OfType<DamageAppliedEvent>().Count(e => !e.SourceLess && e.SourceSeat == owner && e.Amount > 0));
    }

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 50)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> WuHfDamageBonuses(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        // Native elemental propagation already carries the finalized first
        // damage amount, including this bonus; never add the same bonus twice.
        if (attack.IsChainPropagation || attack.IsSourceLess || attack.SourceSeat != attack.CardUserSeat || !HasIssuedWuHfUse(attack.ResolutionId, attack.SourceSeat) || WuHfOutstandingArm(attack.SourceSeat)is not { } arm)
            return[];
        var amount = WuHfPriorDamageCount(attack.SourceSeat);
        return amount == 0 ? [] : [(new(arm.Source.SkillId, arm.Source.BindingId, arm.Source.OwnerSeat, arm.Source.SkillInstanceId), amount)];
    }

    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeWuHfEndingItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        var arm = WuHfOutstandingArm(parent.OwnerSeat);
        if (arm is null || !_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive)
            return ordinary;
        var items = ordinary.Where(i => i.Candidate is not { SkillId: WuHuangFuSongComposition.Jiangxian, BindingId: "lose-after-turn" }).ToList();
        var c = new ProgramTriggerCandidate(parent.OwnerSeat, WuHuangFuSongComposition.Jiangxian, "lose-after-turn", arm.Source.SkillInstanceId, arm.GameplayHash, 0, 0);
        items.Add(new(TurnEndingBoundaryItemKind.Program, 0, $"wu-huangfu-song:{(arm.FrameId)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        return Array.AsReadOnly(items.ToArray());
    }

    [ProgramIssuedTrigger(460)]
    internal bool IsIssuedWuHfEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == WuHuangFuSongComposition.Jiangxian && c.BindingId == "lose-after-turn" && context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.OwnerSeat == c.OwnerSeat && context.SourceSeat == c.OwnerSeat && WuHfOutstandingArm(c.OwnerSeat)is { } arm && arm.Source.SkillInstanceId == c.SkillInstanceId && arm.GameplayHash == c.GameplayHash && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is { } parent && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) == c && parent.OwnerSeat == c.OwnerSeat && parent.TurnNumber == arm.Turn.Number;
}
