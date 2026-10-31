#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRoundFirstSlashTargetsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreRoundFirstSlashTargetsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRoundFirstSlashTargetsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRoundFirstSlashTargetsCoreSlashUseOrdinalsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind SlashUseKindAtPrefix(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current);
    int CountSlashUsePrefix(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int start, int end, int expectedActorSeat, bool elemental, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current);
}

internal interface ICoreRoundFirstSlashTargetsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
