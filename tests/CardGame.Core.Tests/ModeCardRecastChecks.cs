using CardGame.Core;
using CardGame.Modes.Standard;

internal static class ModeCardRecastChecks
{
    public static void ActivityRecastAndBasicFamiliesKeepIndependentBoundaries()
    {
        var ordinaryMode = new ContentModeDefinition("test:activity-card-rules", "Activity card rules", 4, 4,
            new Dictionary<string, int>());
        var ordinary = Rules(ordinaryMode);
        var hulao = Rules(ordinaryMode with { HulaoActivity = new() });
        var guanduMode = ordinaryMode with
        {
            GuanduActivity = new(new Dictionary<string, IReadOnlyList<string>>(), new Dictionary<string, string>(), [], [])
        };
        var dragonMode = ordinaryMode with { DragonBoatActivity = new(new Dictionary<string, IReadOnlyList<string>>()) };
        var guandu = Rules(guanduMode);
        var dragon = Rules(dragonMode);
        Require(!ordinary.HasHandEquipmentRecast && !ordinary.AllowsHandEquipmentRecast(EquipmentSlot.Weapon) &&
            !ordinary.AllowsNativePhysicalRecast(CardKind.UnitedEffort, CardKind.UnitedEffort) &&
            ordinary.BasicPlayConversionKinds.Count == 0,
            "An ordinary table installs no activity hand-equipment recast, native special-card recast or extra basic play family.");
        Require(hulao.HasHandEquipmentRecast && hulao.AllowsHandEquipmentRecast(EquipmentSlot.Weapon) &&
            Enum.GetValues<EquipmentSlot>().Where(slot => slot != EquipmentSlot.Weapon)
                .All(slot => !hulao.AllowsHandEquipmentRecast(slot)) &&
            !hulao.AllowsNativePhysicalRecast(CardKind.UnitedEffort, CardKind.UnitedEffort),
            "Hulao chooses the actual registered weapon slot without granting the special native-card recast of another activity.");
        Require(guandu.AllowsNativePhysicalRecast(CardKind.UnitedEffort, CardKind.UnitedEffort) &&
            !guandu.AllowsNativePhysicalRecast(CardKind.UnitedEffort, CardKind.IronChain) &&
            !guandu.AllowsNativePhysicalRecast(CardKind.Slash, CardKind.UnitedEffort) &&
            !guandu.AllowsHandEquipmentRecast(EquipmentSlot.Weapon),
            "Guandu requires its original physical/effective special-card identity; it grants neither a converted surrogate nor Hulao weapon permission.");
        Require(!ordinary.AllowsNativePhysicalRecast(CardKind.IronChain, CardKind.IronChain) &&
            !hulao.AllowsNativePhysicalRecast(CardKind.IronChain, CardKind.IronChain) &&
            !guandu.AllowsNativePhysicalRecast(CardKind.IronChain, CardKind.IronChain),
            "Ordinary native Iron Chain recast remains in its existing generic payment path rather than acquiring an activity exception.");
        Require(guandu.BasicPlayConversionKinds.SequenceEqual([CardKind.Intoxication]) &&
            !guandu.AllowsBasicPlayConversion(CardKind.Zong) &&
            dragon.BasicPlayConversionKinds.SequenceEqual([CardKind.Zong, CardKind.RealgarWine]) &&
            !dragon.AllowsBasicPlayConversion(CardKind.Intoxication),
            "The two native content adapters activate only the basic output families selected by the match's immutable profile.");
        var composed = Rules(guanduMode with { HulaoActivity = new(), DragonBoatActivity = dragonMode.DragonBoatActivity });
        Require(composed.AllowsHandEquipmentRecast(EquipmentSlot.Weapon) &&
            composed.AllowsNativePhysicalRecast(CardKind.UnitedEffort, CardKind.UnitedEffort) &&
            composed.BasicPlayConversionKinds.SequenceEqual([CardKind.Zong, CardKind.RealgarWine, CardKind.Intoxication]) &&
            composed.ReplaceOrdinaryTrick(CardKind.FiveGrains) == CardKind.AllianceFeast &&
            composed.TryReplaceBasicConversion(CardKind.Alcohol, out var output) && output == CardKind.Intoxication &&
            !composed.TryReplaceBasicConversion(CardKind.Peach, out _),
            "Composed fixtures keep independent hand-recast and play-family activation while retaining Hulao-first ordinary names and Guandu-first source-program replacement.");

        var selectedKinds = new List<CardKind> { CardKind.RealgarWine };
        var custom = new ModeCardRules(false, new OriginalNames(), null, new AllNames(), [],
            basicPlayConversionKinds: selectedKinds);
        selectedKinds.Clear();
        Require(custom.AllowsBasicPlayConversion(CardKind.RealgarWine) && !custom.AllowsBasicPlayConversion(CardKind.Zong) &&
            custom.BasicPlayConversionKinds is System.Collections.IList { IsReadOnly: true } &&
            ordinary.BasicPlayConversionKinds.Count == 0,
            "Output-family selection is frozen per profile and may select one supported native output without sharing a mode flag or caller-owned list.");
    }

    private static ModeCardRules Rules(ContentModeDefinition mode) => StandardModeRuleProfiles.Create(mode,
        new GameOptions { PlayerCount = 4, HumanSeat = 0 }).Cards;
    private sealed class OriginalNames : IModeOrdinaryTrickRule
    { public CardKind? Replace(CardKind kind) => kind; }
    private sealed class AllNames : IModeCardNameRule
    { public bool IsAllowed(CardKind kind) => true; }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
