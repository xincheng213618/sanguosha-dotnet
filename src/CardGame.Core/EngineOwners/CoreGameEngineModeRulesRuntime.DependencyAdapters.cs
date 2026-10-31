#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime))]
internal sealed class CoreModeRulesCoreMatchOutcomesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreModeRulesCoreMatchOutcomesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Winner GetWinnerForTeam(string teamId) => this._targetFactory().GetWinnerForTeam(teamId);
    public global::CardGame.Core.Winner GetWinnerForFaction(string factionId) => this._targetFactory().GetWinnerForFaction(factionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime))]
internal sealed class CoreModeRulesCharacterDongZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreModeRulesCharacterDongZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> _targetFactory = targetFactory;
    public bool ZaowangRebelDeathWins(global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer) => this._targetFactory().ZaowangRebelDeathWins(victim, killer);
}
