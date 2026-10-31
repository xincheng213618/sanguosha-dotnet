#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaiShiCiMobileShiRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67600)]
    internal int ShiTzZhanlieDamageBonus(GameEngine.IDamageAttempt attack) => _shiTaiShiCiOperations.Get().ZhanlieDamageBonus((attack as NativeOwners.CardAttackHandle)?.ProgramSkillCardUseFrameId);
    ShiTaiShiCiPlayer IShiTaiShiCiFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].Hp, _coreMatchParticipantsPlayersAccess._players[seat].MaxHp);
    int IShiTaiShiCiFacts.LivingCount => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive);

    bool IShiTaiShiCiFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    IReadOnlyList<Card> IShiTaiShiCiFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    int IShiTaiShiCiFacts.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int IShiTaiShiCiFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    long IShiTaiShiCiFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    Winner IShiTaiShiCiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool IShiTaiShiCiFacts.HasActivationOperation(SkillProgramEffectOp operation) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(operation);
    ContentSkillDefinition IShiTaiShiCiFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> IShiTaiShiCiFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    IReadOnlyList<CardMovementRecord> IShiTaiShiCiFacts.Movements => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.AsReadOnly();

    int IShiTaiShiCiFacts.LieScanSequence => _characterTaiShiCiMobileShiShiTzLieScanSequenceAccess._shiTzLieScanSequence;

    int IShiTaiShiCiFacts.AttackRange(int seat) => _coreNativeRuleQueriesOperations.GetAttackRange(seat);
    ShiTaiShiCiAttack IShiTaiShiCiFacts.Attack(long frameId)
    {
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(frameId);
        return new(attack.ResolutionId, attack.TargetSeat, attack.CardUserSeat, attack.EffectiveCardKind);
    }

    long? IShiTaiShiCiFacts.ActiveAttackId => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId;

    IReadOnlyList<int> IShiTaiShiCiFacts.Targets(int owner, SkillProgramTargetKind kind) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, kind);
    IReadOnlyList<PromptChoice> IShiTaiShiCiFacts.HandDiscardChoices(long frameId, int chooser, int owner) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
    bool IShiTaiShiCiFacts.Allies(int owner, int target) => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    int IShiTaiShiCiFacts.KeepValue(int owner, Card card) => CoreNativeAiDecisionsRuntime.GetKeepValue(card, _coreMatchParticipantsPlayersAccess._players[owner]);
    IReadOnlyList<IndexedSkillProgramInstance> IShiTaiShiCiFacts.ProgramInstances(int owner) => _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).ProgramInstances;
    void IShiTaiShiCiOperations.SetLieScanSequence(int sequence) => _characterTaiShiCiMobileShiShiTzLieScanSequenceAccess._shiTzLieScanSequence = sequence;
    void IShiTaiShiCiOperations.MarkEffectIneffective(long frameId, int target) => _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(frameId, target);
    void IShiTaiShiCiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IShiTaiShiCiOperations.LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, actor, ids, reason);
    SkillProgramStepOutcome IShiTaiShiCiOperations.BeginVirtualCardUse(ProgramSkillFrame frame, int target, CardKind kind, bool ignoreDistance) => _coreProgramTurnPhaseOperations.BeginProgramVirtualCardUse(frame, target, kind, ignoreDistance);
    void IShiTaiShiCiOperations.DrawCards(long frameId, int recipient, int amount, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramCards(frameId, recipient, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
    void IShiTaiShiCiOperations.Recover(long frameId, int owner, int target, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    bool IShiTaiShiCiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
    void IShiTaiShiCiOperations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
}
