#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramOrdinaryTrickUsesRuntime
{
    internal SkillProgramStepOutcome UseProgramAllHandCardsAsOrdinaryTrick(ProgramSkillFrame frame, string viewAsId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.TriggerId is not null || active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id)
            throw new InvalidOperationException("An all-hand ordinary-trick use requires the current active program activation.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).OrderBy(card => card.Id).ToArray();
        if (hand.Length == 0 || !active.SelectedCardIds.Order().SequenceEqual(hand.Select(card => card.Id)) || hand.Any(card => _coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card)))
            throw new InvalidOperationException("The all-hand ordinary-trick use no longer owns exactly its selected unrestricted hand cards.");
        var paused = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!).GetPausedInstruction(active.InstructionIndex).Effect;
        var options = BuildProgramOrdinaryTrickUseOptions(owner, paused.OutputKind, enforceUsePermission: true, physicalCardIds: active.SelectedCardIds, includeNextActualUseAdjustment: true);
        if (options.Count == 0)
            throw new InvalidOperationException("No ordinary trick is currently legal for the selected hand cards.");
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, owner.Seat, $"【{(skill.Name)}】已选择全部 {(hand.Length)} 张手牌，请选择要视为使用的普通锦囊及其合法目标。", active.SelectedCardIds, options.SelectMany(option => option.TargetSeats).Distinct().Order().ToArray(), SourceSeat: owner.Seat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(active.SkillId, skill.Name, $"{(skill.Name)} · 选择普通锦囊", skill.Description),
            Choices = options.Select(option => CreateProgramOrdinaryTrickPromptChoice(active, viewAsId, option)).ToArray()
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("use-all-hand-as-ordinary-trick")]
    internal void ResolveProgramOrdinaryTrickUseChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The ordinary-trick choice lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.UseAllHandCardsAsOrdinaryTrick || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("view-as-id") != effect.SourceBind)
            throw new InvalidOperationException("The ordinary-trick choice does not match its suspended program instruction.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).OrderBy(card => card.Id).ToArray();
        if (!owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId) || hand.Length == 0 || !frame.SelectedCardIds.Order().SequenceEqual(hand.Select(card => card.Id)) || hand.Any(card => _coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card)))
            throw new InvalidOperationException("The ordinary-trick choice lost its owner, skill instance or exact all-hand cost.");
        var option = BuildProgramOrdinaryTrickUseOptions(owner, effect.OutputKind, enforceUsePermission: true, physicalCardIds: frame.SelectedCardIds, includeNextActualUseAdjustment: true).SingleOrDefault(candidate => candidate.Id == selected.Id) ?? throw new InvalidOperationException("The selected ordinary-trick use is no longer legal.");
        if (!selected.Targets.SequenceEqual(option.TargetSeats))
            throw new InvalidOperationException("The ordinary-trick targets changed while the choice was pending.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var representative = hand[0];
        var physicalCardIds = Array.AsReadOnly(hand.Select(card => card.Id).ToArray());
        var source = new CardConversionSource(frame.SkillId, effect.SourceBind!, owner.Seat, frame.SkillInstanceId);
        if (option.NextActualUseAdjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(representative, owner.Seat, option.TargetSeats, option.EffectiveCardKind, physicalCardIds: physicalCardIds, conversionSource: source);
        _corePhysicalMovementFacadesOperations.MoveCards(hand, CardLocation.Hand(owner.Seat), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(resolutionId, frame.SkillId, effect.SourceBind!, owner.Seat, physicalCardIds, option.EffectiveCardKind, IsUse: true, option.TargetSeats));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Name)}】，将全部 {(physicalCardIds.Count)} 张手牌当【{(CardCatalog.Get(option.EffectiveCardKind).DisplayName)}】使用。", owner.Seat, option.TargetSeats.FirstOrDefault(-1));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, representative, owner.Seat, option.TargetSeats, option.ActionKind, option.TargetCardId, option.RequiredCardKind, option.EffectiveCardKind);
    }

    internal IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(CharacterState source, CardKind? outputKind = null, Suit? physicalSuit = null, bool excludeOwner = false, bool enforceUsePermission = false, Suit? beneficiaryShieldSuit = null, bool? actualEffectiveColor = null, bool hasActualColor = false, IReadOnlyList<int>? physicalCardIds = null, bool includeNextActualUseAdjustment = false, CardConversionSource? conversionSource = null)
    {
        var options = new List<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption>();
        bool Wants(CardKind kind) => _modeRulesOperations._modeRules.Cards.AllowsOrdinaryTrickOption(kind, outputKind);
        var effectiveColor = hasActualColor ? actualEffectiveColor : _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(source, _corePhysicalOwnedCardQueriesOperations.GetHand(source));
        // A multi-card virtual card only carries a suit while every physical card
        // shares it; mixed-suit combinations are colorless and pass suit shields.
        var virtualSuit = physicalSuit;
        if (virtualSuit is null)
        {
            var handSuits = _corePhysicalOwnedCardQueriesOperations.GetHand(source).Select(card => card.Suit).Distinct().ToArray();
            virtualSuit = handSuits.Length == 1 ? handSuits[0] : null;
        }

        var shieldSuit = beneficiaryShieldSuit ?? physicalSuit;
        if (shieldSuit is null)
        {
            var effectiveHandSuits = _corePhysicalOwnedCardQueriesOperations.GetHand(source).Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(source, c)).Distinct().ToArray();
            shieldSuit = effectiveHandSuits.Length == 1 ? effectiveHandSuits[0] : null;
        }

        void Add(CardKind cardKind, LegalActionKind actionKind, string description, IReadOnlyList<int>? targetSeats = null, int? targetCardId = null, CardKind? requiredCardKind = null)
        {
            var targets = targetSeats?.ToArray() ?? [];
            var declared = cardKind == CardKind.BorrowedSword ? targets.Take(1) : targets.AsEnumerable();
            if (cardKind is CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains or CardKind.AllianceFeast or CardKind.ReturnSoul or CardKind.StriveUpstream)
                targets = targets.Where(t => !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, t, shieldSuit) && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(source.Seat, t, cardKind))).ToArray();
            else if (declared.Any(t => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, t, shieldSuit)))
                return;
            if (cardKind == CardKind.ReturnSoul && targets.Length == 0)
                return;
            if (cardKind == CardKind.StriveUpstream && targets.Length == 0)
                return;
            if (_coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess._beneficiarySuitShields.Count > 0 && targets.Length == 0 && cardKind is CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains)
                return;
            var targetPart = targets.Length == 0 ? "none" : string.Join('-', targets);
            var cardPart = targetCardId?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "none";
            options.Add(new global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption(new ChoiceId($"ordinary-trick.{(cardKind)}.targets-{(targetPart)}.card-{(cardPart)}"), cardKind, actionKind, Array.AsReadOnly(targets), targetCardId, requiredCardKind, description));
        }

        if (Wants(CardKind.DrawTwo) && !excludeOwner && !_coreDyingWindowSkillPoliciesOperations.IsBlackTrickTargetProhibited(source, CardKind.DrawTwo, effectiveColor))
            Add(CardKind.DrawTwo, LegalActionKind.DrawTwo, "当【无中生有】使用：摸两张牌");
        foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsDefinitions.GuanduOrdinaryTrickKinds.Where(Wants))
            foreach (var targets in _coreGuanduCardsOperations.GuanduTargetChoices(source, kind, virtualSuit, effectiveColor, excludeOwner))
                Add(kind, global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime.GuanduCardAction(kind), $"当【{(CardCatalog.Get(kind).DisplayName)}】使用", targets);
        foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsDefinitions.DragonBoatOrdinaryTrickKinds.Where(Wants))
        {
            var seats = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive && (!excludeOwner || player.Seat != source.Seat) && (kind == CardKind.StriveUpstream || (kind == CardKind.SameBoat) == _coreDragonBoatCardsOperations.DragonBoatCardAllies(source.Seat, player.Seat)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, kind, virtualSuit, effectiveColor) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(source.Seat, player.Seat, kind) && (kind != CardKind.StriveUpstream || !_coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind))).Select(player => player.Seat).ToArray();
            if (kind == CardKind.StriveUpstream)
            {
                if (seats.Length > 0)
                    Add(kind, global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime.DragonBoatCardAction(kind), "当【力争上游】使用", seats);
            }
            else
                foreach (var seat in seats)
                    Add(kind, global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime.DragonBoatCardAction(kind), $"当【{(CardCatalog.Get(kind).DisplayName)}】对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 使用", [seat]);
        }

        if (Wants(CardKind.QizhengXiangsheng))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.QizhengXiangsheng) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.QizhengXiangsheng, virtualSuit, effectiveColor) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(source.Seat, player.Seat, CardKind.QizhengXiangsheng)))
                Add(CardKind.QizhengXiangsheng, LegalActionKind.QizhengXiangsheng, $"当【奇正相生】对 {(target.Name)} 使用", [target.Seat]);
        if (outputKind == CardKind.WumiaoFlood)
        {
            var eligible = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && (!excludeOwner || player.Seat != source.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.WumiaoFlood) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.WumiaoFlood, virtualSuit, effectiveColor) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(source.Seat, player.Seat, CardKind.WumiaoFlood)).Select(player => player.Seat).ToArray();
            foreach (var first in eligible)
            {
                Add(CardKind.WumiaoFlood, LegalActionKind.WumiaoFlood, $"当【水淹七军】对 {(_coreMatchParticipantsPlayersAccess._players[first].Name)} 使用", [first]);
                foreach (var second in eligible.Where(seat => seat != first))
                    Add(CardKind.WumiaoFlood, LegalActionKind.WumiaoFlood, $"当【水淹七军】依次对 {(_coreMatchParticipantsPlayersAccess._players[first].Name)}、{(_coreMatchParticipantsPlayersAccess._players[second].Name)} 使用", [first, second]);
            }
        }

        foreach (var kind in _coreWenheCardsOperations.WenheOrdinaryTrickKinds.Where(Wants))
        {
            var self = kind is CardKind.BeamSwap or CardKind.WenheChaos;
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && (self ? player.Seat == source.Seat && !excludeOwner : player.Seat != source.Seat) && (kind != CardKind.StarShift || source.HasHpValue && player.HasHpValue && source.Hp >= 1 && player.Hp >= 1) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, kind, virtualSuit, effectiveColor) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(source.Seat, player.Seat, kind)))
                Add(kind, global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime.WenheCardAction(kind), $"当【{(CardCatalog.Get(kind).DisplayName)}】对 {(target.Name)} 使用", [target.Seat]);
        }

        if (Wants(CardKind.ReturnSoul))
        {
            var souls = _coreHellSoulCardsOperations.HellReturnSoulTargets(source.Seat).Where(seat => (!excludeOwner || seat != source.Seat) && !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.ReturnSoul) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.ReturnSoul, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, seat, CardKind.ReturnSoul)).ToArray();
            if (souls.Length > 0)
                Add(CardKind.ReturnSoul, LegalActionKind.ReturnSoul, "当【回魂】使用", souls);
        }

        if (Wants(CardKind.SowBeans) && !excludeOwner && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, source.Seat, CardKind.SowBeans) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(source, CardKind.SowBeans, virtualSuit, effectiveColor))
            Add(CardKind.SowBeans, LegalActionKind.SowBeans, "当【撒豆成兵】对自己使用", [source.Seat]);
        if (Wants(CardKind.TransplantWood))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Count > 0 && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.TransplantWood, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.TransplantWood)))
                Add(CardKind.TransplantWood, LegalActionKind.TransplantWood, $"当【移花接木】对 {(target.Name)} 使用", [target.Seat]);
        if (Wants(CardKind.AllianceFeast) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.AllianceFeast, excludeOwner))
        {
            var feastTargets = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive && (!excludeOwner || player.Seat != source.Seat) && !_coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.AllianceFeast) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.AllianceFeast, virtualSuit, effectiveColor)).Select(player => player.Seat).ToArray();
            if (feastTargets.Length > 0)
                Add(CardKind.AllianceFeast, LegalActionKind.AllianceFeast, "当【联军盛宴】使用", feastTargets);
        }

        if (Wants(CardKind.BarbarianAssault) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.BarbarianAssault, excludeOwner) || Wants(CardKind.ArrowBarrage) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.ArrowBarrage, excludeOwner))
        {
            var otherSeats = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive).Select(player => player.Seat).ToArray();
            var barbarianTargets = !Wants(CardKind.BarbarianAssault) ? [] : otherSeats.Where(seat => !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.BarbarianAssault) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.BarbarianAssault, virtualSuit, effectiveColor)).ToArray();
            var arrowTargets = !Wants(CardKind.ArrowBarrage) ? [] : otherSeats.Where(seat => !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.ArrowBarrage, virtualSuit, effectiveColor)).ToArray();
            if (Wants(CardKind.BarbarianAssault) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.BarbarianAssault, excludeOwner) && barbarianTargets.Length > 0)
                Add(CardKind.BarbarianAssault, LegalActionKind.BarbarianAssault, "当【南蛮入侵】使用：其他角色依次响应【杀】", barbarianTargets, requiredCardKind: CardKind.Slash);
            if (Wants(CardKind.ArrowBarrage) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.ArrowBarrage, excludeOwner) && arrowTargets.Length > 0)
                Add(CardKind.ArrowBarrage, LegalActionKind.ArrowBarrage, "当【万箭齐发】使用：其他角色依次响应【闪】", arrowTargets, requiredCardKind: CardKind.Dodge);
        }

        var allAliveSeats = !Wants(CardKind.PeachGarden) && !Wants(CardKind.FiveGrains) ? [] : Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive && (!excludeOwner || player.Seat != source.Seat)).Select(player => player.Seat).ToArray();
        if (Wants(CardKind.PeachGarden) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.PeachGarden, excludeOwner))
        {
            var peachTargets = allAliveSeats.Where(seat => !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.PeachGarden, virtualSuit, effectiveColor)).ToArray();
            if (peachTargets.Length > 0)
                Add(CardKind.PeachGarden, LegalActionKind.PeachGarden, "当【桃园结义】使用：所有存活角色依次回复体力", peachTargets);
        }

        var availableCards = _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile);
        if (Wants(CardKind.FiveGrains) && _coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(source, CardKind.FiveGrains, excludeOwner))
        {
            var grainTargets = allAliveSeats.Where(seat => !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.FiveGrains, virtualSuit, effectiveColor)).Take(availableCards).ToArray();
            if (grainTargets.Length > 0)
                Add(CardKind.FiveGrains, LegalActionKind.FiveGrains, "当【五谷丰登】使用：所有存活角色依次选牌", grainTargets);
        }

        if (Wants(CardKind.Duel))
        {
            var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.Duel, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.Duel)).Select(player => player.Seat).ToArray();
            foreach (var targets in _coreProgramMultiTargetDuelsOperations.EnumerateProgramDuelTargets(source, candidates))
                Add(CardKind.Duel, LegalActionKind.Duel, "当【决斗】对 " + string.Join("、", targets.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)) + " 使用", targets);
        }

        if (Wants(CardKind.Dismantlement))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(player) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.Dismantlement, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.Dismantlement)))
                AddProgramOrdinaryTrickTargetCardOptions(options, source, target, CardKind.Dismantlement, LegalActionKind.Dismantlement, "过河拆桥");
        if (Wants(CardKind.Snatch))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat && (_characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(source, CardKind.Snatch, physicalCardIds, conversionSource) || _characterHuangfuSongWumiaoOperations.HasWuHfOrWuyouMaterialPrivilege(source, CardKind.Snatch, physicalCardIds) || _characterMiHengLeOperations.SiruiIgnoresTrickDistance(source, conversionSource, physicalCardIds) || _characterSunQuanAnniversaryGodOperations.IsAnniversaryChouxiSource(source, conversionSource) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(source.Seat, shieldSuit) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(source, player, CardKind.Snatch, physicalCardIds: physicalCardIds) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance(source, physicalCardIds) || _coreProgramCardPoliciesOperations.HasCardPolicy(source, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.Snatch) || _coreNativeRuleQueriesOperations.GetCombatDistance(source.Seat, player.Seat) == 1) && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(player) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.Snatch, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.Snatch)))
                AddProgramOrdinaryTrickTargetCardOptions(options, source, target, CardKind.Snatch, LegalActionKind.Snatch, "顺手牵羊");
        if (Wants(CardKind.FireAttack))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > 0 && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.FireAttack, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.FireAttack)))
                Add(CardKind.FireAttack, LegalActionKind.FireAttack, $"当【火攻】对 {(target.Name)} 使用", [target.Seat]);
        if (Wants(CardKind.IronChain))
        {
            var chainTargets = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.IronChain, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.IronChain)).OrderBy(player => player.Seat).ToArray();
            foreach (var targets in _coreProgramIronChainTargetsOperations.EnumerateProgramIronChainTargets(source, chainTargets))
                Add(CardKind.IronChain, LegalActionKind.IronChain, $"当【铁索连环】对 {(string.Join("、", targets.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)))} 使用", targets);
        }

        if (Wants(CardKind.BorrowedSword))
            foreach (var weaponOwner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat && _coreNativeBorrowedSwordOperations.GetWeapon(player)is not null && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.BorrowedSword, virtualSuit, effectiveColor) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, player.Seat, CardKind.BorrowedSword)))
                foreach (var slashTarget in _coreMatchParticipantsPlayersAccess._players.Where(player => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(weaponOwner, player)))
                    Add(CardKind.BorrowedSword, LegalActionKind.BorrowedSword, $"当【借刀杀人】使用：令 {(weaponOwner.Name)} 对 {(slashTarget.Name)} 使用【杀】，否则获得其武器", [weaponOwner.Seat, slashTarget.Seat]);
        var legalOptions = Array.AsReadOnly(options.Where(option => (outputKind is null || option.EffectiveCardKind == outputKind) && (!excludeOwner || !option.TargetSeats.Contains(source.Seat)) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(source, option.EffectiveCardKind, option.TargetSeats) && (option.EffectiveCardKind is CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains or CardKind.AllianceFeast or CardKind.ReturnSoul || (option.EffectiveCardKind == CardKind.BorrowedSword ? option.TargetSeats.Where((_, index) => index % 2 == 0) : option.TargetSeats).All(t => !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, t, shieldSuit))) && !_coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(source.Seat) && !_coreNextActualUseTargetAdjustmentsOperations.IsNextActualUseTrickForbidden(source.Seat, option.EffectiveCardKind) && (!enforceUsePermission || !_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, option.EffectiveCardKind, CardActionType.Use))).ToArray());
        return includeNextActualUseAdjustment ? _coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(source, legalOptions, virtualSuit, effectiveColor, shieldSuit, excludeOwner) : legalOptions;
    }

    internal void AddProgramOrdinaryTrickTargetCardOptions(ICollection<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> options, CharacterState source, CharacterState target, CardKind cardKind, LegalActionKind actionKind, string cardName)
    {
        void AddTarget(int? targetCardId, string suffix)
        {
            var cardPart = targetCardId?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "none";
            options.Add(new global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption(new ChoiceId($"ordinary-trick.{(cardKind)}.targets-{(target.Seat)}.card-{(cardPart)}"), cardKind, actionKind, Array.AsReadOnly(new[] { target.Seat }), targetCardId, RequiredCardKind: null, $"当【{(cardName)}】对 {(target.Name)} 使用{(suffix)}"));
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0)
            AddTarget(null, "，选择其一张暗置手牌");
        foreach (var equipment in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target))
            AddTarget(equipment.Id, $"，选择其装备【{(equipment.DisplayName)}】");
        foreach (var judgment in _corePhysicalOwnedCardQueriesOperations.GetJudgment(target))
            AddTarget(judgment.Id, $"，选择其判定区【{(judgment.DisplayName)}】");
    }

    internal static PromptChoice CreateProgramOrdinaryTrickPromptChoice(ProgramSkillFrame frame, string viewAsId, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption option) => new(option.Id, option.Description, [], option.TargetSeats, new Dictionary<string, string> { ["program-action"] = "use-all-hand-as-ordinary-trick", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["view-as-id"] = viewAsId, ["card-kind"] = option.EffectiveCardKind.ToString(), ["action-kind"] = option.ActionKind.ToString(), ["target-card-id"] = option.TargetCardId?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty });
}
