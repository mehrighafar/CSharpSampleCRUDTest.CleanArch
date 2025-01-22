using System.Diagnostics;

namespace CSharpSampleCRUDTest.CleanArch.Web.Configurations;

public class Instrumentation : IDisposable
{
  internal const string ActivitySourceName = "customer-server";
  internal const string ActivitySourceVersion = "1.0.0";

  public Instrumentation()
  {
    ActivitySource = new ActivitySource(ActivitySourceName, ActivitySourceVersion);
  }

  public ActivitySource ActivitySource { get; }

  public void Dispose()
  {
    ActivitySource.Dispose();
  }
}
