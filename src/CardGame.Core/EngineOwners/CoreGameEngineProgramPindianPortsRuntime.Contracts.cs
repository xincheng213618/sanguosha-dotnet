#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPindianPortsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSharedPindian(long parentId, global::CardGame.Core.SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = default, int? sourceCardId = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility programResultVisibility = (global::CardGame.Core.SkillProgramCardSetVisibility)1, global::CardGame.Core.PindianOwnedPileSource? ownedPileSource = default);
}

internal interface ICoreProgramPindianPortsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramPindianPortsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}
