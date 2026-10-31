#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreModeRulesModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreModeRulesCoreMatchOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Winner GetWinnerForTeam(string teamId);
    global::CardGame.Core.Winner GetWinnerForFaction(string factionId);
}

internal interface ICoreModeRulesCharacterDongZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ZaowangRebelDeathWins(global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer);
}

internal interface ICoreModeRulesModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}
