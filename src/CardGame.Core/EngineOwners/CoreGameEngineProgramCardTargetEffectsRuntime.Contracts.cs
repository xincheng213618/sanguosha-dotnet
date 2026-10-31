#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramCardTargetEffectsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramCardTargetEffectsCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreProgramCardTargetEffectsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreProgramCardTargetEffectsCharacterZhangXiuMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhangXiuOlQiulieIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreProgramCardTargetEffectsCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreProgramCardTargetEffectsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramCardTargetEffectsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreProgramCardTargetEffectsCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXinPiUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreProgramCardTargetEffectsCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreProgramCardTargetEffectsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramCardTargetEffectsCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}

internal interface ICoreProgramCardTargetEffectsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramCardTargetEffectsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreProgramCardTargetEffectsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreProgramCardTargetEffectsCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreProgramCardTargetEffectsCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteCanceledNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreProgramCardTargetEffectsCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNingwuUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreProgramCardTargetEffectsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}
