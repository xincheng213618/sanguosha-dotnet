#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed class CoreFeibaiEffectiveUseKindsCharacterCaiYongLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFeibaiEffectiveUseKindsCharacterCaiYongLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind FeibaiRecordedKind(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact) => this._targetFactory().FeibaiRecordedKind(fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime))]
internal sealed class CoreFeibaiEffectiveUseKindsCoreSlashUseOrdinalsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFeibaiEffectiveUseKindsCoreSlashUseOrdinalsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind SlashUseKindAtPrefix(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current, global::CardGame.Core.CardKind original) => this._targetFactory().SlashUseKindAtPrefix(fact, history, end, current, original);
}
