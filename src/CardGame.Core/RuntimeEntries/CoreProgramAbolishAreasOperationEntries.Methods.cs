#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramAbolishAreasOperationEntries
{
    internal void AbolishProgramOwnerAreas(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones) => _resolve().AbolishProgramOwnerAreas(frame, zones);
}
