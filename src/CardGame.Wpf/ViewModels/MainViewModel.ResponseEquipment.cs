using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private GameSnapshot? _responseEquipmentSnapshot;
    private HashSet<ChoiceId> _zhangbaResponseChoiceIds = [];

    // Keep the engine's exact pairs for payment, but activate the draft from the weapon slot.
    // Unrepresented equipment effects retain their published central choices.
    private IReadOnlySet<ChoiceId> ZhangbaResponseChoiceIds
    {
        get
        {
            if (ReferenceEquals(_responseEquipmentSnapshot, _snapshot)) return _zhangbaResponseChoiceIds;
            _responseEquipmentSnapshot = _snapshot;
            _zhangbaResponseChoiceIds = [];
            if (!IsHandResponsePending || IsBaguaActivationPending ||
                !_snapshot.Players.Single(player => player.IsHuman).Equipment.Any(card => card.Kind == CardKind.ZhangbaSerpentSpear))
                return _zhangbaResponseChoiceIds;
            var handIds = HandResponseMaterials.Select(card => card.Id).ToHashSet();
            _zhangbaResponseChoiceIds = _snapshot.PendingDecision!.Choices.Where(choice =>
                choice.Parameters.GetValueOrDefault("response") == "zhangba-slash" &&
                choice.Parameters.GetValueOrDefault("equipment") == nameof(CardKind.ZhangbaSerpentSpear) &&
                choice.Cards.Count == 2 && choice.Cards.Distinct().Count() == 2 && choice.Cards.All(handIds.Contains))
                .Select(choice => choice.Id).ToHashSet();
            return _zhangbaResponseChoiceIds;
        }
    }

    private bool CanActivateResponseEquipment(CardSnapshot? card) => card?.Kind == CardKind.ZhangbaSerpentSpear &&
        ZhangbaResponseChoiceIds.Count > 0;

    private bool IsSelectedResponseEquipment(CardSnapshot? card) => card is not null &&
        IsResponseConversionSelectionPending && _responseConversionSelection!.EquipmentKind == card.Kind;

    private void SelectResponseEquipment(CardSnapshot card)
    {
        if (!CanActivateResponseEquipment(card)) return;
        var cancel = IsSelectedResponseEquipment(card);
        ClearSelection();
        if (cancel) return;
        _responseConversionSelection = new(null, null, SelectByMaterials: true, EquipmentKind: card.Kind);
        RefreshResponseConversionDraft();
    }
}
