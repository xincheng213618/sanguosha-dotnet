#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingBoundaryEntries
{
    internal bool BanBoundaryJieyongUnavailableConversion(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().BanBoundaryJieyongUnavailableConversion(query);
    internal bool BanBoundaryJieyongUnavailableSubmittedSource(global::CardGame.Core.ProgramViewAsLegalityQuery query) => _resolve().BanBoundaryJieyongUnavailableSubmittedSource(query);
    internal bool IsBoundaryJieyongLastRedHand(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().IsBoundaryJieyongLastRedHand(owner, ids);
}
