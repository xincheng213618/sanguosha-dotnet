#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheAlliedTricksEntries
{
    internal bool RefreshShanheAlliedHarvestSelection() => _resolve().RefreshShanheAlliedHarvestSelection();
    internal bool ShanheAlliedHarvestProhibited(global::CardGame.Core.NativeOwners.GroupCardHandle group, int pickerSeat) => _resolve().ShanheAlliedHarvestProhibited(group, pickerSeat);
    internal int ShanheAlliedPeachGardenRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group, int targetSeat, int original) => _resolve().ShanheAlliedPeachGardenRecovery(group, targetSeat, original);
}
