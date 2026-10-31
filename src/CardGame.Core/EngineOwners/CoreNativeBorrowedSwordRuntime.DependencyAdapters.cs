#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime))]
internal sealed class CoreNativeBorrowedSwordCoreProgramDistanceRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeBorrowedSwordCoreProgramDistanceRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CoreNativeBorrowedSwordCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeBorrowedSwordCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreNativeBorrowedSwordCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeBorrowedSwordCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}
