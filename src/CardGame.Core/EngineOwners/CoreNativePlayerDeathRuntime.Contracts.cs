#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativePlayerDeathCoreNativeDeathFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginDeath(long parentFrameId, int victimSeat, int? killerSeat, global::CardGame.Core.DeathReturnKind returnKind);
}

internal interface ICoreNativePlayerDeathCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame);
    bool IsTeamMode { get; }

    bool IsNationalWarMode { get; }
}

internal interface ICoreNativePlayerDeathResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativePlayerDeathCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireDeadOwnerDamageModifiers(int ownerSeat);
}

internal interface ICoreNativePlayerDeathCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
}

internal interface ICoreNativePlayerDeathCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteHappyQiaoWholeDeath(long deathFrameId, global::CardGame.Core.CharacterState victim);
}

internal interface ICoreNativePlayerDeathCharacterHeJinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureHeJinYanhuoDeath(long deathId, global::CardGame.Core.CharacterState victim);
}

internal interface ICoreNativePlayerDeathCharacterCaoYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.CaoYuDeathMinimumCapture? CaptureCaoYuDeathMinimumSources(int victimSeat);
    void RecordCaoYuDeathMinimumTransitions(long deathFrameId, int victimSeat, global::CardGame.Core.GameEngine.CaoYuDeathMinimumCapture? capture);
}

internal interface ICoreNativePlayerDeathModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreNativePlayerDeathKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativePlayerDeathCoreNationalGeneralRevealOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RevealNationalInformation(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativePlayerDeathCharacterDongZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyZaowangSuccession(long deathId, global::CardGame.Core.CharacterState victim);
}

internal interface ICoreNativePlayerDeathCoreCardDamageMaximumHpReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishMaximumHpDamageReplacement(long ownerId);
}

internal interface ICoreNativePlayerDeathCharacterYuanYinAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAnniversaryYuanYinDeathBeforeCleanup(long deathId);
}

internal interface ICoreNativePlayerDeathCoreDeathProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePlayerDeathCleanup(long deathFrameId);
}

internal interface ICoreNativePlayerDeathCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}
