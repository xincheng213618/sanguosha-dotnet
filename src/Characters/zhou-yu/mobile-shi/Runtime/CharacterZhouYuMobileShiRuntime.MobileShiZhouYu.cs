#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMobileShiRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67800)]
    internal int ShiZhouYuFentaoAmplification(GameEngine.IDamageAttempt attack) => _shiZhouYuOperations.Get().FentaoAmplification(attack.ResolutionId, attack.IsChainPropagation);
    ShiZhouYuPlayer IShiZhouYuFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].IsChained);
    IReadOnlyList<ShiZhouYuPlayer> IShiZhouYuFacts.Players => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => ((IShiZhouYuFacts)this).Player(player.Seat)).ToArray());

    bool IShiZhouYuFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    IReadOnlyList<Card> IShiZhouYuFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<Card> IShiZhouYuFacts.Equipment(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    int IShiZhouYuFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int IShiZhouYuFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int IShiZhouYuFacts.Phase => (int)_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    Winner IShiZhouYuFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool IShiZhouYuFacts.HasTriggerOperation(SkillProgramEffectOp operation) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(operation);
    ContentSkillDefinition IShiZhouYuFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> IShiZhouYuFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    CardConversionSource IShiZhouYuFacts.Source(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    CardLocation IShiZhouYuFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    IReadOnlyList<Card> IShiZhouYuFacts.CardsAt(CardLocation location) => _cardZonesOperations._cardZones.CardsAt(location);
    bool IShiZhouYuFacts.IsRedCard(int owner, Card card) => CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card));
    bool IShiZhouYuFacts.Allies(int owner, int target) => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    int IShiZhouYuFacts.KeepValue(int owner, Card card) => CoreNativeAiDecisionsRuntime.GetKeepValue(card, _coreMatchParticipantsPlayersAccess._players[owner]);
    bool IShiZhouYuFacts.ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right) => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(left, right);
    ShiZhouYuDamage? IShiZhouYuFacts.Damage => _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack ? new(attack.ResolutionId, attack.TargetSeat, _coreNativeCardClassificationsOperations.GetDamageNature(attack)) : null;

    void IShiZhouYuOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IShiZhouYuOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
    void IShiZhouYuOperations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    void IShiZhouYuOperations.MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, disclosure: disclosure);
    void IShiZhouYuOperations.LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, actor, ids, reason);
    void IShiZhouYuOperations.RevealHand(long frameId, int owner, string name, IReadOnlyList<int> ids) => _coreProgramRevealsOperations.CommitProgramHandReveal(frameId, owner, name, ids);
    void IShiZhouYuOperations.Damage(ProgramSkillFrame frame, int target, int amount, DamageNature nature) => _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, amount, nature: nature);
    void IShiZhouYuOperations.DrawCards(long frameId, int recipient, int amount, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramCards(frameId, recipient, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
    void IShiZhouYuOperations.SetChained(ProgramSkillFrame frame, bool chained, int target) => _coreProgramLifecycleOperations.SetProgramChainedState(frame, chained, target);
    void IShiZhouYuOperations.Log(string category, string message, int actor, int target) => _coreMatchLogOperations.AddLog(category, message, actor, target);
}
