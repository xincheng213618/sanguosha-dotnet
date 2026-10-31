#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CharacterZhuLingOlDefinitions
{
internal static IReadOnlyList<CardKind> ZhanyiTrickKinds { get; } = Enum.GetValues<CardKind>().Where(kind => kind is not (CardKind.GeneralWeapon or CardKind.RedBloodBlade) && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Trick).Order().ToArray();
internal static IReadOnlyList<CardKind> ZhanyiBasicDamageKinds { get; } = [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash];
}
