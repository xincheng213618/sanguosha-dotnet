using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<int, int> _mouZhitianCounts => ref _characterZhugeLiangMou.ReferenceToMouZhitianCounts;
    private ref readonly global::System.Collections.Generic.HashSet<int> _mouWujingAllowances => ref _characterZhugeLiangMou.ReferenceToMouWujingAllowances;
}
