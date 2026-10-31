#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePublicPileSourcesCoreRicePileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RiceCapability { get; }
}

internal interface ICorePublicPileSourcesCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiFengCapability { get; }
}

internal interface ICorePublicPileSourcesCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGuanHaiCapability { get; }
}

internal interface ICorePublicPileSourcesCharacterGanNingMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouGanNingMobile { get; }
}

internal interface ICorePublicPileSourcesCharacterXiahouDunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouXiahouDunCapability { get; }
}

internal interface ICorePublicPileSourcesCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouGszMobile { get; }
}

internal interface ICorePublicPileSourcesCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICorePublicPileSourcesCharacterGuoHuaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouGuoHuaiMobile { get; }
}

internal interface ICorePublicPileSourcesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePublicPileSourcesCharacterDengAiMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouDengAiOl { get; }
}

internal interface ICorePublicPileSourcesCharacterSunShangxiangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouSunShangxiangMobile { get; }
}

internal interface ICorePublicPileSourcesCharacterZhenJiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyZhenJi { get; }
}

internal interface ICorePublicPileSourcesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICorePublicPileSourcesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePublicPileSourcesCharacterCaoXianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCaoXian { get; }
}
