#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeRuleQueriesCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::CardGame.Core.RuleQueryEvaluation EvaluateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreNativeRuleQueriesCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
    int TableStepSeat(int from, int offset);
}

internal interface ICoreNativeRuleQueriesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
    void ValidatePlayerSeat(int seat, string parameterName);
}

internal interface ICoreNativeRuleQueriesCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateHandLimit(global::CardGame.Core.CharacterState player, bool suppressHanSongYinbi = false);
}
