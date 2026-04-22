using Soenneker.Telnyx.ClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Telnyx.ClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class TelnyxClientUtilTests : HostedUnitTest
{
    private readonly ITelnyxClientUtil _util;

    public TelnyxClientUtilTests(Host host) : base(host)
    {
        _util = Resolve<ITelnyxClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
