#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineObserverSubtreesRuntime
{
    internal bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = null, bool routeUnreturnedAttackThroughRuntime = false)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (top is ProgramSkillFrame { AttackAttempt: not null } attack && (!routeUnreturnedAttackThroughRuntime || attack.AttackReturn is not null))
        {
            if (attack.AttackReturn is null || _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId != attack.Id || _coreDyingCursorRuntimeOperations.ActiveDying is not null || _resolutionStackOperations._resolutionStack.Any(frame => frame is DamageFrame damage && damage.ParentFrameId == attack.Id || frame is BeforeDamageProgramWindowFrame before && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == attack.Id))
            {
                if (incompleteAttackError is not null)
                    throw new InvalidOperationException(incompleteAttackError);
                return false;
            }

            _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(attack.Id));
        }
        else if (top is HappyQiaoHealthChoiceFrame or ProgramSkillFrame or CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame or ProgramLifecycleTriggerWindowFrame or ProgramCardTriggerWindowFrame or BeforeDamageProgramWindowFrame or ProgramDeathTriggerWindowFrame or ProgramKillTriggerWindowFrame || advanceRecoveryReplacement && top is RecoveryReplacementFrame)
        {
            _coreRuntimeOperations.AdvanceRuntimeFrame(top.Id);
        }
        else if (top is DeathFrame death)
        {
            _coreDeathProgramsOperations.ContinueDeathResolution(death.Id);
        }
        else
        {
            // Card uses (including Dying-suit Peach) and other native frames
            // retain their own dispatch and typed return.
            return false;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal ProgramSkillFrame? FindProgramObserverRoot(Func<ProgramSkillFrame, ResolutionFrame, bool> firstChild, Func<int, bool> suffix, long? requiredRootId = null)
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
            if (_resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame root && (requiredRootId is null || root.Id == requiredRootId) && firstChild(root, _resolutionStackOperations._resolutionStack[index + 1]) && suffix(index))
                return root;
        return null;
    }

    internal bool HasObserverSuffix(int rootIndex, Func<int, bool> edge, Func<int, DyingFrame, bool>? completedDyingSuffix)
    {
        for (var child = rootIndex + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            if (!edge(child))
                return false;
            if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && child + 1 < _resolutionStackOperations._resolutionStack.Count && ((completedDyingSuffix)is { } completedDyingCallback ? completedDyingCallback(child, dying) : default(bool? )) == true)
                return true;
        }

        return true;
    }

    internal static bool IsObserverSkillsChangedEdge(ResolutionFrame parent, ResolutionFrame child) => RuntimeSkills.RuntimeRules_Core_ObserverSubtrees.IsObserverSkillsChangedEdge(parent, child);
    internal bool IsPaidRescueObserverSuffix(int child, DyingFrame dying) => _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying);
    internal bool IsSuspendedOrPaidRescueObserverSuffix(int child, DyingFrame dying) => _coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(dying) || _coreRecipientCategoryMarkContinuationsOperations.IsOriginalDyingSuspendedByRecipientCategoryMark(dying) || _coreOwnedDeathBenefitReturnsOperations.IsOriginalDyingSuspendedByOwnedDeathBenefit(dying) || _characterHeJinOlOperations.IsOriginalDyingSuspendedByHeJinYanhuo(dying) || _characterDiaoChanLeOperations.IsOriginalDyingSuspendedByDiou(dying) || _characterZhangFeiWumiaoOperations.IsOriginalDyingSuspendedByWuZishengMovement(dying) || IsPaidRescueObserverSuffix(child, dying);
    internal bool HasMovementObserverSuffix(int rootIndex) => HasObserverSuffix(rootIndex, (child) => this.IsMovementObserverEdge(child), (child, dying) => this.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal bool IsMovementObserverEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(current, parent) || _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoShutuDeclarationEdge(child) || IsObserverSkillsChangedEdge(parent, current) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandStructuralEdge(parent, current) || _coreCompletedUndamagedTargetRevealObserversOperations.CompletedUndamagedTargetRevealStructuralEdge(parent, current) || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkStructuralEdge(parent, current) || _coreOffTurnUsedCardObserversOperations.OffTurnUsedCardGiftStructuralEdge(parent, current) || _coreCardSupplyCompletionOperations.CardSupplyCompletionStructuralEdge(parent, current) || _coreResponseCompletionOperations.ResponseCompletionStructuralEdge(parent, current) || _coreOriginalHandEntityObserversOperations.OriginalHandEntityStructuralEdge(parent, current) || _coreOutsidePhaseDrawDiscardObserversOperations.OutsidePhaseDrawDiscardStructuralEdge(parent, current) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _characterDianWeiHappyGodOperations.HappyDianWeiStructuralEdge(current, parent) || _characterPangTongHappyGodOperations.HappyPangTongStructuralEdge(current, parent) || _characterDiaoChanHappyGodOperations.HappyDiaoChanStructuralEdge(current, parent) || _characterMuShunAnniversaryOperations.MuShunPindianStructuralEdge(current, parent);
    }

    internal bool HasHandGainDebtObserverSuffix(int rootIndex) => HasObserverSuffix(rootIndex, (child) => IsObserverSkillsChangedEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]), (child, dying) => this.IsPaidRescueObserverSuffix(child, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying)));
    internal bool HasPaidDamageObserverSuffix(int rootIndex) => HasObserverSuffix(rootIndex, (child) => IsObserverSkillsChangedEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child), (child, dying) => this.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
}
