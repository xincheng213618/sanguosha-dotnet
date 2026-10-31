namespace CardGame.Core;

// Construction does not resolve the router, whose own dependencies include the
// frame service. The module registry retains successful instances and retries
// failed construction using the same match's dependency guards.
internal sealed class NativeFrameRoutingProvider(ProgramRuntimeModuleRegistry modules)
{
    internal NativeFrameRoutingRuntime Runtime =>
        (NativeFrameRoutingRuntime)modules.Get(typeof(NativeFrameRoutingRuntime));
}
