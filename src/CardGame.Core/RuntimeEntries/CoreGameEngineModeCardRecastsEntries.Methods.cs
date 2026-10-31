#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeCardRecastsEntries
{
    internal bool CanModeHandEquipmentRecast(Card card) => _resolve().CanModeHandEquipmentRecast(card);
}
