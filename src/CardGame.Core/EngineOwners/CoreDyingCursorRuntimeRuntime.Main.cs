#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDyingCursorRuntimeRuntime
{
    internal DyingFrame? ActiveDying
    {
        get
        {
            for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
                if (_resolutionStackOperations._resolutionStack[index] is DyingFrame frame && !_coreProgramDamageInsertionRuntimeOperations.IsOriginalDyingSuspendedByProgramInsertion(frame) && !(_coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(frame) || _coreRecipientCategoryMarkContinuationsOperations.IsOriginalDyingSuspendedByRecipientCategoryMark(frame) || _characterYangBiaoAnniversaryOperations.IsOriginalDyingSuspendedByAnniversaryYangBiao(frame)) && !_coreOwnedDeathBenefitReturnsOperations.IsOriginalDyingSuspendedByOwnedDeathBenefit(frame) && !_coreProgramOwnerDeathDamageOperations.IsOriginalDyingSuspendedByOwnerDeathDamage(frame) && !_characterDianWeiHappyGodOperations.IsOriginalDyingSuspendedByHappyGuardElai(frame) && !_coreDeathGrowthSkillObserversOperations.IsOriginalDyingSuspendedByDeathGrowth(frame) && !_characterHeJinOlOperations.IsOriginalDyingSuspendedByHeJinYanhuo(frame) && !_coreHellKingProgramsOperations.IsOriginalDyingSuspendedByHellKing(frame) && !_coreWenhePoisonOperations.IsOriginalDyingSuspendedByWenhePoison(frame) && !_coreNativeRecoveryDamageOperations.IsOriginalDyingSuspendedByRecoveryDamage(frame) && !_coreNativeDrawThresholdDyingOperations.IsOriginalDyingSuspendedByNativeDrawThresholdDamage(frame) && !_characterJiangWeiMobileGodOperations.IsOriginalDyingSuspendedByMobileJwShenpei(frame) && !_characterDiaoChanLeOperations.IsOriginalDyingSuspendedByDiou(frame) && !_characterZhangFeiWumiaoOperations.IsOriginalDyingSuspendedByWuZishengMovement(frame) && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(frame.Id))
                    return frame;
            return null;
        }
    }
}
