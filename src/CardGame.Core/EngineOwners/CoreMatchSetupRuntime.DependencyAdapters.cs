#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreMatchSetupCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchSetupCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime))]
internal sealed class CoreMatchSetupCoreModePresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchSetupCoreModePresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ModePresentationContext CaptureModePresentationContext() => this._targetFactory().CaptureModePresentationContext();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime))]
internal sealed class CoreMatchSetupCoreOpeningCustomizationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchSetupCoreOpeningCustomizationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> _targetFactory = targetFactory;
    public void RequestOpeningHandConfirmation() => this._targetFactory().RequestOpeningHandConfirmation();
}
