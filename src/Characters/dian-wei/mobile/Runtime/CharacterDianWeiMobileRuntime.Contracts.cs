#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterDianWeiMobileCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSayingRoundRange(int actor, int target);
}

internal interface ICharacterDianWeiMobileCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterDianWeiMobileCharacterTaishiCiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenTaishiCiPoweiRange(int actor, int target);
}

internal interface ICharacterDianWeiMobileResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterDianWeiMobileCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterDianWeiMobileCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICharacterDianWeiMobileCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICharacterDianWeiMobileCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterDianWeiMobileCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::CardGame.Core.RuleQueryEvaluation EvaluateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICharacterDianWeiMobileCoreProgramPersistentCombatPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGameFactionAttackRangeTarget(int actorSeat, int targetSeat);
}

internal interface ICharacterDianWeiMobileCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterDianWeiMobileCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}
