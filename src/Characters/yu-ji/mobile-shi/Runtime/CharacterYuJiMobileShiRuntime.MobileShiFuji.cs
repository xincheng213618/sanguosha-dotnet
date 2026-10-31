#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuJiMobileShiRuntime
{
    internal const string MobileShiFujiSkill = "mobile:shi-fuji";
    internal const string MobileShiFujiEmpoweredState = "fuji-empowered";
    internal const string MobileShiFujiSuitGainMoveReason = "skill-program.mobile:shi-fuji.suit-gain";
    internal const string MobileShiFujiBonusDrawReason = "skill-program.mobile:shi-fuji.bonus-draw";
    internal const string MobileShiFujiDodgeDrawReason = "skill-program.mobile:shi-fuji.dodge-draw";
    SkillProgramStepOutcome IMobileShiYuJiProgramHost.ResolveFujiGift(ProgramSkillFrame frame) => ResolveMobileShiFujiGiftProgram(frame);
    SkillProgramStepOutcome IMobileShiYuJiProgramHost.ResolveFujiUseEffects(ProgramSkillFrame frame) => ResolveMobileShiFujiUseEffectsProgram(frame);
    SkillProgramStepOutcome IMobileShiYuJiProgramHost.ResolveFujiDodgeDraw(ProgramSkillFrame frame) => ResolveMobileShiFujiDodgeDrawProgram(frame);
    internal SkillProgramStepOutcome ResolveMobileShiFujiGiftProgram(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.ActivationId != MobileShiYuJiComposition.FujiGiftActivation || f.TriggerId is not null || f.WindowContext is not null || f.SelectedTargetSeats is not [var recipient] || recipient == f.OwnerSeat)
            throw new InvalidOperationException("Fuji gift requires its own activation frame and one exact other recipient.");
        if (f.SelectedCardIds.Count == 0 || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return SkillProgramStepOutcome.Continue;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!;
        var source = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var recipientHand = CardLocation.Hand(recipient);
        var marked = 0;
        foreach (var cardId in f.SelectedCardIds)
        {
            if (_cardZonesOperations._cardZones.GetLocation(cardId) != recipientHand)
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(recipientHand).SingleOrDefault(item => item.Id == cardId);
            if (card is null)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiFujiCardMarkedEvent(f.Id, source, program.GameplayHash, f.OwnerSeat, recipient, cardId, card.Kind, card.Suit));
            marked++;
        }

        if (marked > 0)
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 符济：{(marked)} 张牌成为“符济”牌。", f.OwnerSeat);
        var minimum = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Min(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
        if (marked == 0 || _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > minimum)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiFujiEmpowerIssuedEvent(f.Id, f.OwnerSeat));
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, f.SkillId, f.SkillInstanceId, MobileShiFujiEmpoweredState, true);
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 符济：手牌数为全场最少，摸一张牌且首张【杀】和【闪】带有“符济”牌效果。", f.OwnerSeat);
        _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, new(MobileShiFujiBonusDrawReason));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsMobileShiFujiEmpoweredActive(ProgramSkillFrame f) => _coreProgramPersistentStateOperations.GetProgramBooleanState2(f.OwnerSeat, f.SkillId, f.SkillInstanceId, MobileShiFujiEmpoweredState);
    internal int LastMobileShiFujiEmpowerIndex(IGameEvent[] history, int ownerSeat)
    {
        for (var i = history.Length - 1; i >= 0; i--)
            if (history[i] is MobileShiFujiEmpowerIssuedEvent { OwnerSeat: var seat } && seat == ownerSeat)
                return i;
        return -1;
    }

    internal bool GainMobileShiFujiSuitCard(int userSeat, int usedCardId, Suit suit, long actionId)
    {
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(item => item.Suit == suit);
        if (card is null)
            return false;
        var sequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(userSeat), new(MobileShiFujiSuitGainMoveReason));
        var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? sequence;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiFujiSuitGainIssuedEvent(actionId, userSeat, usedCardId, suit, card.Id, after));
        return true;
    }

    internal SkillProgramStepOutcome ResolveMobileShiFujiUseEffectsProgram(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.TriggerId != MobileShiYuJiComposition.FujiUseEffectsTrigger || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } })
            throw new InvalidOperationException("Fuji use effects require their exact committed card-use window.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return SkillProgramStepOutcome.Continue;
        // The committed window freezes its exact action on the owning window frame;
        // the accepted fact only enters history after that window completes.
        var action = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(item => item.Id == f.WindowContext.ParentFrameId)?.Action;
        if (action is null)
            return SkillProgramStepOutcome.Continue;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var issued = false;
        var gained = 0;
        foreach (var cost in action.PhysicalCards)
        {
            if (!history.OfType<MobileShiFujiCardMarkedEvent>().Any(item => item.CardId == cost.CardId && item.Source.SkillId == MobileShiFujiSkill && item.Source.SkillInstanceId == f.SkillInstanceId))
                continue;
            if (history.OfType<MobileShiFujiCardUsedEvent>().Any(item => item.CardId == cost.CardId && !item.Empowered))
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cost.CardId)).SingleOrDefault(item => item.Id == cost.CardId);
            if (card is null)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiFujiCardUsedEvent(action.ActionId, action.ActorSeat, cost.CardId, card.Kind, MobileShiYuJiComposition.Name(card.Kind), Empowered: false));
            if (GainMobileShiFujiSuitCard(action.ActorSeat, cost.CardId, card.Suit, action.ActionId))
                gained++;
            issued = true;
        }

        var normalized = MobileShiYuJiComposition.Name(action.EffectiveKind);
        if (action.ActorSeat == owner.Seat && normalized is CardKind.Slash or CardKind.Dodge && IsMobileShiFujiEmpoweredActive(f) && !history.Skip(LastMobileShiFujiEmpowerIndex(history, owner.Seat) + 1).OfType<MobileShiFujiCardUsedEvent>().Any(item => item.Empowered && item.UserSeat == owner.Seat && item.Name == normalized))
        {
            var first = action.PhysicalCards.Count > 0 ? action.PhysicalCards[0] : null;
            var firstCard = first is { } leading ? _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(leading.CardId)).SingleOrDefault(item => item.Id == leading.CardId) : null;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiFujiCardUsedEvent(action.ActionId, owner.Seat, firstCard?.Id ?? 0, action.EffectiveKind, normalized, Empowered: true));
            if (firstCard is not null && GainMobileShiFujiSuitCard(owner.Seat, firstCard.Id, firstCard.Suit, action.ActionId))
                gained++;
            issued = true;
        }

        if (!issued)
            return SkillProgramStepOutcome.Continue;
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].Name)} 符济：使用了“符济”牌" + (gained > 0 ? $"，从牌堆获得 {(gained)} 张同花色牌。" : "。"), f.OwnerSeat);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome ResolveMobileShiFujiDodgeDrawProgram(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.TriggerId != MobileShiYuJiComposition.FujiDodgeDrawTrigger || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } use })
            throw new InvalidOperationException("Fuji's Dodge draw requires its exact response-completion window.");
        // The window's own completed-response fact carries the accepted action identity;
        // a finished response answers exactly one accepted action.
        var completedFact = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardResponseCompletedEvent>().SingleOrDefault(item => item.ActionId == use.CardActionId);
        if (completedFact is null || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return SkillProgramStepOutcome.Continue;
        var user = _coreMatchParticipantsPlayersAccess._players[completedFact.ActorSeat];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var used = history.OfType<MobileShiFujiCardUsedEvent>().Where(item => item.ActionId == completedFact.ActionId && item.Name == CardKind.Dodge).ToArray();
        if (used.Length == 0 || !user.IsAlive || !owner.IsAlive || history.OfType<MobileShiFujiDodgeDrawIssuedEvent>().Any(item => item.ActionId == completedFact.ActionId))
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiFujiDodgeDrawIssuedEvent(completedFact.ActionId, user.Seat, used[0].CardId, used[0].Empowered));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(user.Name)} 符济：【闪】结算完毕后摸一张牌。", f.OwnerSeat);
        _corePhysicalDrawFacadesOperations.DrawCards(user, 1, true, new(MobileShiFujiDodgeDrawReason));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    // 符济【杀】基础伤害值+1：已结算的符济使用事实按 ActionId 归属本次攻击。
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67210)]
    internal int MobileShiFujiSlashDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { } action } || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<MobileShiFujiCardUsedEvent>().Any(item => item.ActionId == action.ActionId && CoreNativeCardClassificationsRuntime.IsSlashCard(item.Kind)))
            return 0;
        return 1;
    }
}
