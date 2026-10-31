#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePlayerViewProjectionQueriesCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ActionQueryScope BeginActionQuery();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICorePlayerViewProjectionQueriesProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerViewProjector Get();
}

internal interface ICorePlayerViewProjectionQueriesKernelCoreGameEngineActivePlayerEffectQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ActivePlayerEffectQueries CreateActivePlayerEffectQueries();
}

internal interface ICorePlayerViewProjectionQueriesCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? JuewuForeignHandRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICorePlayerViewProjectionQueriesKernelCoreGameEnginePlayerViewProjectionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.PlayerViewVisibilityQueries CreatePlayerViewVisibilityQueries();
    global::CardGame.Core.NativeOwners.PlayerViewCardsQueries CreatePlayerViewCardsQueries();
    global::CardGame.Core.NativeOwners.PlayerViewRulesQueries CreatePlayerViewRulesQueries();
    global::CardGame.Core.NativeOwners.PlayerViewMatchQueries CreatePlayerViewMatchQueries();
}
