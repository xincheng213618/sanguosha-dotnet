#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDrawTwoBonusCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreProgramDrawTwoBonusCoreNativeDrawAttemptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeDrawAttemptCapability { get; }
}

internal interface ICoreProgramDrawTwoBonusCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int WushuangDrawTwoCount(int sourceSeat);
}

internal interface ICoreProgramDrawTwoBonusContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
