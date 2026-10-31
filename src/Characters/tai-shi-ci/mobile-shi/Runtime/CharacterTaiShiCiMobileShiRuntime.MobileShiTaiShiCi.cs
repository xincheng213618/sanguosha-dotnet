#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaiShiCiMobileShiRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67600)]
    internal int ShiTzZhanlieDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _shiTaiShiCiOperations.Get().ZhanlieDamageBonus((attack as global::CardGame.Core.NativeOwners.CardAttackHandle)?.ProgramSkillCardUseFrameId);
    ShiTaiShiCiPlayer global::CardGame.Core.IShiTaiShiCiFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].Hp, _coreMatchParticipantsPlayersAccess._players[seat].MaxHp);
    int global::CardGame.Core.IShiTaiShiCiFacts.LivingCount => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive);

    bool global::CardGame.Core.IShiTaiShiCiFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    IReadOnlyList<Card> global::CardGame.Core.IShiTaiShiCiFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    int global::CardGame.Core.IShiTaiShiCiFacts.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IShiTaiShiCiFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    long global::CardGame.Core.IShiTaiShiCiFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    Winner global::CardGame.Core.IShiTaiShiCiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.IShiTaiShiCiFacts.HasActivationOperation(SkillProgramEffectOp operation) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(operation);
    ContentSkillDefinition global::CardGame.Core.IShiTaiShiCiFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> global::CardGame.Core.IShiTaiShiCiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<CardMovementRecord> global::CardGame.Core.IShiTaiShiCiFacts.Movements => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.AsReadOnly();

    int global::CardGame.Core.IShiTaiShiCiFacts.LieScanSequence => _characterTaiShiCiMobileShiShiTzLieScanSequenceAccess._shiTzLieScanSequence;

    int global::CardGame.Core.IShiTaiShiCiFacts.AttackRange(int seat) => _coreNativeRuleQueriesOperations.GetAttackRange(seat);
    ShiTaiShiCiAttack global::CardGame.Core.IShiTaiShiCiFacts.Attack(long frameId)
    {
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(frameId);
        return new(attack.ResolutionId, attack.TargetSeat, attack.CardUserSeat, attack.EffectiveCardKind);
    }

    long? global::CardGame.Core.IShiTaiShiCiFacts.ActiveAttackId => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId;

    IReadOnlyList<int> global::CardGame.Core.IShiTaiShiCiFacts.Targets(int owner, SkillProgramTargetKind kind) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, kind);
    IReadOnlyList<PromptChoice> global::CardGame.Core.IShiTaiShiCiFacts.HandDiscardChoices(long frameId, int chooser, int owner) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, chooser, owner, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
    bool global::CardGame.Core.IShiTaiShiCiFacts.Allies(int owner, int target) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    int global::CardGame.Core.IShiTaiShiCiFacts.KeepValue(int owner, Card card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, _coreMatchParticipantsPlayersAccess._players[owner]);
    IReadOnlyList<IndexedSkillProgramInstance> global::CardGame.Core.IShiTaiShiCiFacts.ProgramInstances(int owner) => _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).ProgramInstances;
    void global::CardGame.Core.IShiTaiShiCiOperations.SetLieScanSequence(int sequence) => _characterTaiShiCiMobileShiShiTzLieScanSequenceAccess._shiTzLieScanSequence = sequence;
    void global::CardGame.Core.IShiTaiShiCiOperations.MarkEffectIneffective(long frameId, int target) => _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(frameId, target);
    void global::CardGame.Core.IShiTaiShiCiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IShiTaiShiCiOperations.LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason) => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, actor, ids, reason);
    SkillProgramStepOutcome global::CardGame.Core.IShiTaiShiCiOperations.BeginVirtualCardUse(ProgramSkillFrame frame, int target, CardKind kind, bool ignoreDistance) => _coreProgramTurnPhaseOperations.BeginProgramVirtualCardUse(frame, target, kind, ignoreDistance);
    void global::CardGame.Core.IShiTaiShiCiOperations.DrawCards(long frameId, int recipient, int amount, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramCards(frameId, recipient, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
    void global::CardGame.Core.IShiTaiShiCiOperations.Recover(long frameId, int owner, int target, int amount) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frameId, owner, target, amount, null, null);
    bool global::CardGame.Core.IShiTaiShiCiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.AwaitedProgramMovement);
    void global::CardGame.Core.IShiTaiShiCiOperations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
}
