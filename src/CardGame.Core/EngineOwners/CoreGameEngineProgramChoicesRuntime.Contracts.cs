#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramChoicesCharacterPanZhangMaZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySelectBoundaryAnjianOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, out global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramChoicesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramChoicesCharacterXuChuBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySelectBoundaryXuChuAiOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramChoicesCoreShanheS5XuchuAiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySelectShanheS5LuoyiAiOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan, out global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramChoicesProgramPaidCardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPaidCardRuntime Get();
}

internal interface ICoreProgramChoicesCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreProgramChoicesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramAiPublicContext CreateProgramAiPublicContext(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.Card[] GetClaimableProgramDamageCards(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramChoicesCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
}

internal interface ICoreProgramChoicesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramChoicesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AlternatingChoiceBenefitResolvedEvent? GetAlternatingChoicePredecessor(int owner, string skill, string instance, global::CardGame.Core.SkillProgramEffect effect);
}
