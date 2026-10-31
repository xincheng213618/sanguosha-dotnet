#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDamageArmorPoliciesCoreDuanhunOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveDuanhunPolicySource(int ownerSeat, global::CardGame.Core.IndexedSkillProgramInstance source);
    bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICoreNativeDamageArmorPoliciesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeDamageArmorPoliciesCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangArmorBypass(global::CardGame.Core.GameEngine.IDamageAttempt attack);
    int WushuangArmorDamageMultiplier(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageArmorPoliciesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}
