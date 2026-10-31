#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanHaiBridgesRuntime
{
    internal bool IsXieweiMultiTargetAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing) => _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { PhysicalCardIds: { } materials } use && _characterGuanHaiOlOperations.IsXieweiMultiTargetUse(use) && attack.ResolutionId == use.Id && attack.SourceSeat == pending.SourceSeat && attack.SourceSeat == use.SourceSeat && attack.EffectiveCardKind == pending.EffectiveCardKind && attack.EffectiveCardKind == use.CardKind && attack.Card == pending.Card && pending.Card.Id == use.CardId && use.TargetIndex == pending.TargetIndex && pending.TargetIndex < pending.TargetSeats.Count && attack.TargetSeat == _coreCardActionsOperations.GetFangtianEffectiveTarget(pending, pending.TargetIndex) && attack.PhysicalCards.Select(card => card.Id).SequenceEqual(materials) && materials.All(id => _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(use.Id, id)) && processing.Count == 0;
    internal Suit? ProgramMultiCardSuit(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? Suit.None : _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards);
    internal bool? ProgramMultiCardColor(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? null : _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, selection.Cards);
}
