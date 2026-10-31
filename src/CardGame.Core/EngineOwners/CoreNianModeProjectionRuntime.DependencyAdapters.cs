#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime))]
internal sealed class CoreNianModeProjectionCoreNianChallengeHostAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNianModeProjectionCoreNianChallengeHostOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianChallengeHostRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.INianChallengeFeature? NianFeature { get => this._targetFactory().NianFeature; }
}
