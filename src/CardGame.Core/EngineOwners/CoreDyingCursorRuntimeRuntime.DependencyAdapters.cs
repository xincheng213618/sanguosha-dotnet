#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreRecipientCategoryMarkContinuationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreRecipientCategoryMarkContinuationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByRecipientCategoryMark(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get => this._targetFactory().MouXiaoQiaoOl; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByWenhePoison(global::CardGame.Core.DyingFrame original) => this._targetFactory().IsOriginalDyingSuspendedByWenhePoison(original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByDyingSuits(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreHellKingProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreHellKingProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByHellKing(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByHellKing(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByWuZishengMovement(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByDiou(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByDiou(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreProgramOwnerDeathDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreProgramOwnerDeathDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByOwnerDeathDamage(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByOwnerDeathDamage(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreDeathGrowthSkillObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreDeathGrowthSkillObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByDeathGrowth(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByDeathGrowth(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterJiangWeiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterJiangWeiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByMobileJwShenpei(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByMobileJwShenpei(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreNativeRecoveryDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreNativeRecoveryDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByRecoveryDamage(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByRecoveryDamage(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreNativeDrawThresholdDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreNativeDrawThresholdDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByNativeDrawThresholdDamage(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterDianWeiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterDianWeiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByHappyGuardElai(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByHappyGuardElai(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterHeJinOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterHeJinOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByHeJinYanhuo(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByHeJinYanhuo(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreProgramDamageInsertionRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreProgramDamageInsertionRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByProgramInsertion(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByProgramInsertion(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime))]
internal sealed class CoreDyingCursorRuntimeCoreOwnedDeathBenefitReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCoreOwnedDeathBenefitReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByOwnedDeathBenefit(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByOwnedDeathBenefit(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime))]
internal sealed class CoreDyingCursorRuntimeCharacterYangBiaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingCursorRuntimeCharacterYangBiaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByAnniversaryYangBiao(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByAnniversaryYangBiao(dying);
}
