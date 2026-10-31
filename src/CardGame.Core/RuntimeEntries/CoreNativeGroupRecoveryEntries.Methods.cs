#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupRecoveryEntries
{
    internal void CompleteGroupRecoveryTarget() => _resolve().CompleteGroupRecoveryTarget();
    internal void RunOneGroupRecoveryStep() => _resolve().RunOneGroupRecoveryStep();
}
