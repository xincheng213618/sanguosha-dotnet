#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardUseModulesCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseModulesCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramHandPhysicalRestricted(actorSeat, cardId);
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}
