#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFactionSlashCompletionEntries
{
    internal void CompleteProgramFactionCardRequest(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => _resolve().CompleteProgramFactionCardRequest(pending);
}
