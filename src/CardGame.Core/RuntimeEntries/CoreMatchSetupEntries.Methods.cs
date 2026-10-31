#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchSetupEntries
{
    internal void StartCore() => _resolve().StartCore();
}
