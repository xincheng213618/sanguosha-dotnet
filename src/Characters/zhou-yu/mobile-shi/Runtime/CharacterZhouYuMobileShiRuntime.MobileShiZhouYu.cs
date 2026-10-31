#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMobileShiRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67800)]
    internal int ShiZhouYuFentaoAmplification(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _shiZhouYuOperations.Get().FentaoAmplification(attack.ResolutionId, attack.IsChainPropagation);
    ShiZhouYuPlayer global::CardGame.Core.IShiZhouYuFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].IsChained);
    IReadOnlyList<ShiZhouYuPlayer> global::CardGame.Core.IShiZhouYuFacts.Players => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => ((global::CardGame.Core.IShiZhouYuFacts)this).Player(player.Seat)).ToArray());

    bool global::CardGame.Core.IShiZhouYuFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    IReadOnlyList<Card> global::CardGame.Core.IShiZhouYuFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<Card> global::CardGame.Core.IShiZhouYuFacts.Equipment(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    int global::CardGame.Core.IShiZhouYuFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IShiZhouYuFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IShiZhouYuFacts.Phase => (int)_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    Winner global::CardGame.Core.IShiZhouYuFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.IShiZhouYuFacts.HasTriggerOperation(SkillProgramEffectOp operation) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(operation);
    ContentSkillDefinition global::CardGame.Core.IShiZhouYuFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> global::CardGame.Core.IShiZhouYuFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    CardConversionSource global::CardGame.Core.IShiZhouYuFacts.Source(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    CardLocation global::CardGame.Core.IShiZhouYuFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    IReadOnlyList<Card> global::CardGame.Core.IShiZhouYuFacts.CardsAt(CardLocation location) => _cardZonesOperations._cardZones.CardsAt(location);
    bool global::CardGame.Core.IShiZhouYuFacts.IsRedCard(int owner, Card card) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card));
    bool global::CardGame.Core.IShiZhouYuFacts.Allies(int owner, int target) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    int global::CardGame.Core.IShiZhouYuFacts.KeepValue(int owner, Card card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, _coreMatchParticipantsPlayersAccess._players[owner]);
    bool global::CardGame.Core.IShiZhouYuFacts.ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right) => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(left, right);
    ShiZhouYuDamage? global::CardGame.Core.IShiZhouYuFacts.Damage => _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack ? new(attack.ResolutionId, attack.TargetSeat, _coreNativeCardClassificationsOperations.GetDamageNature(attack)) : null;

    void global::CardGame.Core.IShiZhouYuOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IShiZhouYuOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
    void global::CardGame.Core.IShiZhouYuOperations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    void global::CardGame.Core.IShiZhouYuOperations.MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, disclosure: disclosure);
    void global::CardGame.Core.IShiZhouYuOperations.LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, actor, ids, reason);
    void global::CardGame.Core.IShiZhouYuOperations.RevealHand(long frameId, int owner, string name, IReadOnlyList<int> ids) => _coreProgramRevealsOperations.CommitProgramHandReveal(frameId, owner, name, ids);
    void global::CardGame.Core.IShiZhouYuOperations.Damage(ProgramSkillFrame frame, int target, int amount, DamageNature nature) => _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, amount, nature: nature);
    void global::CardGame.Core.IShiZhouYuOperations.DrawCards(long frameId, int recipient, int amount, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramCards(frameId, recipient, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
    void global::CardGame.Core.IShiZhouYuOperations.SetChained(ProgramSkillFrame frame, bool chained, int target) => _coreProgramLifecycleOperations.SetProgramChainedState(frame, chained, target);
    void global::CardGame.Core.IShiZhouYuOperations.Log(string category, string message, int actor, int target) => _coreMatchLogOperations.AddLog(category, message, actor, target);
}
