#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDyingCursorRuntimeCoreRecipientCategoryMarkContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDyingCursorRuntimeCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get; }
}

internal interface ICoreDyingCursorRuntimeCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByWenhePoison(global::CardGame.Core.DyingFrame original);
}

internal interface ICoreDyingCursorRuntimeCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreHellKingProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByHellKing(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByDiou(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreProgramOwnerDeathDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByOwnerDeathDamage(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreDeathGrowthSkillObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByDeathGrowth(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCharacterJiangWeiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByMobileJwShenpei(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreNativeRecoveryDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByRecoveryDamage(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreNativeDrawThresholdDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCharacterDianWeiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByHappyGuardElai(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCharacterHeJinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByHeJinYanhuo(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreProgramDamageInsertionRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByProgramInsertion(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCoreOwnedDeathBenefitReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByOwnedDeathBenefit(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingCursorRuntimeCharacterYangBiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByAnniversaryYangBiao(global::CardGame.Core.DyingFrame dying);
}
