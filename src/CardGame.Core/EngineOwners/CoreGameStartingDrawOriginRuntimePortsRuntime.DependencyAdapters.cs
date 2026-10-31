#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime))]
internal sealed class CoreGameStartingDrawOriginRuntimePortsCharacterTangTangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameStartingDrawOriginRuntimePortsCharacterTangTangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TangTangGain>? CurrentTangTangGains(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CurrentTangTangGains(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime))]
internal sealed class CoreGameStartingDrawOriginRuntimePortsCoreOutsidePhaseDrawDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameStartingDrawOriginRuntimePortsCoreOutsidePhaseDrawDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardRuntime> _targetFactory = targetFactory;
    public bool TracksNativeDrawInvocations { get => this._targetFactory().TracksNativeDrawInvocations; }

    public bool ValidNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationProof proof) => this._targetFactory().ValidNativeDrawInvocation(proof);
}
