#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanShaoBoundaryMobileRuntime))]
internal sealed class CoreProgramCardLegalityCharacterYuanShaoBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCardLegalityCharacterYuanShaoBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsMobileYuanShaoViewAsAllowed(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source) => this._targetFactory().IsMobileYuanShaoViewAsAllowed(owner, cardIds, source);
}
