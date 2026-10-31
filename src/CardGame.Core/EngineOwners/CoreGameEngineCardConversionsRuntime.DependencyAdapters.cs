#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardConversionsCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false) => this._targetFactory().IsProgramMaterialUseForbidden(actorSeat, kind, cardIds, response);
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }

    public bool IsProgramViewAsAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => this._targetFactory().IsProgramViewAsAllowed(owner, kind, cardIds, source, forResponse, dyingUse, requestedUseTarget);
    public bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreCardConversionsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime))]
internal sealed class CoreCardConversionsCoreProgramDistanceRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramDistanceRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}
