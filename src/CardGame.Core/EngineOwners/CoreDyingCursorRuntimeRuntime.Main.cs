#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDyingCursorRuntimeRuntime
{
    internal DyingFrame? ActiveDying
    {
        get
        {
            for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
                if (_resolutionStackOperations._resolutionStack[index] is DyingFrame frame && !_coreProgramDamageInsertionRuntimeOperations.Value.IsOriginalDyingSuspendedByProgramInsertion(frame) && !(_coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(frame) || _coreRecipientCategoryMarkContinuationsOperations.Value.IsOriginalDyingSuspendedByRecipientCategoryMark(frame) || _characterYangBiaoAnniversaryOperations.IsOriginalDyingSuspendedByAnniversaryYangBiao(frame)) && !_coreOwnedDeathBenefitReturnsOperations.Value.IsOriginalDyingSuspendedByOwnedDeathBenefit(frame) && !_coreProgramOwnerDeathDamageOperations.IsOriginalDyingSuspendedByOwnerDeathDamage(frame) && !_characterDianWeiHappyGodOperations.Value.IsOriginalDyingSuspendedByHappyGuardElai(frame) && !_coreDeathGrowthSkillObserversOperations.Value.IsOriginalDyingSuspendedByDeathGrowth(frame) && !_characterHeJinOlOperations.IsOriginalDyingSuspendedByHeJinYanhuo(frame) && !_coreHellKingProgramsOperations.Value.IsOriginalDyingSuspendedByHellKing(frame) && !_coreWenhePoisonOperations.IsOriginalDyingSuspendedByWenhePoison(frame) && !_coreNativeRecoveryDamageOperations.IsOriginalDyingSuspendedByRecoveryDamage(frame) && !_coreNativeDrawThresholdDyingOperations.Value.IsOriginalDyingSuspendedByNativeDrawThresholdDamage(frame) && !_characterJiangWeiMobileGodOperations.IsOriginalDyingSuspendedByMobileJwShenpei(frame) && !_characterDiaoChanLeOperations.IsOriginalDyingSuspendedByDiou(frame) && !_characterZhangFeiWumiaoOperations.IsOriginalDyingSuspendedByWuZishengMovement(frame) && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(frame.Id))
                    return frame;
            return null;
        }
    }
}
