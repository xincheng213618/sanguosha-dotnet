using CardGame.Core;

namespace CardGame.Modes.Standard;

internal static class StandardModeCardProfiles
{
    internal static ModeCardRules Create(ContentModeDefinition mode)
    {
        var additions = new List<ModeAdditionalOrdinaryTrick>();
        if (mode.HellActivity is not null) additions.Add(new(CardKind.ReturnSoul, ".activity-return-soul"));
        if (mode.WenheActivity is not null)
            additions.AddRange(StandardModeRuleProfiles.WenheOrdinaryTrickKinds.Select(kind => new ModeAdditionalOrdinaryTrick(kind, ".activity-" + kind, 1)));
        return new(mode.ShenwuActivity is not null || mode.HulaoActivity is not null || mode.HellActivity is not null ||
                mode.WenheActivity is not null || mode.DragonBoatActivity is not null || mode.GuanduActivity is not null,
            SelectOrdinaryTricks(mode), SelectBasicConversions(mode),
            mode.Id == WushuangMode.ModeId ? new WushuangCardNameRule() : new UnrestrictedCardNameRule(), additions, ["ol:mo-guifu"],
            DisabledOrdinaryTricks(mode));
    }

    private static IEnumerable<CardKind> DisabledOrdinaryTricks(ContentModeDefinition mode)
    {
        if (mode.GuanduActivity is null)
            foreach (var kind in new[] { CardKind.StoreGrain, CardKind.Reinforcements, CardKind.UnitedEffort }) yield return kind;
        if (mode.DragonBoatActivity is null)
            foreach (var kind in new[] { CardKind.SameBoat, CardKind.StriveUpstream, CardKind.AgainstCurrent }) yield return kind;
        if (mode.WenheActivity is null)
            foreach (var kind in StandardModeRuleProfiles.WenheOrdinaryTrickKinds) yield return kind;
        if (mode.HellActivity is null) yield return CardKind.ReturnSoul;
        if (mode.ShenwuActivity is null)
        {
            yield return CardKind.SowBeans;
            yield return CardKind.TransplantWood;
        }
        if (mode.HulaoActivity is null) yield return CardKind.AllianceFeast;
    }

    private static IModeOrdinaryTrickRule SelectOrdinaryTricks(ContentModeDefinition mode)
    {
        // Existing priority is significant when validating a composed fixture or an activity recipe.
        if (mode.ShenwuActivity is not null) return new ShenwuOrdinaryTrickRule();
        if (mode.HulaoActivity is not null) return new HulaoOrdinaryTrickRule();
        if (mode.GuanduActivity is not null) return new GuanduOrdinaryTrickRule();
        return mode.DragonBoatActivity is not null ? new DragonBoatOrdinaryTrickRule() : new OriginalOrdinaryTrickRule();
    }

    private static IModeBasicConversionRule? SelectBasicConversions(ContentModeDefinition mode)
    {
        if (mode.GuanduActivity is not null) return new GuanduBasicConversionRule();
        return mode.DragonBoatActivity is not null ? new DragonBoatBasicConversionRule() : null;
    }
}

internal sealed class OriginalOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind;
}
internal sealed class ShenwuOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    { CardKind.DrawTwo => CardKind.SowBeans, CardKind.BorrowedSword => CardKind.TransplantWood, _ => kind };
}
internal sealed class HulaoOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    { CardKind.FiveGrains => CardKind.AllianceFeast, CardKind.PeachGarden => null, _ => kind };
}
internal sealed class GuanduOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    {
        CardKind.FiveGrains => CardKind.StoreGrain, CardKind.PeachGarden => CardKind.Reinforcements,
        CardKind.BarbarianAssault => CardKind.UnitedEffort, _ => kind
    };
}
internal sealed class DragonBoatOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    {
        CardKind.DrawTwo => CardKind.SameBoat, CardKind.FiveGrains => CardKind.StriveUpstream,
        CardKind.PeachGarden => null, CardKind.BarbarianAssault => CardKind.AgainstCurrent,
        CardKind.ArrowBarrage => null, _ => kind
    };
}

internal sealed class GuanduBasicConversionRule : IModeBasicConversionRule
{
    public bool TryReplace(CardKind kind, out CardKind output)
    { output = kind == CardKind.Alcohol ? CardKind.Intoxication : kind; return output != kind; }
}
internal sealed class DragonBoatBasicConversionRule : IModeBasicConversionRule
{
    public bool TryReplace(CardKind kind, out CardKind output)
    {
        output = kind switch { CardKind.Peach => CardKind.Zong, CardKind.Alcohol => CardKind.RealgarWine, _ => kind };
        return output != kind;
    }
}

internal sealed class UnrestrictedCardNameRule : IModeCardNameRule
{
    public bool IsAllowed(CardKind kind) => true;
}
internal sealed class WushuangCardNameRule : IModeCardNameRule
{
    public bool IsAllowed(CardKind kind) => WushuangCatalogue.IsCardAllowed(kind);
}
