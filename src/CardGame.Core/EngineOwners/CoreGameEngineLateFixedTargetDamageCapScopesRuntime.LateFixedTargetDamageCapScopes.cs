#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLateFixedTargetDamageCapScopesRuntime
{
    internal (IReadOnlyList<int> Targets, int Index, RecipientScopedDamageState? Scope) FixedDamageCapChain(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? (_coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats ?? [], _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex, _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).RecipientDamageScope) : (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats, _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex, _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).RecipientDamageScope);
    internal static string FixedDamageCapChainHash(IReadOnlyList<int> targets) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_LateFixedTargetDamageCapScopes.FixedDamageCapChainHash(targets);
    internal void CaptureLateFixedTargetDamageCapScope(DamageFrame damage)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasFixedTargetDamageCaps || _coreDamageArmorPaymentsOperations.DamageArmorPaymentOwner(damage)is not { } attack)
            return;
        var chain = FixedDamageCapChain(attack);
        if (chain.Scope is not null || chain.Index < 0 || chain.Index >= chain.Targets.Count)
            return;
        var nextIndex = chain.Index;
        while (nextIndex < chain.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[chain.Targets[nextIndex]].IsAlive)
            nextIndex++;
        if (nextIndex == chain.Targets.Count || _corePassiveDamageCapsOperations.FindPassiveProgramDamageCap(_coreMatchParticipantsPlayersAccess._players[chain.Targets[nextIndex]], null)is not { } cap)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() != damage || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != attack.SourceSeat || damage.TargetSeat != attack.TargetSeat || damage.Amount != attack.DamageAmount || damage.Amount <= 0 || !attack.DamageAmountFinalized || !attack.DamageWasApplied || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != damage.Nature || damage.Nature is not (DamageNature.Fire or DamageNature.Thunder or DamageNature.Cold) || (!attack.IsChainPropagation ? chain.Index != 0 : chain.Index == 0 || chain.Targets[chain.Index - 1] != damage.TargetSeat))
            throw new InvalidOperationException("A late fixed damage cap requires the exact completed elemental damage segment and chain cursor.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var requested = Array.FindIndex(history, fact => fact is DamageRequestedEvent request && request.ResolutionId == damage.Id);
        var completed = Array.FindIndex(history, fact => fact is AfterDamageEvent after && after.ResolutionId == damage.Id);
        var applied = -1;
        if (requested >= 0 && completed > requested)
        {
            for (var index = requested + 1; index < completed; index++)
            {
                if (history[index] is DamageRequestedEvent)
                    break;
                if (history[index] is DamageAppliedEvent)
                {
                    applied = index;
                    break;
                }
            }
        }

        var receipt = new LateFixedTargetDamageCapScopeReceipt(attack.ResolutionId, damage.Id, attack.IsSourceLess ? null : attack.SourceSeat, attack.IsSourceLess, damage.TargetSeat, damage.Amount, damage.Nature, attack.EffectiveCardKind, chain.Index, nextIndex, chain.Targets[nextIndex], chain.Targets.Count, FixedDamageCapChainHash(chain.Targets), requested, applied, completed, _coreProgramVirtualDuelOperations.DamageProgramUseInstructionIndex(attack), cap.Source, _contentRegistryOperations._contentRegistry.GetSkill(cap.Source.SkillId).Program!.GameplayHash, cap.Cap);
        if (!IsValidLateFixedTargetDamageCapProof(receipt, attack.ResolutionId, chain.Targets) || damage.ArmorPayment is not null && !_coreDamageArmorPaymentsOperations.IsIssuedDamageArmorPayment(damage))
            throw new InvalidOperationException("A late fixed damage cap lost its issued request, atomic payment or completed damage fact.");
        var scope = new RecipientScopedDamageState(damage.TargetSeat, damage.TargetSeat, damage.Amount)
        {
            FixedTargetDamageCap = true,
            LateFixedTargetDamageCap = receipt,
            ProgramUseInstructionIndex = receipt.ProgramUseInstructionIndex
        };
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { RecipientDamageScope = scope });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { RecipientDamageScope = scope });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientScopedDamageBaseEvent(attack.ResolutionId, damage.TargetSeat, damage.Amount) { FixedTargetDamageCap = true, LateFixedTargetDamageCap = receipt, ProgramUseInstructionIndex = receipt.ProgramUseInstructionIndex });
    }

    internal bool IsValidLateFixedTargetDamageCapProof(LateFixedTargetDamageCapScopeReceipt proof, long attackFrameId, IReadOnlyList<int> chainTargets)
    {
        if (proof.AttackFrameId != attackFrameId || proof.DamageFrameId <= attackFrameId || proof.Amount <= 0 || proof.Nature is not (DamageNature.Fire or DamageNature.Thunder or DamageNature.Cold) || proof.SourceLess != (proof.SourceSeat is null) || proof.SourceSeat is { } source && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(proof.PaidTargetSeat) || proof.ChainTargetCount != chainTargets.Count || proof.ChainTargetsHash != FixedDamageCapChainHash(chainTargets) || proof.ChainTargetIndex < 0 || proof.NextTargetIndex < proof.ChainTargetIndex || proof.NextTargetIndex >= chainTargets.Count || chainTargets[proof.NextTargetIndex] != proof.NextTargetSeat || proof.ChainTargetIndex > 0 && chainTargets[proof.ChainTargetIndex - 1] != proof.PaidTargetSeat || proof.CapSource.OwnerSeat != proof.NextTargetSeat || string.IsNullOrWhiteSpace(proof.CapSource.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(proof.CapSource.SkillId)?.Program is not { } capProgram || capProgram.GameplayHash != proof.CapGameplayHash || !capProgram.DamageModifiers.Any(modifier => modifier.Id == proof.CapSource.BindingId && modifier.CapsToAmount && modifier.Amount == proof.Cap && modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageParticipant && modifier.Condition == SkillProgramDamageModifierCondition.Always && modifier.CardKinds.Count == 0))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (proof.RequestedEventIndex < 0 || proof.AppliedEventIndex <= proof.RequestedEventIndex || proof.AfterDamageEventIndex <= proof.AppliedEventIndex || proof.AfterDamageEventIndex >= history.Length || history[proof.RequestedEventIndex] is not DamageRequestedEvent request || request.ResolutionId != proof.DamageFrameId || history[proof.AppliedEventIndex] is not DamageAppliedEvent applied || history[proof.AfterDamageEventIndex] is not AfterDamageEvent after || after.ResolutionId != proof.DamageFrameId || request.TargetSeat != proof.PaidTargetSeat || applied.TargetSeat != proof.PaidTargetSeat || after.TargetSeat != proof.PaidTargetSeat || request.Amount != proof.Amount || applied.Amount != proof.Amount || after.Amount != proof.Amount || request.Nature != proof.Nature || applied.Nature != proof.Nature || after.Nature != proof.Nature || request.SourceCard != proof.SourceCard || request.SourceLess != proof.SourceLess || applied.SourceLess != proof.SourceLess || after.SourceLess != proof.SourceLess || (request.SourceLess ? null : (int? )request.SourceSeat) != proof.SourceSeat || (applied.SourceLess ? null : (int? )applied.SourceSeat) != proof.SourceSeat || (after.SourceLess ? null : (int? )after.SourceSeat) != proof.SourceSeat || history.Count(fact => fact is DamageRequestedEvent issued && issued.ResolutionId == proof.DamageFrameId) != 1 || history.Count(fact => fact is AfterDamageEvent finished && finished.ResolutionId == proof.DamageFrameId) != 1 || history.Skip(proof.RequestedEventIndex + 1).Take(proof.AppliedEventIndex - proof.RequestedEventIndex - 1).Any(fact => fact is DamageRequestedEvent or DamageAppliedEvent))
            return false;
        return applied.ArmorPayment is null || applied.ArmorPayment.AttackFrameId == attackFrameId && applied.ArmorPayment.DamageFrameId == proof.DamageFrameId;
    }

    internal bool IsValidLateFixedTargetDamageCapScope(RecipientScopedDamageState scope, long attackFrameId, IReadOnlyList<int> targets) => scope.LateFixedTargetDamageCap is not { } proof || scope.FixedTargetDamageCap && !scope.PublicEnemyObserver && scope.FirstTargetSeat == proof.PaidTargetSeat && scope.PropagationBaseAmount == proof.Amount && scope.ProgramUseInstructionIndex == proof.ProgramUseInstructionIndex && IsValidLateFixedTargetDamageCapProof(proof, attackFrameId, targets);
}
