#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduExtraEffectsEntries
{
    internal bool GuanduRepeatTargetsRemainLegal(CardUseFrame use) => _resolve().GuanduRepeatTargetsRemainLegal(use);
}
