#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheS5XuchuAiCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreShanheS5XuchuAiCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheS5XuchuAiCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetTurnDrawCount(global::CardGame.Core.CharacterState current);
}

internal interface ICoreShanheS5XuchuAiCoreShanheS5XuchuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame ShanheS5LuoyiDrawPlan(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.ShanheS5LuoyiFrozenEvent ShanheS5LuoyiFrozen(global::CardGame.Core.ProgramSkillFrame f, string bind);
}
