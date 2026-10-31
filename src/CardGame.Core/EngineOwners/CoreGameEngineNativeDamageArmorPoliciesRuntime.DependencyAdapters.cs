#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime))]
internal sealed class CoreNativeDamageArmorPoliciesCoreDuanhunAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageArmorPoliciesCoreDuanhunOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> _targetFactory = targetFactory;
    public bool IsActiveDuanhunPolicySource(int ownerSeat, global::CardGame.Core.IndexedSkillProgramInstance source) => this._targetFactory().IsActiveDuanhunPolicySource(ownerSeat, source);
    public bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => this._targetFactory().DuanhunBypassesNativeDamageArmor(sourceSeat, sourceLess, visibleSkillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeDamageArmorPoliciesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageArmorPoliciesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime))]
internal sealed class CoreNativeDamageArmorPoliciesCoreWushuangCombatTacticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageArmorPoliciesCoreWushuangCombatTacticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> _targetFactory = targetFactory;
    public bool WushuangArmorBypass(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().WushuangArmorBypass(attack);
    public int WushuangArmorDamageMultiplier(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().WushuangArmorDamageMultiplier(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeDamageArmorPoliciesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageArmorPoliciesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}
