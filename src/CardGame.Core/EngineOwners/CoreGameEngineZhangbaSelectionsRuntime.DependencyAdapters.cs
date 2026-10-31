#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreZhangbaSelectionsCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhangbaSelectionsCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PhysicalGroupSuit(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CoreZhangbaSelectionsCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhangbaSelectionsCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PhysicalGroupColor(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreZhangbaSelectionsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhangbaSelectionsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreZhangbaSelectionsCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhangbaSelectionsCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().IsTurnPhysicalUseForbidden(seat, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreZhangbaSelectionsCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhangbaSelectionsCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetZhangbaSlashPairs(responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed class CoreZhangbaSelectionsCoreFinalTargetSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhangbaSelectionsCoreFinalTargetSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _targetFactory = targetFactory;
    public int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default) => this._targetFactory().ZhangbaSpecificSlashRank(actor, cards, provider);
}
