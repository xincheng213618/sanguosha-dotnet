#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramAlcoholConversionsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreProgramAlcoholConversionsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreProgramAlcoholConversionsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuyouIdentityMaterial(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreProgramAlcoholConversionsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}
