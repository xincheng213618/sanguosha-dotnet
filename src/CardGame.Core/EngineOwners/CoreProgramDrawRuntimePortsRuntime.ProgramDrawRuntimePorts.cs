#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDrawRuntimePortsRuntime
{
    bool global::CardGame.Core.IProgramDrawFacts.HasHpValue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    int global::CardGame.Core.IProgramDrawFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    string? global::CardGame.Core.IProgramDrawFacts.EffectiveFaction(int seat) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]);
    int global::CardGame.Core.IProgramDrawFacts.AttackRange(int seat) => _coreNativeRuleQueriesOperations.GetAttackRange(seat);
    long global::CardGame.Core.IProgramDrawFacts.PairBenefitSequence => _corePairObtainShownGiftsOperations.PairBenefitSequence;

    int global::CardGame.Core.IProgramDrawFacts.TurnOwnerDiscardPhaseHandDiscardCount => _coreActualDiscardPhaseLedgerOperations.TurnOwnerDiscardPhaseHandDiscardCount;

    int global::CardGame.Core.IProgramDrawFacts.CategoryTargetTurnUsage(ProgramSkillFrame frame) => _coreProgramCategoryTargetLedgerOperations.GetProgramCategoryTargetTurnUsage(frame);
    int global::CardGame.Core.IProgramDrawFacts.OwnerLostHp(ProgramSkillFrame frame) => _coreProgramOwnedCardsOperations.GetProgramOwnerLostHp(frame);
    int global::CardGame.Core.IProgramDrawFacts.CurrentTurnCategoryDrawCount(ProgramSkillFrame frame, int targetSeat) => _coreCurrentTurnUseKindsOperations.CurrentTurnCategoryDrawCount(frame, targetSeat);
    int global::CardGame.Core.IProgramDrawFacts.ActualTurnDeathDrawCount(ProgramSkillFrame frame, int targetSeat) => _coreForcedPhaseAlcoholOperations.ActualTurnDeathDrawCount(frame, targetSeat);
    int global::CardGame.Core.IProgramDrawFacts.CommittedMijiCount(ProgramSkillFrame frame) => _characterWangYiBoundaryMobileOperations.BoundaryWangYiMijiDrawCount(frame);
    void global::CardGame.Core.IProgramDrawOperations.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    IReadOnlyList<int> global::CardGame.Core.IProgramDrawOperations.DrawCards(int seat, int count, bool log, CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], count, log, reason);
    void global::CardGame.Core.IProgramDrawOperations.CaptureFixedRecipientDraw(long frameId, int targetSeat, CardMoveReason reason, long sequenceBefore, int actualCount) => _corePairBenefitObserversOperations.CaptureFixedRecipientDraw(frameId, targetSeat, reason, sequenceBefore, actualCount);
    int global::CardGame.Core.IProgramDrawOperations.FreezeNormalDrawBase(int seat) => _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[seat]);
}
