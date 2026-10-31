#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramCardLegalityCharacterYuanShaoBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileYuanShaoViewAsAllowed(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreProgramCardLegalityCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramCardLegality? _cardLegality { get; }
}

internal interface ICoreProgramCardLegalityKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType);
}
