#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal IReadOnlyList<CardKind> MouLixianAddedNames(int owner) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNameAddedEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.OutputKind).Distinct().ToArray());
    internal IReadOnlyList<CardKind> MouLixianUsedNames(int owner) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNameUsedEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.OutputKind).Distinct().ToArray());
    internal bool MouLixianPreparationUnlocked(int owner) => CharacterHuangYueyingOlMouDefinitions.MouLixianAdditionalNames.All(MouLixianAddedNames(owner).Contains);
    internal bool MouLixianEndingUnlocked(int owner) => MouLixianUsedNames(owner).Count >= 3;
    internal IReadOnlyList<CardKind> MouLixianNames(int owner) => Array.AsReadOnly(new[] { CardKind.DrawTwo }.Concat(MouLixianAddedNames(owner)).Distinct().ToArray());
}
