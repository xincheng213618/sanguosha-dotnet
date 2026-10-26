using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool IsMaterialPlayConversionPending => IsPlayConversionSelectionPending &&
        _playConversionSelection!.SelectByMaterials;

    private IReadOnlyList<PlayConversionMode> MaterialPlayConversionModes => IsMaterialPlayConversionPending
        ? PlayConversionModes(_playConversionSelection!.SkillId) : [];

    private static int ConversionMaterialCount(PlayConversionMode mode) => mode.Activation?.MinCardCount ?? 1;

    private IReadOnlyList<int> ConversionMaterialIds(PlayConversionMode mode) => mode.Activation?.SelectableCardIds ??
        GetViewLegalActions().Where(mode.Matches).Select(action => action.CardId!.Value).Distinct().ToArray();

    // The material set must identify the exact published conversion, including its provenance.
    // Declarations such as Guhuo, and ambiguous conversion chains, keep their name chooser.
    private bool CanSelectPlayConversionByMaterials(IReadOnlyList<PlayConversionMode> modes)
    {
        if (modes.Count < 2 || modes.Any(mode => mode.Activation is { } action &&
                (action.MinCardCount < 1 || action.MinCardCount != action.MaxCardCount ||
                 action.SelectedCardsSameNameOrAllEquipment ||
                 action.SelectableCardTargets is not null || action.SelectableCardGroupTargets is not null)) ||
            modes.Where(mode => mode.Activation is null).Any(mode => GetViewLegalActions().Where(mode.Matches)
                .Any(action => action.TargetCardId is not null || action.TargetHandPreselection is not null ||
                    action.TargetSeats.Count > 1 || action.Kind == LegalActionKind.Recast)))
            return false;
        var materials = modes.Select(ConversionMaterialIds).ToArray();
        var human = _snapshot.Players.Single(player => player.IsHuman);
        var owned = human.Hand.Concat(human.Equipment).Concat(human.WoodenOxGrain ?? []).Select(card => card.Id).ToHashSet();
        if (materials.Any(ids => ids.Any(id => !owned.Contains(id)))) return false;
        for (var i = 0; i < modes.Count; i++)
        for (var j = i + 1; j < modes.Count; j++)
            if (ConversionMaterialCount(modes[i]) == ConversionMaterialCount(modes[j]) &&
                materials[i].Intersect(materials[j]).Take(ConversionMaterialCount(modes[i])).Count() == ConversionMaterialCount(modes[i]))
                return false;
        return true;
    }

    private bool MaterialPlayModeAccepts(PlayConversionMode mode, IEnumerable<int> ids)
    {
        var cards = ids.ToHashSet();
        return cards.Count <= ConversionMaterialCount(mode) && cards.All(ConversionMaterialIds(mode).Contains) &&
            (mode.Activation is not { } action || cards.Count == 0 || SelectedActiveSkillCardsMatchContract(action, cards));
    }

    private PlayConversionMode? SelectedMaterialPlayMode => MaterialPlayConversionModes.SingleOrDefault(mode =>
        ConversionMaterialCount(mode) == _selectedActiveSkillCardIds.Count && MaterialPlayModeAccepts(mode, _selectedActiveSkillCardIds));

    private LegalAction[] MaterialPlayNativeActions(PlayConversionMode mode) => GetViewLegalActions()
        .Where(action => mode.Matches(action) && action.CardId is { } id && _selectedActiveSkillCardIds.SetEquals([id])).ToArray();

    // Reuse the ordinary active-skill draft for hand/equipment selection and target portraits.
    // This aggregate is presentation metadata only; confirmation resolves a native action below.
    private LegalAction? MaterialPlaySelectionAction
    {
        get
        {
            var modes = MaterialPlayConversionModes;
            if (modes.Count == 0) return null;
            var mode = SelectedMaterialPlayMode;
            var native = mode is { Activation: null } ? MaterialPlayNativeActions(mode) : [];
            var basis = mode?.Activation ?? native.FirstOrDefault() ?? new LegalAction(LegalActionKind.UseProgramSkill, null, null, "选牌转化");
            var selectable = modes.SelectMany(ConversionMaterialIds).Distinct().Where(id =>
                _selectedActiveSkillCardIds.Contains(id) || modes.Any(candidate =>
                    MaterialPlayModeAccepts(candidate, _selectedActiveSkillCardIds.Append(id)))).ToArray();
            return basis with
            {
                ProgramSkillId = _playConversionSelection!.SkillId,
                PlayedCardKind = mode?.Kind,
                TargetSeat = null,
                TargetSeats = [],
                MinCardCount = modes.Min(ConversionMaterialCount),
                MaxCardCount = modes.Max(ConversionMaterialCount),
                SelectableCardIds = selectable,
                MinTargetCount = mode?.Activation?.MinTargetCount ?? (native.Length == 0 ? 0 : native.Min(action => action.TargetSeats.Count)),
                MaxTargetCount = mode?.Activation?.MaxTargetCount ?? (native.Length == 0 ? 0 : native.Max(action => action.TargetSeats.Count)),
                SelectableTargetSeats = mode?.Activation?.SelectableTargetSeats ?? native.SelectMany(action => action.TargetSeats).Distinct().ToArray(),
                SelectableTargetSequences = mode?.Activation is { } activation ? activation.SelectableTargetSequences :
                    native.Select(action => action.TargetSeats).ToArray()
            };
        }
    }

    private void ToggleMaterialPlayCard(int id)
    {
        if (MaterialPlaySelectionAction is not { } action || !action.SelectableCardIds.Contains(id)) return;
        var previousKind = SelectedMaterialPlayMode?.Kind;
        if (!_selectedActiveSkillCardIds.Remove(id))
        {
            _selectedActiveSkillCardIds.Add(id);
            if (!MaterialPlayConversionModes.Any(mode => MaterialPlayModeAccepts(mode, _selectedActiveSkillCardIds)))
            {
                _selectedActiveSkillCardIds.Remove(id);
                return;
            }
        }
        if (SelectedMaterialPlayMode?.Kind != previousKind) _selectedActiveSkillTargetSeats.Clear();
        if (MaterialPlaySelectionAction is { } updated)
            _selectedActiveSkillTargetSeats.RemoveAll(seat => !updated.SelectableTargetSeats.Contains(seat));
        SelectOnlyActiveSkillTarget();
        _humanSkillSnapshot = null;
        SelectedCardText = MaterialPlayConversionHint;
        RefreshCurrentView();
    }

    private string MaterialPlayConversionHint
    {
        get
        {
            var modes = MaterialPlayConversionModes;
            var range = modes.Count == 0 ? "" : FormatSelectionRange(modes.Min(ConversionMaterialCount), modes.Max(ConversionMaterialCount));
            var sameSuit = modes.Any(mode => mode.Activation?.SelectedCardsSameSuit == true) ? "；多张牌须同花色" : "";
            return $"{PlayConversionSkillName} · " + (SelectedMaterialPlayMode is { } selected
                ? $"已选 {_selectedActiveSkillCardIds.Count} 张牌，当【{CardCatalog.Get(selected.Kind).DisplayName}】使用 · " +
                  (CanConfirmActiveSkill ? "点击出牌或按 Enter 确认" : "选择亮起的目标后确认")
                : $"选择 {range} 张合法材料牌，按材料自动确定牌型{sameSuit}") + "；Esc 取消";
        }
    }

    private void ConfirmMaterialPlayConversion()
    {
        if (!CanConfirmActiveSkill || SelectedMaterialPlayMode is not { } mode || _snapshot.PendingDecision is not { } prompt) return;
        var cards = _selectedActiveSkillCardIds.Order().ToArray();
        var targets = _selectedActiveSkillTargetSeats.ToArray();
        GameCommand command;
        if (mode.Activation is { } activation)
            command = new UseProgramSkillCommand(_snapshot.HumanSeat, activation.ProgramSkillId!, activation.ProgramActivationId!,
                cards, targets, _snapshot.Revision, prompt.PromptId) { SkillOwnerSeat = activation.ProgramSkillOwnerSeat };
        else
        {
            var action = MaterialPlayNativeActions(mode).SingleOrDefault(candidate => candidate.TargetSeats.SequenceEqual(targets));
            if (action is null) return;
            command = new PlayCardCommand(_snapshot.HumanSeat, cards[0], action.TargetSeats, _snapshot.Revision, prompt.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource,
                AdditionalConversionSources = action.AdditionalConversionSources };
        }
        ExecuteSafely(() =>
        {
            var result = SubmitCommand(command);
            if (result.Accepted) ClearSelection();
            else PromptText = $"转化未执行：{result.Error?.Message}";
            RefreshCommandResult(result.State);
        });
    }
}
