#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDrawTwoBonusEntries
{
    internal int NativeDrawTwoCount(int sourceSeat) => _resolve().NativeDrawTwoCount(sourceSeat);
    internal bool HasNativeDrawTwoCapability { get => _resolve().HasNativeDrawTwoCapability; }
}
