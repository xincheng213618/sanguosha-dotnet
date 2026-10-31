#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterSharedGameEngineOlMoRumoProofsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidQiangangRumoOrigin(long frameId, global::CardGame.Core.CardConversionSource source, string gameplayHash);
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsCharacterDiaoChanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoDcQingshiRumoOrigin(global::CardGame.Core.OlMoRumoEnteredEvent e);
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoLuBuRumoOrigin(global::CardGame.Core.OlMoRumoEnteredEvent entry);
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactRoundEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterSharedGameEngineOlMoRumoProofsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}
