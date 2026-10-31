#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMouMobileRuntime
{
    internal bool CanMouLbDuel(ProgramSkillFrame f, int target)
    {
        var r = f.MouLuBuLiyu!;
        return _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && target != f.OwnerSeat && target != r.VictimSeat && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, CardKind.Duel, CardActionType.Use) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(f.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Duel, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, target, CardKind.Duel) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(f.OwnerSeat, target, Suit.None);
    }

    internal void IssueMouLbDuel(ProgramSkillFrame f, int target)
    {
        var r = f.MouLuBuLiyu!;
        if (!ValidMouLb(f) || r.Stage != MouLuBuLiyuStage.VictimChoice || r.CategoryMask != 7 || !CanMouLbDuel(f, target))
            throw new InvalidOperationException("Mobile Liyu Duel lost its actual gain categories and victim's legal choice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r = r with { Stage = MouLuBuLiyuStage.DuelIssued, DuelTarget = target } });
        _coreCardAttackStateOperations.UpdateCardAttackState(r.OuterUseId, s => s!with { Active = false });
        var origin = new ProgramDamageTargetDuelOrigin(f.Id, f.InstructionIndex, MouLbOriginAnchor(f), r.OuterUseId, r.WindowId, r.DamageFrameId, r.OuterActionId, f.OwnerSeat, r.VictimSeat, target, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, r.ActualTurn);
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var use = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [target], CardKind.Duel, physicalCardIds: [], damageTargetDuelOrigin: origin);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuDuelIssuedEvent(f.Id, use, f.OwnerSeat, target));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(use, card, f.OwnerSeat, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
    }

    internal long MouLbOriginAnchor(ProgramSkillFrame f) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuObtainedEvent>().First(e => e.FrameId == f.Id).Sequence;
    internal static bool IsMouLbDuelOrigin(ProgramDamageTargetDuelOrigin o) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_bu_mou_mobile_MouLuBuMobileNative.IsMouLbDuelOrigin(o);
    internal bool MatchesMouLbDuelParent(ProgramSkillFrame f, ProgramDamageTargetDuelOrigin o) => IsMouLbDuelOrigin(o) && ValidMouLb(f) && f.MouLuBuLiyu is { Stage: MouLuBuLiyuStage.DuelIssued, Paid: true, CategoryMask: 7, DuelTarget: { } target } r && o.ParentProgramFrameId == f.Id && o.InstructionIndex == f.InstructionIndex && o.ReceiptSequence == MouLbOriginAnchor(f) && o.OuterCardUseFrameId == r.OuterUseId && o.DamageWindowId == r.WindowId && o.DamageFrameId == r.DamageFrameId && o.OuterActionId == r.OuterActionId && o.InitialActorSeat == f.OwnerSeat && o.VictimChooserSeat == r.VictimSeat && o.InitialTargetSeat == target && o.SkillId == f.SkillId && o.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) && o.SkillInstanceId == f.SkillInstanceId && o.GameplayHash == f.GameplayHash && o.TurnNumber == r.ActualTurn && o.BaseDamage is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuBranchEvent>().Count(e => e.FrameId == f.Id && e.ChooserSeat == r.VictimSeat && e.Branch == "duel" && e.TargetSeat == target && e.CategoryMask == 7) == 1;

    [NativeUseReturn(229)]
    internal void ReturnMouLbNullifiedDuel(CardUseFrame use)
    {
        if (use.DamageTargetDuelOrigin is not { AttackStarted: false } o || !IsMouLbDuelOrigin(o))
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !MatchesMouLbDuelParent(f, o))
            throw new InvalidOperationException("Nullified mobile Liyu Duel lost its parent.");
        CompleteMouLbDuel(f, use.Id, false);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 99)]
    internal bool ReturnMouLbDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c)
    {
        if (c.DamageTargetDuelReturn is not { AttackStarted: true } o || !IsMouLbDuelOrigin(o))
            return false;
        if (c.ProgramFrameId != o.ParentProgramFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !MatchesMouLbDuelParent(f, o))
            throw new InvalidOperationException("Mobile Liyu Duel attack lost its complete native return.");
        CompleteMouLbDuel(f, c.ResolutionId, c.DamageWasApplied);
        return true;
    }

    internal void CompleteMouLbDuel(ProgramSkillFrame f, long useId, bool damage)
    {
        var r = f.MouLuBuLiyu!;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.OuterUseId)?.CardAttack is not { Active: false })
            throw new InvalidOperationException("Liyu return cannot resume another attack.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuDuelReturnedEvent(f.Id, useId, damage));
        _coreCardAttackStateOperations.UpdateCardAttackState(r.OuterUseId, s => s!with { Active = true });
        FinishMouLb(f);
    }
}
