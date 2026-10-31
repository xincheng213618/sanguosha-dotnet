#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime))]
internal sealed class CoreHandCardGuidanceProjectionCoreHandGuidanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHandCardGuidanceProjectionCoreHandGuidanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.HandCardGuidance> BuildHumanHandGuidance(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> legalActions) => this._targetFactory().BuildHumanHandGuidance(legalActions);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreHandCardGuidanceProjectionCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> GetHumanLegalActions() => this._targetFactory().GetHumanLegalActions();
}
