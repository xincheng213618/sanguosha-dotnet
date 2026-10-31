#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhenJiOlGodRuntime
{
    internal bool DrainOlShenfu(long frameId) => _olShenZhenJiOperations.Get().DrainChildWindows(frameId);
    int IOlShenZhenJiFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int IOlShenZhenJiFacts.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    long IOlShenZhenJiFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    Winner IOlShenZhenJiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    IReadOnlyList<OlShenfuPlayer> IOlShenZhenJiFacts.Players => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => new OlShenfuPlayer(player.Seat, player.Name, player.IsAlive, player.IsHuman, player.Hp)).ToArray());

    OlShenfuPlayer IOlShenZhenJiFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].Hp);
    IReadOnlyList<Card> IOlShenZhenJiFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IOlShenZhenJiFacts.HasSkillInstance(int seat, string skillId, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instance);
    int IOlShenZhenJiFacts.RelativeOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    ContentSkillDefinition IOlShenZhenJiFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IReadOnlyList<PromptChoice> IOlShenZhenJiFacts.HandDiscardChoices(long frameId, int chooser, int owner) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
    void IOlShenZhenJiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IOlShenZhenJiOperations.LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, actor, ids, reason);
    void IOlShenZhenJiOperations.BeginDamage(ProgramSkillFrame frame, int target, int amount, DamageNature nature) => _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, amount, nature: nature);
    bool IOlShenZhenJiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
}
