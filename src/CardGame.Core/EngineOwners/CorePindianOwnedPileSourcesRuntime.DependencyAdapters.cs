#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime))]
internal sealed class CorePindianOwnedPileSourcesCoreOutsidePhaseDrawDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianOwnedPileSourcesCoreOutsidePhaseDrawDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> _targetFactory = targetFactory;
    public long OutsidePhaseMovementSequence { get => this._targetFactory().OutsidePhaseMovementSequence; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CorePindianOwnedPileSourcesCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianOwnedPileSourcesCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsGuanHaiPindianSource(global::CardGame.Core.PindianFrame p, bool revealed) => this._targetFactory().IsGuanHaiPindianSource(p, revealed);
}
