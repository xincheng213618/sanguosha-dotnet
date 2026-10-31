#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingBoundaryEntries
{
    internal bool BanBoundaryJieyongUnavailableConversion(ProgramConfiguredViewAsQuery query) => _resolve().BanBoundaryJieyongUnavailableConversion(query);
    internal bool BanBoundaryJieyongUnavailableSubmittedSource(ProgramViewAsLegalityQuery query) => _resolve().BanBoundaryJieyongUnavailableSubmittedSource(query);
    internal bool IsBoundaryJieyongLastRedHand(CharacterState owner, IReadOnlyList<int> ids) => _resolve().IsBoundaryJieyongLastRedHand(owner, ids);
}
