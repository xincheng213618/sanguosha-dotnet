#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime))]
internal sealed class CoreProgramChoicesCharacterPanZhangMaZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCharacterPanZhangMaZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TrySelectBoundaryAnjianOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, out global::CardGame.Core.PromptChoice choice) => this._targetFactory().TrySelectBoundaryAnjianOption(decision, frame, out choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuChuBoundaryMobileRuntime))]
internal sealed class CoreProgramChoicesCharacterXuChuBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCharacterXuChuBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TrySelectBoundaryXuChuAiOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.PromptChoice choice) => this._targetFactory().TrySelectBoundaryXuChuAiOption(decision, f, out choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime))]
internal sealed class CoreProgramChoicesCoreShanheS5XuchuAiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCoreShanheS5XuchuAiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> _targetFactory = targetFactory;
    public bool TrySelectShanheS5LuoyiAiOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan, out global::CardGame.Core.PromptChoice choice) => this._targetFactory().TrySelectShanheS5LuoyiAiOption(decision, frame, plan, out choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPaidCardRuntime))]
internal sealed class CoreProgramChoicesProgramPaidCardAdapter(global::System.Func<global::CardGame.Core.ProgramPaidCardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesProgramPaidCardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPaidCardRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreProgramChoicesCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramChoicesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramAiPublicContext CreateProgramAiPublicContext(global::CardGame.Core.CharacterState owner) => this._targetFactory().CreateProgramAiPublicContext(owner);
    public global::CardGame.Core.Card[] GetClaimableProgramDamageCards(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetClaimableProgramDamageCards(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime))]
internal sealed class CoreProgramChoicesCoreProgramPersistentStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCoreProgramPersistentStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _targetFactory = targetFactory;
    public bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId) => this._targetFactory().GetProgramBooleanState(ownerSeat, skillId, skillInstanceId, stateId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramChoicesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreProgramChoicesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramChoicesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AlternatingChoiceBenefitResolvedEvent? GetAlternatingChoicePredecessor(int owner, string skill, string instance, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().GetAlternatingChoicePredecessor(owner, skill, instance, effect);
}
