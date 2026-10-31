#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDrawRuntimePortsRuntime
{
    bool IProgramDrawFacts.HasHpValue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    int IProgramDrawFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    string? IProgramDrawFacts.EffectiveFaction(int seat) => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]);
    int IProgramDrawFacts.AttackRange(int seat) => _coreNativeRuleQueriesOperations.GetAttackRange(seat);
    long IProgramDrawFacts.PairBenefitSequence => _corePairObtainShownGiftsOperations.PairBenefitSequence;

    int IProgramDrawFacts.TurnOwnerDiscardPhaseHandDiscardCount => _coreActualDiscardPhaseLedgerOperations.TurnOwnerDiscardPhaseHandDiscardCount;

    int IProgramDrawFacts.CategoryTargetTurnUsage(ProgramSkillFrame frame) => _coreProgramCategoryTargetLedgerOperations.GetProgramCategoryTargetTurnUsage(frame);
    int IProgramDrawFacts.OwnerLostHp(ProgramSkillFrame frame) => _coreProgramOwnedCardsOperations.GetProgramOwnerLostHp(frame);
    int IProgramDrawFacts.CurrentTurnCategoryDrawCount(ProgramSkillFrame frame, int targetSeat) => _coreCurrentTurnUseKindsOperations.CurrentTurnCategoryDrawCount(frame, targetSeat);
    int IProgramDrawFacts.ActualTurnDeathDrawCount(ProgramSkillFrame frame, int targetSeat) => _coreForcedPhaseAlcoholOperations.ActualTurnDeathDrawCount(frame, targetSeat);
    int IProgramDrawFacts.CommittedMijiCount(ProgramSkillFrame frame) => _characterWangYiBoundaryMobileOperations.Value.BoundaryWangYiMijiDrawCount(frame);
    void IProgramDrawOperations.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    IReadOnlyList<int> IProgramDrawOperations.DrawCards(int seat, int count, bool log, CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], count, log, reason);
    void IProgramDrawOperations.CaptureFixedRecipientDraw(long frameId, int targetSeat, CardMoveReason reason, long sequenceBefore, int actualCount) => _corePairBenefitObserversOperations.CaptureFixedRecipientDraw(frameId, targetSeat, reason, sequenceBefore, actualCount);
    int IProgramDrawOperations.FreezeNormalDrawBase(int seat) => _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IProgramDrawOperations.PublishDrawCompleted(ProgramDrawCompletedEvent completed) => _ruleProgression.AdvanceEventRulesAndQueueFact(completed);
}
