#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhenJiOlGodRuntime
{
    internal bool DrainOlShenfu(long frameId) => _olShenZhenJiOperations.Get().DrainChildWindows(frameId);
    int global::CardGame.Core.IOlShenZhenJiFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IOlShenZhenJiFacts.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    long global::CardGame.Core.IOlShenZhenJiFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    Winner global::CardGame.Core.IOlShenZhenJiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    IReadOnlyList<OlShenfuPlayer> global::CardGame.Core.IOlShenZhenJiFacts.Players => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => new OlShenfuPlayer(player.Seat, player.Name, player.IsAlive, player.IsHuman, player.Hp)).ToArray());

    OlShenfuPlayer global::CardGame.Core.IOlShenZhenJiFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].Hp);
    bool global::CardGame.Core.IOlShenZhenJiFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    IReadOnlyList<Card> global::CardGame.Core.IOlShenZhenJiFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IOlShenZhenJiFacts.HasSkillInstance(int seat, string skillId, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instance);
    int global::CardGame.Core.IOlShenZhenJiFacts.RelativeOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    ContentSkillDefinition global::CardGame.Core.IOlShenZhenJiFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IReadOnlyList<PromptChoice> global::CardGame.Core.IOlShenZhenJiFacts.HandDiscardChoices(long frameId, int chooser, int owner) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
    void global::CardGame.Core.IOlShenZhenJiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IOlShenZhenJiOperations.LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, actor, ids, reason);
    void global::CardGame.Core.IOlShenZhenJiOperations.BeginDamage(ProgramSkillFrame frame, int target, int amount, DamageNature nature) => _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, amount, nature: nature);
    bool global::CardGame.Core.IOlShenZhenJiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
}
