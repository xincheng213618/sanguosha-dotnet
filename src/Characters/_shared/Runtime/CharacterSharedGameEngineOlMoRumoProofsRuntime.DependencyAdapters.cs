#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CharacterSharedGameEngineOlMoRumoProofsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CharacterSharedGameEngineOlMoRumoProofsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsPaidQiangangRumoOrigin(long frameId, global::CardGame.Core.CardConversionSource source, string gameplayHash) => this._targetFactory().IsPaidQiangangRumoOrigin(frameId, source, gameplayHash);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime))]
internal sealed class CharacterSharedGameEngineOlMoRumoProofsCharacterDiaoChanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsCharacterDiaoChanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsMoDcQingshiRumoOrigin(global::CardGame.Core.OlMoRumoEnteredEvent e) => this._targetFactory().IsMoDcQingshiRumoOrigin(e);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CharacterSharedGameEngineOlMoRumoProofsCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsMoLuBuRumoOrigin(global::CardGame.Core.OlMoRumoEnteredEvent entry) => this._targetFactory().IsMoLuBuRumoOrigin(entry);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed class CharacterSharedGameEngineOlMoRumoProofsCoreRoundEndingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsCoreRoundEndingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _targetFactory = targetFactory;
    public bool IsExactRoundEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().IsExactRoundEndingCandidate(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CharacterSharedGameEngineOlMoRumoProofsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}
