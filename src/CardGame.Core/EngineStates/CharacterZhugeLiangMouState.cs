#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhugeLiangMouState : Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess
{
    private HashSet<int> _mouWujingAllowances = [];
    internal ref readonly HashSet<int> ReferenceToMouWujingAllowances => ref _mouWujingAllowances;

    private Dictionary<int, int> _mouZhitianCounts = [];
    internal ref readonly Dictionary<int, int> ReferenceToMouZhitianCounts => ref _mouZhitianCounts;

    ref readonly HashSet<int> Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances => ref _mouWujingAllowances;

    ref readonly Dictionary<int, int> Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouZhitianCounts => ref _mouZhitianCounts;
}
