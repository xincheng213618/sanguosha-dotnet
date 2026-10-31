#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundDistinctBasicRequestsRuntime
{
    internal const string RoundDistinctBasicRequestMarker = "round-distinct-basic-request";
    internal IReadOnlyList<PromptChoice> RoundDistinctBasicRequestedSlashChoices(ProgramSkillFrame frame, int actorSeat, IReadOnlyList<int> targetSeats, string? resultBind = null)
    {
        if (!(_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundDistinctBasicUse || _characterKongShuOlOperations.HasLeiluanCapability) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var choices = new List<PromptChoice>();
        var nearest = resultBind is null;
        var action = nearest ? "request-slash-nearest" : "request-slash";
        foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, CardKind.Slash, forResponse: false).Where(s => (_corePlayCardHistoryOperations.ViewAsRule(s.Source)is { RoundDistinctBasicUse: not null } or { LeiluanBasicUse: not null }) && CoreNativeCardClassificationsRuntime.IsSlashCard(s.OutputKind)).DistinctBy(s => (s.Source, s.OutputKind, Cards: string.Join('-', s.Cards.Select(c => c.Id).Order()))))
            foreach (var targetSeat in targetSeats)
            {
                if (!RoundDistinctBasicRequestedSlashTarget(actor, selection, targetSeat))
                    continue;
                var ids = selection.Cards.Select(card => card.Id).ToArray();
                var countLabel = _characterKongShuOlOperations.IsLeiluanSource(selection.Source) ? ids.Length.ToString(System.Globalization.CultureInfo.InvariantCulture) : "两";
                var parameters = new Dictionary<string, string>
                {
                    ["program-action"] = action,
                    ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["card-id"] = ids[0].ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["effective-kind"] = selection.OutputKind.ToString(),
                    [RoundDistinctBasicRequestMarker] = "true"
                };
                if (nearest)
                {
                    parameters["seat"] = actorSeat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    parameters["target-seat"] = targetSeat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                    parameters["result-bind"] = resultBind!;
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
                choices.Add(PromptChoiceProjection.Freeze(new PromptChoice(new ChoiceId($"program-{(action)}.frame-{(frame.Id)}.pair.{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.{(selection.Source.SkillInstanceId)}.{(selection.OutputKind)}.{(string.Join('-', ids))}.seat-{(targetSeat)}"), _coreCardConversionsOperations.DescribeConversion(selection.Source, $"将{(countLabel)}张牌当【{(CardCatalog.Get(selection.OutputKind).DisplayName)}】使用，攻击 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。"), ids, [targetSeat], parameters)));
            }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal bool RoundDistinctBasicRequestedSlashTarget(CharacterState actor, ProgramMultiCardViewAsSelection selection, int targetSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && actor.IsAlive && // These two existing requests supply their own fixed/nearest target
 // frontier and do not spend the normal Play Slash quota or add a range
    // precheck. Preserve that contract while validating this new true Use.
    _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], selection.Cards[0], selection.Source, selection.OutputKind, ignoreDistance: true, noEffectiveRank: true, physicalCardIds: selection.Cards.Select(card => card.Id).ToArray()) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], selection.OutputKind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, selection.Cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, selection.Cards));
    internal ProgramMultiCardViewAsSelection RequireRoundDistinctBasicRequestedSlash(ProgramSkillFrame frame, PendingDecision decision, PromptChoice selected, int actorSeat, IReadOnlyList<int> targetSeats, string? resultBind = null)
    {
        if (selected.Parameters.GetValueOrDefault(RoundDistinctBasicRequestMarker) != "true" || decision.Kind != DecisionKind.ProgramTrigger || !decision.IsPrivate || decision.PlayerSeat != actorSeat || decision.SourceSeat != frame.OwnerSeat || decision.TargetSeat != actorSeat || decision.SkillPrompt?.SkillId != frame.SkillId || decision.Choices.SingleOrDefault(choice => choice.Id == selected.Id)is not { } published || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([published], [selected]) || RoundDistinctBasicRequestedSlashChoices(frame, actorSeat, targetSeats, resultBind).SingleOrDefault(choice => choice.Id == selected.Id)is not { } canonical || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([canonical], [selected]) || selected.Cards.Count == 0 || selected.Targets.Count != 1 || !Enum.TryParse<CardKind>(selected.Parameters.GetValueOrDefault("effective-kind"), out var kind) || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            throw new InvalidOperationException("A requested pair Slash changed its published source, materials, kind or native target frontier.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        if (_corePlayCardHistoryOperations.ViewAsRule(source)is not ({ RoundDistinctBasicUse: not null } or { LeiluanBasicUse: not null }) || !_characterKongShuOlOperations.IsLeiluanSource(source) && selected.Cards.Count != 2 || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[actorSeat], selected.Cards, kind, false, source)is not { } selection || selection.OutputKind != kind || !RoundDistinctBasicRequestedSlashTarget(_coreMatchParticipantsPlayersAccess._players[actorSeat], selection, selected.Targets[0]))
            throw new InvalidOperationException("A requested pair Slash lost its exact currently legal conversion payment.");
        return selection;
    }
}
