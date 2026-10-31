#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramDrawRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreProgramDrawRuntimePortsCoreForcedPhaseAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ActualTurnDeathDrawCount(global::CardGame.Core.ProgramSkillFrame f, int seat);
}

internal interface ICoreProgramDrawRuntimePortsCorePairBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureFixedRecipientDraw(long frameId, int targetSeat, global::CardGame.Core.CardMoveReason reason, long sequenceBefore, int actualCount);
}

internal interface ICoreProgramDrawRuntimePortsCoreProgramCategoryTargetLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetProgramCategoryTargetTurnUsage(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramDrawRuntimePortsCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false);
}

internal interface ICoreProgramDrawRuntimePortsCharacterWangYiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryWangYiMijiDrawCount(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramDrawRuntimePortsCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CurrentTurnCategoryDrawCount(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat);
}

internal interface ICoreProgramDrawRuntimePortsCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long PairBenefitSequence { get; }
}

internal interface ICoreProgramDrawRuntimePortsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreProgramDrawRuntimePortsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramDrawRuntimePortsCoreActualDiscardPhaseLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TurnOwnerDiscardPhaseHandDiscardCount { get; }
}

internal interface ICoreProgramDrawRuntimePortsCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetProgramOwnerLostHp(global::CardGame.Core.ProgramSkillFrame frame);
}
