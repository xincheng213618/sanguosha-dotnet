#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYouAnniversaryRuntime
{
    internal static CardConversionSource LiuYouSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal AnniversaryLiuYouStartedEvent[] LiuYouPhaseStarts(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouStartedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == AnniversaryLiuYouComposition.Skill && e.Phase == phase).ToArray();
    internal bool LiuYouPhaseAvailable(int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && LiuYouPhaseStarts(owner, p)is var starts && starts.Length < _coreMatchParticipantsPlayersAccess._players[owner].Hp && !starts.Any(s => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouResultEvent>().Any(e => e.FrameId == s.FrameId && e.Winner == owner));
    internal bool LiuYouTarget(int owner, int target) => LiuYouPhaseAvailable(owner) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && target != owner && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[owner]) && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[target]) && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(owner, target) && !LiuYouPhaseStarts(owner, _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()!).Any(e => e.Target == target);
    bool global::CardGame.Core.IAnniversaryLiuYouQueries.Target(int owner, int target) => LiuYouTarget(owner, target);
    int global::CardGame.Core.IAnniversaryLiuYouQueries.Hp(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].Hp;
    void global::CardGame.Core.IAnniversaryLiuYouHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.PrepareAnniversaryLiuYouKannan)
            PrepareLiuYou(f);
        else
            ResolveLiuYou(f);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 50000, SkillProgramEffectOp.PrepareAnniversaryLiuYouKannan)]
    internal bool CanStartLiuYou(CharacterState owner) => LiuYouPhaseAvailable(owner.Seat) && _coreMatchParticipantsPlayersAccess._players.Any(p => LiuYouTarget(owner.Seat, p.Seat));
    [ProgramPhaseUsedProjection(50000)]
    internal int LiuYouProjectedUses(int owner, string skill, int fallback) => skill == AnniversaryLiuYouComposition.Skill && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase ? LiuYouPhaseStarts(owner, phase).Length : fallback;
    internal void PrepareLiuYou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != AnniversaryLiuYouComposition.Skill || f.ActivationId != AnniversaryLiuYouComposition.Activation || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.AnniversaryLiuYou is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var peer] || !LiuYouTarget(f.OwnerSeat, peer))
            throw new InvalidOperationException("Kannan requires the current actual own Play, remaining HP quota, and an unchallenged native-legal opponent.");
        var receipt = new AnniversaryLiuYouReceipt(LiuYouSource(f), f.GameplayHash, _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()!, peer);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryLiuYou = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYouStartedEvent(f.Id, receipt.Source, receipt.Hash, receipt.Phase, peer));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 50000)]
    internal void CaptureLiuYouPindian(PindianResultDeterminedEvent fact)
    {
        if (fact.SkillId != AnniversaryLiuYouComposition.Skill)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianFrame pair || pair.Id != fact.FrameId || pair.Result is { } already && already != fact.Result || pair.ProgramResultBind != AnniversaryLiuYouComposition.Result || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(candidate => candidate.Id == pair.ParentFrameId)is not { AnniversaryLiuYou: { PindianId: null } r } f || f.InstructionIndex != 2 || !ValidLiuYou(f) || pair.SourceSeat != f.OwnerSeat || pair.OpponentSeat != r.Target)
            throw new InvalidOperationException("Kannan cannot capture another native Pindian or repeated revealed result.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryLiuYou = r with { PindianId = pair.Id, Result = fact.Result } });
    }

    internal bool ValidLiuYou(ProgramSkillFrame f)
    {
        if (f.AnniversaryLiuYou is not { } r || f.SkillId != AnniversaryLiuYouComposition.Skill || f.ActivationId != AnniversaryLiuYouComposition.Activation || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || !f.SelectedTargetSeats.SequenceEqual([r.Target]) || r.Source != LiuYouSource(f) || r.Hash != f.GameplayHash || r.Phase.ActorSeat != f.OwnerSeat || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouStartedEvent>().Count(e => e == new AnniversaryLiuYouStartedEvent(f.Id, r.Source, r.Hash, r.Phase, r.Target)) != 1)
            return false;
        if (r.PindianId is null)
            return r.Result is null && !r.Completed && f.InstructionIndex is 1 or 2;
        return r.Result is { } result && result.SourceSeat == f.OwnerSeat && result.OpponentSeat == r.Target && f.InstructionIndex is >= 2 and <= 4 && _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId && e.Result == result) == 1;
    }

    internal void ResolveLiuYou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidLiuYou(f) || f.InstructionIndex != 3 || f.AnniversaryLiuYou is not { Completed: false, PindianId: { } pairId, Result: { } result } r || _resolutionStackOperations._resolutionStack.Any(p => p.Id == pairId) || f.PindianResultBindings is not [{ Name: AnniversaryLiuYouComposition.Result } b] || b.SourceSeat != result.SourceSeat || b.OpponentSeat != result.OpponentSeat || b.SourceRank != result.SourceRank || b.OpponentRank != result.OpponentRank || b.SourceWon != result.SourceWon || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouResultEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Kannan must settle its once-returned native effective ranks and original bound result.");
        var winner = result.SourceWon ? result.SourceSeat : result.WonBy(result.OpponentSeat) ? result.OpponentSeat : -1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryLiuYou = r with { Completed = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYouResultEvent(f.Id, pairId, result.SourceRank, result.OpponentRank, winner));
        if (winner >= 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYouBonusIssuedEvent(f.Id, r.Source, r.Hash, pairId, winner));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 50000)]
    internal void AssertLiuYou(ProgramSkillFrame f)
    {
        if (f.AnniversaryLiuYou is not null && !ValidLiuYou(f))
            throw new InvalidOperationException("Kannan lost its actual-Play issue or original paid native Pindian.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 50000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 50000)]
    internal bool KeepLiuYouPaid(ProgramSkillFrame f) => f.AnniversaryLiuYou is not null && ValidLiuYou(f);
    internal AnniversaryLiuYouBonusIssuedEvent[] LiuYouUnconsumed(int beneficiary) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouBonusIssuedEvent>().Where(e => e.Beneficiary == beneficiary && e.Source.SkillId == AnniversaryLiuYouComposition.Skill && e.Source.BindingId == AnniversaryLiuYouComposition.Activation && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(e.Source.SkillId, out var skill) && skill.Program?.GameplayHash == e.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouStartedEvent>().Count(s => s.FrameId == e.FrameId && s.Source == e.Source && s.Hash == e.Hash) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouResultEvent>().Count(r => r.FrameId == e.FrameId && r.PindianId == e.PindianId && r.Winner == beneficiary) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYouBonusConsumedEvent>().Any(c => c.GrantFrameId == e.FrameId)).ToArray();
    internal void CaptureAnniversaryLiuYouNextSlash(long cardUseFrameId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(cardUseFrameId) ?? throw new InvalidOperationException("Kannan requires the actual committing card-use frame.");
        if (use.Action is not { Type: CardActionType.Use } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || use.AnniversaryLiuYouSlashBonus is not null)
            return;
        var grants = LiuYouUnconsumed(action.ActorSeat);
        if (grants.Length == 0)
            return;
        var amount = grants.Length;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { AnniversaryLiuYouSlashBonus = amount, ProgramAdjustedSlashBaseDamage = use.ProgramAdjustedSlashBaseDamage is { } original ? checked(original + amount) : use.CardAttack is { } attack ? checked(attack.DamageAmount + amount) : null, CardAttack = use.CardAttack is { } current ? current with { DamageAmount = checked(current.DamageAmount + amount) } : null });
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYouBonusConsumedEvent(grant.FrameId, use.Id, action.ActionId, action.ActorSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYouSlashCapturedEvent(use.Id, action.ActionId, action.ActorSeat, amount));
    }
}
