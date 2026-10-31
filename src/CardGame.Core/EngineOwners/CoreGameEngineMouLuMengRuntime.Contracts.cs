#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreMouLuMengKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreMouLuMengCharacterGuanYuRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnDemonRoundHolds();
}

internal interface ICoreMouLuMengCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreMouLuMengCoreShanheSourceDifficultyOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RequiresShanheSourceArmorTracking { get; }

    void RestoreShanheSourceRoundArmor();
}

internal interface ICoreMouLuMengContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreMouLuMengCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreMouLuMengSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreMouLuMengCoreCharacterHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasClassicGeneralPackage { get; }
}

internal interface ICoreMouLuMengCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksRoundEnding { get; }
}

internal interface ICoreMouLuMengModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}
