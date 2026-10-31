#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientScopedDamageRuntime
{
    internal bool HasShenwuRecipientDamageModifiers => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventShenwuNitaiOwnTurnDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ShenwuArmedSourceDamageBonus);
    internal bool NeedsRecipientScopedDamageBase => _coreSignedDamagePaymentsOperations.HasSignedDamagePayments || HasShenwuRecipientDamageModifiers || _modeRulesOperations._modeRules.RuntimeRequirements.CapturesRecipientDamageBase || _characterCheZhouAnniversaryOperations.HasAnniversaryCheZhouRules || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheS5Yimie);

    internal bool SupportsIssuedRecipientDamageScope(RecipientScopedDamageState scope) => NeedsRecipientScopedDamageBase || scope.PublicEnemyObserver && _corePassiveRulePoliciesOperations.HasPublicEnemyParticipantDamageModifiers || scope.FixedTargetDamageCap && _contentRegistryOperations._contentRegistry.ProgramDependencies.HasFixedTargetDamageCaps;
    internal bool HasPublicEnemyRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack) => (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).RecipientDamageScope : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).RecipientDamageScope)?.PublicEnemyObserver == true;
    internal bool HasFixedTargetRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack) => (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).RecipientDamageScope : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).RecipientDamageScope)?.FixedTargetDamageCap == true;
    internal void CaptureRecipientScopedDamageBase(global::CardGame.Core.GameEngine.IDamageAttempt attack, IReadOnlyList<int> targets)
    {
        if (targets.Count == 0)
            return;
        // A relevant observer or an active fixed cap at an actual possible
        // recipient may need this base before that recipient is visited.
        // Subsequent death or suppression cannot revoke its issued scope.
        var publicEnemyObserver = _corePassiveRulePoliciesOperations.HasEligiblePublicEnemyDamageObserver(attack, targets);
        var fixedTargetDamageCap = _contentRegistryOperations._contentRegistry.ProgramDependencies.HasFixedTargetDamageCaps && targets.Prepend(attack.TargetSeat).Any(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _corePassiveDamageCapsOperations.GetIncomingFixedDamageCap(_coreMatchParticipantsPlayersAccess._players[seat])is not null);
        if (!NeedsRecipientScopedDamageBase && !publicEnemyObserver && !fixedTargetDamageCap)
            return;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !attack.DamageAmountFinalized || attack.IsChainPropagation || attack.DamageAmount <= 0)
            throw new InvalidOperationException("Recipient damage base must originate from the finalized first elemental recipient.");
        var scope = new RecipientScopedDamageState(attack.TargetSeat, attack.TargetSeat, attack.DamageAmount)
        {
            PublicEnemyObserver = publicEnemyObserver,
            FixedTargetDamageCap = fixedTargetDamageCap,
            ProgramUseInstructionIndex = _coreProgramVirtualDuelOperations.DamageProgramUseInstructionIndex(attack)
        };
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { RecipientDamageScope = scope });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { RecipientDamageScope = scope });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientScopedDamageBaseEvent(attack.ResolutionId, attack.TargetSeat, attack.DamageAmount) { PublicEnemyObserver = publicEnemyObserver, FixedTargetDamageCap = fixedTargetDamageCap, ProgramUseInstructionIndex = scope.ProgramUseInstructionIndex });
    }

    internal void RestoreRecipientScopedDamageBase(global::CardGame.Core.GameEngine.IDamageAttempt attack, int fromSeat)
    {
        var scope = attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).RecipientDamageScope : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).RecipientDamageScope;
        if (scope is null)
            return;
        if (!SupportsIssuedRecipientDamageScope(scope) || scope.ProgramUseInstructionIndex != _coreProgramVirtualDuelOperations.DamageProgramUseInstructionIndex(attack) || !attack.IsChainPropagation || scope.CurrentTargetSeat != fromSeat || scope.PropagationBaseAmount <= 0 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientScopedDamageBaseEvent>().Count(e => e.AttackFrameId == attack.ResolutionId && e.FirstTargetSeat == scope.FirstTargetSeat && e.Amount == scope.PropagationBaseAmount && e.PublicEnemyObserver == scope.PublicEnemyObserver && e.FixedTargetDamageCap == scope.FixedTargetDamageCap && e.LateFixedTargetDamageCap == scope.LateFixedTargetDamageCap && e.ProgramUseInstructionIndex == scope.ProgramUseInstructionIndex) != 1 || !_coreLateFixedTargetDamageCapScopesOperations.IsValidLateFixedTargetDamageCapScope(scope, attack.ResolutionId, _coreLateFixedTargetDamageCapScopesOperations.FixedDamageCapChain(attack).Targets) || scope.LateFixedTargetDamageCap is { } late && _coreLateFixedTargetDamageCapScopesOperations.FixedDamageCapChain(attack).Index <= late.NextTargetIndex)
            throw new InvalidOperationException("Recipient damage propagation lost its exact frozen base.");
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { DamageAmount = scope.PropagationBaseAmount, RecipientDamageScope = scope with { CurrentTargetSeat = attack.TargetSeat } });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { DamageAmount = scope.PropagationBaseAmount, RecipientDamageScope = scope with { CurrentTargetSeat = attack.TargetSeat } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientScopedDamageAdvancedEvent(attack.ResolutionId, fromSeat, attack.TargetSeat, scope.PropagationBaseAmount) { ProgramUseInstructionIndex = scope.ProgramUseInstructionIndex });
    }

    internal void AdjustExactRecipientDamage(ProgramSkillFrame f, ProgramSignedDamagePaymentReceipt receipt, int after)
    {
        if (!_coreSignedDamagePaymentsOperations.IsValidSignedDamagePayment(f, true) || receipt.Stage != SignedDamagePaymentStage.Paid || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageAmountFinalized: true } attack || attack.ResolutionId != receipt.OriginalAttackFrameId || attack.SourceSeat != receipt.SourceSeat || attack.TargetSeat != receipt.TargetSeat || attack.DamageAmount != receipt.OriginalAmount || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != receipt.Nature || after != Math.Max(0, checked(receipt.OriginalAmount + receipt.Delta)))
            throw new InvalidOperationException("Recipient damage adjustment lost its paid original attack and current target.");
        // Do not use the older aggregate DamageWasApplied guard: it protects the
        // entire use, while this new receipt proves the current chained recipient.
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { DamageAmount = after });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { DamageAmount = after });
    }

    internal void UpdateRecipientScopedDamageRedirect(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state.RecipientDamageScope is { } scope ? state with { RecipientDamageScope = scope with { CurrentTargetSeat = attack.TargetSeat } } : state);
        else if (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).RecipientDamageScope is { } scope)
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { RecipientDamageScope = scope with { CurrentTargetSeat = attack.TargetSeat } });
    }

    internal void AssertRecipientScopedDamageStates()
    {
        foreach (var frame in _resolutionStackOperations._resolutionStack)
        {
            var scope = frame switch
            {
                ProgramSkillFrame { AttackAttempt: { } a } => a.RecipientDamageScope,
                CardUseFrame { CardAttack: { } a } => a.RecipientDamageScope,
                ProgramSkillFrame { CardAttack: { } a } => a.RecipientDamageScope,
                JudgmentFrame { CardAttack: { } a } => a.RecipientDamageScope,
                _ => null
            };
            if (scope is null)
                continue;
            var target = frame is ProgramSkillFrame { AttackAttempt: { } attempt } ? attempt.TargetSeat : _coreCardAttackStateOperations.GetCardAttackState(frame.Id).TargetSeat;
            var chain = frame is ProgramSkillFrame { AttackAttempt: { } attempt2 } ? attempt2.IsChainPropagation : _coreCardAttackStateOperations.GetCardAttackState(frame.Id).IsChainPropagation;
            var instructionIndex = frame is ProgramSkillFrame { AttackAttempt: not null } ? null : _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(frame.Id);
            var targets = frame is ProgramSkillFrame { AttackAttempt: { } programAttack } ? programAttack.ChainedTargetSeats ?? [] : _coreCardAttackStateOperations.GetCardAttackState(frame.Id).ChainedTargetSeats;
            var cursor = frame is ProgramSkillFrame { AttackAttempt: { } programCursor } ? programCursor.ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(frame.Id).ChainedTargetIndex;
            if (!SupportsIssuedRecipientDamageScope(scope) || scope.ProgramUseInstructionIndex != instructionIndex || scope.CurrentTargetSeat != target || scope.PropagationBaseAmount <= 0 || !_coreLateFixedTargetDamageCapScopesOperations.IsValidLateFixedTargetDamageCapScope(scope, frame.Id, targets) || scope.LateFixedTargetDamageCap is { } lateCursor && !(cursor == lateCursor.ChainTargetIndex && target == lateCursor.PaidTargetSeat) && (cursor <= lateCursor.NextTargetIndex || cursor > targets.Count) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientScopedDamageBaseEvent>().Count(e => e.AttackFrameId == frame.Id && e.FirstTargetSeat == scope.FirstTargetSeat && e.Amount == scope.PropagationBaseAmount && e.PublicEnemyObserver == scope.PublicEnemyObserver && e.FixedTargetDamageCap == scope.FixedTargetDamageCap && e.LateFixedTargetDamageCap == scope.LateFixedTargetDamageCap && e.ProgramUseInstructionIndex == scope.ProgramUseInstructionIndex) != 1 || chain && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientScopedDamageAdvancedEvent>().Any(e => e.AttackFrameId == frame.Id && e.TargetSeat == target && e.Amount == scope.PropagationBaseAmount && e.ProgramUseInstructionIndex == scope.ProgramUseInstructionIndex) && !(scope.LateFixedTargetDamageCap is { } late && late.PaidTargetSeat == target && late.ChainTargetIndex == cursor) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDamageTransferredEvent>().Any(e => e.ResolutionId == frame.Id && e.TargetSeat == target))
                throw new InvalidOperationException("Recipient scoped damage state lost its issued base or actual propagation target.");
        }
    }
}
