#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    internal static bool IsMoGangquanSource(CardConversionSource? s) => RuntimeSkills.RuntimeRules_lu_bu_ol_magic_MoLuBuGangquan.IsMoGangquanSource(s);
    internal bool CanUseMoGangquan(CharacterState owner, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse, bool dyingUse)
    {
        if (instance.SkillId != MoLuBuComposition.Gangquan)
            return true;
        if (rule.OutputKind == CardKind.Duel)
            return !MoLuBuLastUseWasQi(owner.Seat) && !forResponse;
        return MoLuBuLastUseWasQi(owner.Seat) && !dyingUse && (!forResponse && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner.Seat && _resolutionStackOperations._resolutionStack.Count == 0 || _characterCaoZhiLeOperations.FuleTrueSlashUseFrontier(owner));
    }

    internal bool MoGangquanPhysicalFire(CharacterState owner, Card card) => IsMoGangquanMaterial(owner, card);
    internal bool IsMoGangquanMaterial(CharacterState owner, Card card) => HasMoLuBu && card.Id > 0 && _coreNationalSkillsOperations.HasRuntimeSkill(owner, MoLuBuComposition.Gangquan) && _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.FireSlash, false).Any(IsMoGangquanSource);
    internal bool IsMoGangquanMaterial(int owner, CardKind kind, IReadOnlyList<int>? ids) => HasMoLuBu && kind == CardKind.FireSlash && ids is [var id] && id > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[owner], MoLuBuComposition.Gangquan) && _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(_coreMatchParticipantsPlayersAccess._players[owner], id)is { } card && IsMoGangquanMaterial(_coreMatchParticipantsPlayersAccess._players[owner], card);
    internal bool MoGangquanTargetAllowed(CharacterState actor, CharacterState target, CardKind kind, CardConversionSource? source, IReadOnlyList<int>? ids = null) => !(IsMoGangquanSource(source) || IsMoGangquanMaterial(actor.Seat, kind, ids)) || _coreNativeRuleQueriesOperations.GetAliveSeatDistance(actor.Seat, target.Seat) == 1;
    internal bool MoGangquanBorrowedDistance(CharacterState actor, CharacterState target, CardKind kind, IReadOnlyList<int>? ids, CardConversionSource? source = null) => kind == CardKind.FireSlash && target.IsAlive && target.Seat != actor.Seat && _coreNativeRuleQueriesOperations.GetAliveSeatDistance(actor.Seat, target.Seat) == 1 && (IsMoGangquanSource(source) || IsMoGangquanMaterial(actor.Seat, kind, ids) || ids is null && _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Any(card => IsMoGangquanMaterial(actor, card)));
    internal IReadOnlyList<int> MoGangquanTargets(CharacterState actor, Card card, CardConversionSource source)
    {
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && target.Seat != actor.Seat && _coreNativeRuleQueriesOperations.GetAliveSeatDistance(actor.Seat, target.Seat) == 1 && _coreRuleQueriesOperations.CanSpendSlashUse(actor, target, false, CardKind.FireSlash, card) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, card, source, CardKind.FireSlash, ignoreDistance: true)).OrderBy(target => _coreTableOrderOperations.RelativeTableOrder(actor.Seat, target.Seat)).Select(target => target.Seat).ToArray());
    }

    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 30800)]
    internal void AddMoGangquanActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        actions.RemoveAll(a => IsMoGangquanSource(a.ConversionSource));
        foreach (var card in playable.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(c => c.Id).Where(c => selectedPhysicalCardId is null || c.Id == selectedPhysicalCardId))
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.FireSlash, false).Where(IsMoGangquanSource))
            {
                if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card))
                    continue;
                var targets = MoGangquanTargets(actor, card, source);
                if (targets.Count == 0)
                    continue;
                actions.Add(new(LegalActionKind.Slash, card.Id, targets[0], "罡拳：将装备牌当火杀使用，指定全部合法相邻角色", CardKind.FireSlash, TargetSeats: targets) { ConversionSource = source });
            }
    }

    internal bool IsMoGangquanMultiSlash(CharacterState actor, Card card, CardKind kind, CardConversionSource? source, IReadOnlyList<int> targets) => kind == CardKind.FireSlash && IsMoGangquanSource(source) && targets.SequenceEqual(MoGangquanTargets(actor, card, source!));
    internal IReadOnlyList<int> MoGangquanNativeTargets(int actor, Card card, CardKind kind, CardConversionSource? source, IReadOnlyList<int> original)
    {
        source ??= _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        if (kind != CardKind.FireSlash || !IsMoGangquanSource(source))
            return original;
        var targets = MoGangquanTargets(_coreMatchParticipantsPlayersAccess._players[actor], card, source!);
        if (targets.Count == 0 || original.Any(target => !targets.Contains(target)))
            throw new InvalidOperationException("Gangquan must use its complete legal living-neighbor target plan.");
        return targets;
    }

    internal CharacterState MoGangquanFirstTarget(CharacterState actor, CharacterState fallback, Card card, CardKind kind, CardConversionSource? source)
    {
        source ??= _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        if (kind != CardKind.FireSlash || !IsMoGangquanSource(source))
            return fallback;
        var targets = MoGangquanTargets(actor, card, source!);
        return targets.Count > 0 ? _coreMatchParticipantsPlayersAccess._players[targets[0]] : fallback;
    }

    internal IReadOnlyList<LegalAction> FilterMoGangquanActions(CharacterState actor, IEnumerable<LegalAction> actions) => Array.AsReadOnly(actions.Where(a => !IsMoGangquanSource(a.ConversionSource) || a.CardId is { } id && _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, id)is { } card && a.TargetSeats.SequenceEqual(MoGangquanTargets(actor, card, a.ConversionSource!))).ToArray());
    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 40)]
    internal bool MoGangquanConversionUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseMoGangquan(query.Owner, query.Instance, query.Rule, query.ForResponse, query.DyingUse);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 10)]
    internal bool DistanceBorrowedSlash10MoGangquanBorrowedDistance(ProgramSlashDistanceQuery q) => MoGangquanBorrowedDistance(q.Actor, q.Target, q.Kind, q.PhysicalCardIds, q.Conversion);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 10)]
    internal bool DistanceBorrowedPhysicalSlash10MoGangquanBorrowedDistance(ProgramSlashDistanceQuery q) => MoGangquanBorrowedDistance(q.Actor, q.Target, q.Kind, [q.PhysicalCard!.Id]);
}
