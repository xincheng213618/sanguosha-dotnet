using System.Windows.Input;
using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private HumanActionView? _zhuqueFanView;
    private HashSet<LegalAction> _zhuqueFanAlternatives = [];

    // Core retains both native actions for AI and command validation. Present the
    // weapon's alternative only after the player confirms the ordinary Slash.
    private HashSet<LegalAction> ZhuqueFanAlternatives
    {
        get
        {
            var view = GetViewAction();
            if (ReferenceEquals(view, _zhuqueFanView)) return _zhuqueFanAlternatives;
            _zhuqueFanView = view;
            if (!view.CanInvokeZhuqueFan) return _zhuqueFanAlternatives = [];
            var human = _snapshot.Players.SingleOrDefault(player => player.IsHuman);
            var physicalSlashes = human is null ? [] : PlayMaterials(human)
                .Where(card => (card.IdentityKind ?? card.Kind) == CardKind.Slash).Select(card => card.Id).ToHashSet();
            var ordinary = view.LegalActions.Where(action => action.Kind == LegalActionKind.Slash &&
                (action.PlayedCardKind == CardKind.Slash || action.PlayedCardKind is null &&
                    action.CardId is { } id && physicalSlashes.Contains(id))).ToArray();
            return _zhuqueFanAlternatives = view.LegalActions
                .Where(action => action is { Kind: LegalActionKind.Slash, PlayedCardKind: CardKind.FireSlash } &&
                    ordinary.Any(normal => SameZhuqueFanUse(normal, action))).ToHashSet();
        }
    }

    private static bool SameZhuqueFanUse(LegalAction normal, LegalAction fire) =>
        normal.CardId == fire.CardId && normal.ConversionSource == fire.ConversionSource &&
        (normal.AdditionalConversionSources ?? []).SequenceEqual(fire.AdditionalConversionSources ?? []) &&
        normal.TargetSeats.SequenceEqual(fire.TargetSeats) && normal.TargetCardId == fire.TargetCardId;

    private sealed record ZhuqueFanDraft(GameEngine Game, long Revision, PromptId PromptId,
        LegalAction Normal, LegalAction Fire, IReadOnlyList<PromptChoice> Choices);
    private ZhuqueFanDraft? _zhuqueFanSelection;
    private bool IsZhuqueFanSelectionPending => _zhuqueFanSelection is { } draft && ReferenceEquals(draft.Game, _game) &&
        draft.Revision == _snapshot.Revision && draft.PromptId == _snapshot.PendingDecision?.PromptId;
    private const string ZhuqueFanHint = "是否发动【朱雀羽扇】，将本次【杀】改为【火杀】？";
    private ICommand? _chooseZhuqueFanCommand;
    private ICommand ChooseZhuqueFanCommand => _chooseZhuqueFanCommand ??= new RelayCommand<PromptChoice>(ChooseZhuqueFan);

    private bool TryBeginZhuqueFanChoice(LegalAction normal, PromptId promptId)
    {
        if (normal.Kind != LegalActionKind.Slash) return false;
        var fire = ZhuqueFanAlternatives.SingleOrDefault(action => SameZhuqueFanUse(normal, action));
        if (fire is null) return false;
        _zhuqueFanSelection = new(_game, _snapshot.Revision, promptId, normal, fire,
        [
            new(new ChoiceId("zhuque-fan-invoke"), "发动朱雀羽扇", [], [], new Dictionary<string, string>()),
            new(new ChoiceId("zhuque-fan-decline"), "不发动 · 使用普通杀", [], [], new Dictionary<string, string>())
        ]);
        RefreshSelectionHint();
        return true;
    }

    private void ChooseZhuqueFan(PromptChoice choice)
    {
        if (!IsZhuqueFanSelectionPending || _zhuqueFanSelection is not { } draft ||
            !draft.Choices.Any(current => ReferenceEquals(current, choice))) return;
        _zhuqueFanSelection = null;
        var action = ReferenceEquals(choice, draft.Choices[0]) ? draft.Fire : draft.Normal;
        SubmitSelectedPlayAction(action.CardId!.Value, action, draft.PromptId, chooseZhuqueFan: false);
    }
}
