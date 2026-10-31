#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime))]
internal sealed class CoreMouLuMengCharacterGuanYuRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMouLuMengCharacterGuanYuRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> _targetFactory = targetFactory;
    public void ReturnDemonRoundHolds() => this._targetFactory().ReturnDemonRoundHolds();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreMouLuMengCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMouLuMengCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime))]
internal sealed class CoreMouLuMengCoreShanheSourceDifficultyAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMouLuMengCoreShanheSourceDifficultyOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> _targetFactory = targetFactory;
    public bool RequiresShanheSourceArmorTracking { get => this._targetFactory().RequiresShanheSourceArmorTracking; }

    public void RestoreShanheSourceRoundArmor() => this._targetFactory().RestoreShanheSourceRoundArmor();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreMouLuMengCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMouLuMengCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime))]
internal sealed class CoreMouLuMengCoreCharacterHostAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMouLuMengCoreCharacterHostOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> _targetFactory = targetFactory;
    public bool HasClassicGeneralPackage { get => this._targetFactory().HasClassicGeneralPackage; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed class CoreMouLuMengCoreRoundEndingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMouLuMengCoreRoundEndingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _targetFactory = targetFactory;
    public bool TracksRoundEnding { get => this._targetFactory().TracksRoundEnding; }
}
