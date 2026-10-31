#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanPingBoundaryRuntime
{
    internal bool IsBoundaryJieyongLastRedHand(CharacterState owner, IReadOnlyList<int> ids) => ids is [var id] && _cardZonesOperations._cardZones.Contains(CardLocation.Hand(owner.Seat), id) && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, c)) == true).Select(c => c.Id).ToArray()is [var red] && red == id;
    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 55400)]
    internal bool BanBoundaryJieyongUnavailableConversion(ProgramConfiguredViewAsQuery query) => query.Instance.SkillId == BoundaryGuanPingComposition.Jieyong && !IsBoundaryJieyongLastRedHand(query.Owner, [query.Card.Id]);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 55400)]
    internal bool BanBoundaryJieyongUnavailableSubmittedSource(ProgramViewAsLegalityQuery query) => query.Source.SkillId == BoundaryGuanPingComposition.Jieyong && !IsBoundaryJieyongLastRedHand(query.Owner, query.CardIds);
}
