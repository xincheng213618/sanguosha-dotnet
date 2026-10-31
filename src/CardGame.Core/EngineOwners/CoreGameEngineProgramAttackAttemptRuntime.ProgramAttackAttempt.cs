#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAttackAttemptRuntime
{
    internal bool TryAdvanceChainedDamageTarget(global::CardGame.Core.GameEngine.IDamageAttempt attack, out int fromSeat)
    {
        _coreProgramDamageHooksOperations.DamageHooks.AdvanceChain(ProgramDamagePhase.BeforeChainAdvance, attack);
        // A zero segment cannot issue zero damage to the next recipient.
        // An already issued positive recipient base can still restore there.
        if (attack.DamageAmountFinalized && attack.DamageAmount <= 0 && _coreLateFixedTargetDamageCapScopesOperations.FixedDamageCapChain(attack).Scope?.PropagationBaseAmount is not > 0)
        {
            fromSeat = attack.TargetSeat;
            return false;
        }

        if (!attack.TryAdvanceChainedTarget(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreNativeDamageHandlesOperations.UsesFormalTengjia || _coreNativeDamageHandlesOperations.UsesFormalSilverLion || _coreRecipientScopedDamageOperations.HasShenwuRecipientDamageModifiers || _modeRulesOperations._modeRules.RequiresRecipientDamageReevaluation || _characterCheZhouAnniversaryOperations.HasAnniversaryCheZhouRules || _coreRecipientScopedDamageOperations.HasPublicEnemyRecipientDamageScope(attack) || _coreRecipientScopedDamageOperations.HasFixedTargetRecipientDamageScope(attack) || _corePassiveRulePoliciesOperations.HasPerOccurrenceSourceDamageModifiers(attack.SourceSeat) || _corePassiveDamageCapsOperations.HasNextRecipientPassiveDamageCap(attack), out fromSeat))
            return false;
        _coreProgramDamageHooksOperations.DamageHooks.AdvanceChain(ProgramDamagePhase.AfterChainAdvance, attack);
        return true;
    }

    internal AttackAttemptState GetProgramAttackState(long ownerFrameId) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == ownerFrameId)?.AttackAttempt ?? throw new InvalidOperationException($"Program damage owner {(ownerFrameId)} lost its attempt.");
    internal void UpdateProgramAttackState(long ownerFrameId, Func<AttackAttemptState, AttackAttemptState> update)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame is ProgramSkillFrame program && program.Id == ownerFrameId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame program || program.AttackAttempt is null)
            throw new InvalidOperationException($"Program damage owner {(ownerFrameId)} lost its attempt.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, program with { AttackAttempt = update(program.AttackAttempt) });
    }

    internal global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt
    {
        get
        {
            var programIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame is ProgramSkillFrame { AttackAttempt: not null });
            var cardIndex = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } card ? _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == card.ResolutionId) : -1;
            return programIndex > cardIndex && _resolutionStackOperations._resolutionStack[programIndex] is ProgramSkillFrame program ? _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(program.Id) : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
        }
    }

    internal bool IsCardAttackSuspendedByProgramDamage => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null && CurrentDamageAttempt is global::CardGame.Core.NativeOwners.ProgramAttackHandle;

    internal void CompleteProgramAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (_coreShanheS5YimieOperations.TryBeginShanheS5YimieRecovery(attack))
            return;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null && !_characterJiangWeiMobileGodOperations.IsMobileJwSuspendedDying(attack.ResolutionId) && !_coreProgramOwnerDeathDamageOperations.IsOwnerDeathSuspendedDying(attack.ResolutionId))
            throw new InvalidOperationException("Program damage retained a dying child.");
        if (attack.TryConsumeProgramDamageTransferFollowup(out var transfer))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[transfer.TargetSeat];
            var count = transfer.DrawLostHp && attack.DamageWasApplied && target.IsAlive ? Math.Max(0, target.MaxHp - target.Hp) : 0;
            if (count > 0)
                _corePhysicalDrawFacadesOperations.DrawCards(target, count, log: true, reason: new CardMoveReason("skill-program.damage-transfer.followup-draw"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageTransferCardsDrawnEvent(attack.ResolutionId, transfer.SkillId, transfer.OwnerSeat, transfer.TargetSeat, count));
        }

        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && TryAdvanceChainedDamageTarget(attack, out var fromSeat))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedDamagePropagatedEvent(attack.ResolutionId, attack.SourceSeat, fromSeat, attack.TargetSeat, attack.DamageAmount, _coreNativeCardClassificationsOperations.GetDamageNature(attack)));
            _coreMatchLogOperations.AddLog("ChainDamage", $"连环伤害从 {(_coreMatchParticipantsPlayersAccess._players[fromSeat].Name)} 传导至 {(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)}。", attack.SourceSeat, attack.TargetSeat);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                CompleteProgramAttack(attack);
            return;
        }

        var id = attack.ResolutionId;
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == id);
        if (index != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame program)
            throw new InvalidOperationException("Program damage lost its owner continuation.");
        var receipt = program.AttackReturn;
        _coreRecoveryDamageEdgesOperations.AssertRecoveryDamageOrigin(program);
        _coreSuitDamagePreventionBenefitsOperations.CaptureSuitPreventionDamageCompleted(program, attack.DamageWasApplied);
        program = _coreDynamicDiscardDamagePaymentsOperations.CaptureDynamicDiscardDamageCompleted(program);
        program = _characterZhaoZhongOlOperations.CaptureYintianProgramDamageCompleted(program, attack);
        program = _characterLuXunWumiaoOperations.CaptureWuXiongmuProgramDamageCompleted(program, attack);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, program with { AttackAttempt = null, AttackReturn = null });
        if (receipt?.ParentAttackOwnerFrameId is { } parentId && CurrentDamageAttempt?.ResolutionId != parentId)
            throw new InvalidOperationException("Program damage changed its suspended parent owner.");
        if (receipt?.ParentDamageWindowFrameId is { } windowId && _coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id != windowId)
            throw new InvalidOperationException("Program damage lost its parent damage occurrence.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }
}
