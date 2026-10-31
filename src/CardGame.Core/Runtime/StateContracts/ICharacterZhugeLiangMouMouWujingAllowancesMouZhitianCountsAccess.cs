#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.HashSet<int> _mouWujingAllowances { get; }

    ref readonly global::System.Collections.Generic.Dictionary<int, int> _mouZhitianCounts { get; }
}
