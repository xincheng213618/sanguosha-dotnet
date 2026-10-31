#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime))]
internal sealed class CoreNativePlayerDeathCoreNativeDeathFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreNativeDeathFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> _targetFactory = targetFactory;
    public long BeginDeath(long parentFrameId, int victimSeat, int? killerSeat, global::CardGame.Core.DeathReturnKind returnKind) => this._targetFactory().BeginDeath(parentFrameId, victimSeat, killerSeat, returnKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativePlayerDeathCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame) => this._targetFactory().GetDyingAttack(frame);
    public bool IsTeamMode { get => this._targetFactory().IsTeamMode; }
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreNativePlayerDeathCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void ExpireDeadOwnerDamageModifiers(int ownerSeat) => this._targetFactory().ExpireDeadOwnerDamageModifiers(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreNativePlayerDeathCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardAttackState GetCardAttackState(long id) => this._targetFactory().GetCardAttackState(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreNativePlayerDeathCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryCompleteHappyQiaoWholeDeath(long deathFrameId, global::CardGame.Core.CharacterState victim) => this._targetFactory().TryCompleteHappyQiaoWholeDeath(deathFrameId, victim);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime))]
internal sealed class CoreNativePlayerDeathCharacterHeJinOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCharacterHeJinOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> _targetFactory = targetFactory;
    public void CaptureHeJinYanhuoDeath(long deathId, global::CardGame.Core.CharacterState victim) => this._targetFactory().CaptureHeJinYanhuoDeath(deathId, victim);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime))]
internal sealed class CoreNativePlayerDeathCharacterCaoYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCharacterCaoYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.CaoYuDeathMinimumCapture? CaptureCaoYuDeathMinimumSources(int victimSeat) => this._targetFactory().CaptureCaoYuDeathMinimumSources(victimSeat);
    public void RecordCaoYuDeathMinimumTransitions(long deathFrameId, int victimSeat, global::CardGame.Core.GameEngine.CaoYuDeathMinimumCapture? capture) => this._targetFactory().RecordCaoYuDeathMinimumTransitions(deathFrameId, victimSeat, capture);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime))]
internal sealed class CoreNativePlayerDeathCoreNationalGeneralRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreNationalGeneralRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> _targetFactory = targetFactory;
    public void RevealNationalInformation(global::CardGame.Core.CharacterState player) => this._targetFactory().RevealNationalInformation(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime))]
internal sealed class CoreNativePlayerDeathCharacterDongZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCharacterDongZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> _targetFactory = targetFactory;
    public void ApplyZaowangSuccession(long deathId, global::CardGame.Core.CharacterState victim) => this._targetFactory().ApplyZaowangSuccession(deathId, victim);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed class CoreNativePlayerDeathCoreCardDamageMaximumHpReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreCardDamageMaximumHpReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _targetFactory = targetFactory;
    public void PublishMaximumHpDamageReplacement(long ownerId) => this._targetFactory().PublishMaximumHpDamageReplacement(ownerId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime))]
internal sealed class CoreNativePlayerDeathCharacterYuanYinAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCharacterYuanYinAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryBeginAnniversaryYuanYinDeathBeforeCleanup(long deathId) => this._targetFactory().TryBeginAnniversaryYuanYinDeathBeforeCleanup(deathId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime))]
internal sealed class CoreNativePlayerDeathCoreDeathProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreDeathProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> _targetFactory = targetFactory;
    public void ContinuePlayerDeathCleanup(long deathFrameId) => this._targetFactory().ContinuePlayerDeathCleanup(deathFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativePlayerDeathCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePlayerDeathCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}
