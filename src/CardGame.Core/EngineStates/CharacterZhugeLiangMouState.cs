#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhugeLiangMouState : global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess
{
    private global::System.Collections.Generic.HashSet<int> _mouWujingAllowances = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToMouWujingAllowances => ref _mouWujingAllowances;

    private global::System.Collections.Generic.Dictionary<int, int> _mouZhitianCounts = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, int> ReferenceToMouZhitianCounts => ref _mouZhitianCounts;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances => ref _mouWujingAllowances;

    ref readonly global::System.Collections.Generic.Dictionary<int, int> global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouZhitianCounts => ref _mouZhitianCounts;
}
