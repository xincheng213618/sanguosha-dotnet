#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreForcedPhaseAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreForcedPhaseAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> _targetFactory = targetFactory;
    public int ActualTurnDeathDrawCount(global::CardGame.Core.ProgramSkillFrame f, int seat) => this._targetFactory().ActualTurnDeathDrawCount(f, seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCorePairBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCorePairBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime> _targetFactory = targetFactory;
    public void CaptureFixedRecipientDraw(long frameId, int targetSeat, global::CardGame.Core.CardMoveReason reason, long sequenceBefore, int actualCount) => this._targetFactory().CaptureFixedRecipientDraw(frameId, targetSeat, reason, sequenceBefore, actualCount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreProgramCategoryTargetLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreProgramCategoryTargetLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _targetFactory = targetFactory;
    public int GetProgramCategoryTargetTurnUsage(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramCategoryTargetTurnUsage(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false) => this._targetFactory().FreezeAndCaptureCaoXiNativeDrawBase(player, replaced);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCharacterWangYiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCharacterWangYiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryWangYiMijiDrawCount(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().BoundaryWangYiMijiDrawCount(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreCurrentTurnUseKindsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreCurrentTurnUseKindsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _targetFactory = targetFactory;
    public int CurrentTurnCategoryDrawCount(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => this._targetFactory().CurrentTurnCategoryDrawCount(frame, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public long PairBenefitSequence { get => this._targetFactory().PairBenefitSequence; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreActualDiscardPhaseLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreActualDiscardPhaseLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> _targetFactory = targetFactory;
    public int TurnOwnerDiscardPhaseHandDiscardCount { get => this._targetFactory().TurnOwnerDiscardPhaseHandDiscardCount; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreProgramOwnedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawRuntimePortsCoreProgramOwnedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _targetFactory = targetFactory;
    public int GetProgramOwnerLostHp(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramOwnerLostHp(frame);
}
