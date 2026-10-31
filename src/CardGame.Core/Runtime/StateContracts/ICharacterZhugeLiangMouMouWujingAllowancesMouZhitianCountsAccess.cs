#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<int> _mouWujingAllowances { get; }

    ref readonly Dictionary<int, int> _mouZhitianCounts { get; }
}
