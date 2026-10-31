#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAdvancedPoliciesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMarkUnlimitedUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreAdvancedPoliciesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreAdvancedPoliciesCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreAdvancedPoliciesSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreAdvancedPoliciesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreAdvancedPoliciesCoreFriendTrioOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendActualCardUse(global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreAdvancedPoliciesCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFarthestInRange(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
}
